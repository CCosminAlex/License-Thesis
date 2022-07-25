using ADL_Tracker.Entity;
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
    public class QuestionController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository answerRepository;


        public QuestionController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this._questionRepository = new QuestionRepository(dbContext, mapper);
            answerRepository = new AnswerRepository(dbContext, mapper);

        }

        //GET: api/<QuestionController>
        [HttpGet]
        public IEnumerable<Question> Get()
        {
            return _questionRepository.GetAll();
        }

        // GET api/<QuestionController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "atat";
        }

        // POST api/<QuestionController>
        [HttpPost]
        public void Post([FromBody] Question question)
        {
            _questionRepository.Create(question);
        }

        [HttpPost("api/Answer")]
        public void Post([FromBody] Answer answer)
        {
            answerRepository.Create(answer);
        }

        [HttpGet("api/Answer/{qid}")]
        public IEnumerable<Answer> GetAnswer(string qid)
        {
            return answerRepository.GetAll(qid);
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuestionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}