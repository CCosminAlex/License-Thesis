using ADL_Tracker.Entity;
using ADL_Tracker.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class QuestionRepository
    {
        private ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public QuestionRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;

        }

        public void Create(Question question)
        {
            question.QuestionId = Guid.NewGuid().ToString();
            dbContext.Questions.Add(question);
            dbContext.SaveChanges();
        }

        public List<Question> GetAll()
        {
            var questions=dbContext.Questions.ToList();
            foreach (Question q in questions)
            {
                var answers = dbContext.Answers.Where(x => x.QuestionId == q.QuestionId).ToList();
                q.Answers = answers;
            }
            return questions;
        }

    }
}