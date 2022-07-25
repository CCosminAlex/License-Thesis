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
    public class PatientAnswerController : ControllerBase
    {
        private PatientAnswerRepository patientAnswerRepository;
        private QuestionnaireRepository questionnaireRepository;
        public PatientAnswerController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.patientAnswerRepository = new PatientAnswerRepository(dbContext, mapper);
            questionnaireRepository = new QuestionnaireRepository(dbContext);

        }

        // GET: api/<PatientAnswerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PatientAnswerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PatientAnswerController>
        [HttpPost]
        public void Post([FromBody] PatientAnswerListDto answerListDto)
        {
            double totalScore = 0.0;
            string id="";
            foreach(PatientAnswerDto patientAnswerDto in answerListDto.PatientAnswerDtos)
            {
                
                totalScore += patientAnswerRepository.Create(patientAnswerDto);
                id = patientAnswerDto.QuestionnaireId;

            }
            questionnaireRepository.ComputeScore(id, totalScore);
            
        }

        [HttpPost("/postsimplu")]
        public void PostS([FromBody] PatientAnswerDto  patientAnswerDto)
        {
          
                patientAnswerRepository.Create(patientAnswerDto);
            

        }

        [HttpPost("/bplokb")]
        public void Post([FromBody] PatientAnswerDto patientAnswerDto)
        {
           

        }

        // PUT api/<PatientAnswerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PatientAnswerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
