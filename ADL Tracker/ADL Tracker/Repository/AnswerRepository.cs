using ADL_Tracker.Entity;
using ADL_Tracker.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class AnswerRepository
    {
        private ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public AnswerRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;

        }

        public void Create(Answer answer)
        {
            answer.AnswerId = Guid.NewGuid().ToString();
                        
            dbContext.Answers.Add(answer);
            dbContext.SaveChanges();
        }

        public List<Answer> GetAll(string questionId)
        {
            
           return dbContext.Answers.Where(a=>a.QuestionId==questionId).ToList();
            
        }

        
    }
}
