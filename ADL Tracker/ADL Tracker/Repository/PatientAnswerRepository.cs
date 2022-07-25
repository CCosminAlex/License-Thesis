using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class PatientAnswerRepository
    {
        private ApplicationDbContext dbContext;

        private readonly IMapper _mapper;

        public PatientAnswerRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;


        }
        public double Create(PatientAnswerDto patientAnswerDto)
        {
            patientAnswerDto.PatientAnswerId = Guid.NewGuid().ToString();
            var answer = dbContext.Answers.FirstOrDefault(a => a.AnswerId == patientAnswerDto.AnswerId);
            var patientAnswer = _mapper.Map<PatientAnswer>(patientAnswerDto);
            patientAnswer.Score = answer.Score;
            patientAnswer.Question = dbContext.Questions.FirstOrDefault(q => q.QuestionId == patientAnswerDto.QuestionId);
            dbContext.PatientAnswers.Add(patientAnswer);
            dbContext.SaveChanges();
            return patientAnswer.Score;
        }
    }
}
