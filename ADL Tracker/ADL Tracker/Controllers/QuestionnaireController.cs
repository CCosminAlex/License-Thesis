using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Repository;
using ADL_Tracker.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADL_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly QuestionnaireRepository questionnaireRepository;

        public QuestionnaireController(ApplicationDbContext dbContext, IMapper mapper)
        {
            questionnaireRepository = new QuestionnaireRepository(dbContext);
        }
        // GET: api/<QuestionnaireController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<QuestionnaireController>/5
        [HttpGet("{id}")]
        public List<ViewQuestionnaireDto> Get(string id)
        {
           return questionnaireRepository.GetAll(id);
           
        }

        // POST api/<QuestionnaireController>
        [HttpPost]
        public IActionResult Post([FromBody] PatientIdClass patientId)
        {
            var id=questionnaireRepository.Create(patientId.PatientId);
            return Ok(id);
        }

        // PUT api/<QuestionnaireController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuestionnaireController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
