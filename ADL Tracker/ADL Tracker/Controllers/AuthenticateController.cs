using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Repository;
using ADL_Tracker.Service;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADL_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly MonitoringDataRepository monitoringDataRepository;
        private readonly ElderRepository elderRepository;


        public AuthenticateController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.elderRepository = new ElderRepository(applicationDbContext, mapper);
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            monitoringDataRepository = new MonitoringDataRepository(applicationDbContext);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            var user =  userManager.Users.Include(x=>x.Elder).FirstOrDefault(y=>y.UserName == model.Email);
            ElderDto x;
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                  
                if (user.Elder == null) 
                {
                    x = new ElderDto { Id = "No elder" };
                }
                else
                {
                     x = elderRepository.GetElderById(user.Elder.Id);
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));



                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {

                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    username = user.UserName,
                    email = user.Email,
                    role = userRoles[0],
                    id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    ElderId = x.Id,
                    

                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            Users user = new Users()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email

            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(UserRoles.Doctor))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));

            if (await roleManager.RoleExistsAsync(UserRoles.Doctor))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Doctor);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

       [HttpPost]
       [Route("registerElder/{docId}")]
       public async Task<IActionResult> RegisterElder(string docId, [FromForm] RegisterElderDto model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            Elder erlder = new Elder() { Id=Guid.NewGuid().ToString(), BirthDate = model.BirthDate, CNP = model.CNP, EmergencyContact = model.EmergencyContact, EmergencyContactPhoneNumber = model.EmergencyContactPhoneNumber, DoctorId=docId, Address=model.Address };
         
            Users user = new Users()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Elder = erlder

            };


           
            

            model.Password = model.FirstName[0].ToString().ToUpper() + model.LastName[1] + model.CNP.Substring(model.CNP.Length - 6) + '!';
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }
            

            userExists = await userManager.FindByEmailAsync(model.Email);
            
            var list = await ConvertMonoringFileToMonitoringData(model.MonitoringFile, erlder.Id);
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private async Task<List<MonitoringDataCSVDto>> ConvertMonoringFileToMonitoringData(IFormFile file, string elder_id)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                TextReader textReader = new StreamReader(memoryStream);

                var csv = new CsvReader(textReader, CultureInfo.InvariantCulture);

                var monitoringDataList = csv.GetRecords<MonitoringDataCSVDto>().ToList();
                monitoringDataRepository.InsertDataFromFile(monitoringDataList, elder_id);
                return monitoringDataList;
            }
        }
    }

}
