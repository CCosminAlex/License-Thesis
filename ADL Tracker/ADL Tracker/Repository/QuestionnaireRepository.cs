using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ADL_Tracker.Repository
{
    public class QuestionnaireRepository
    {
        private ApplicationDbContext dbContext;


        public QuestionnaireRepository(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
           

        }

        public string Create(string patientId)
        {
            Questionnaire questionnaire = new Questionnaire();
            questionnaire.CompletedDate = DateTime.Now;
            questionnaire.QuestionnaireId = Guid.NewGuid().ToString();
            var user = dbContext.Users.Where(p => p.Id == patientId).Include(p => p.Elder).FirstOrDefault();
            questionnaire.Patient = dbContext.Elders.FirstOrDefault(p => p.Id == user.Elder.Id);
            dbContext.Questionnaires.Add(questionnaire);
            dbContext.SaveChanges();
            return questionnaire.QuestionnaireId;
        }

        public void ComputeScore(string questionnaireId, double score)
        {
            var questionnaire = dbContext.Questionnaires.FirstOrDefault(q => q.QuestionnaireId == questionnaireId);
            questionnaire.TotalScore = score;
            dbContext.Questionnaires.Update(questionnaire);
            dbContext.SaveChanges();

        }

        public List<ViewQuestionnaireDto> GetAll(string patientId)
        {
            List<ViewQuestionnaireDto> viewQuestionnaireDtos = new List<ViewQuestionnaireDto>();
            var questionnaireList = dbContext.Questionnaires.Where(q => q.Patient.Id == patientId).OrderBy(a=>a.CompletedDate).ToList();
            
            foreach (Questionnaire questionnaire in questionnaireList)
            {
                ViewQuestionnaireDto viewQuestionnaireDto = new ViewQuestionnaireDto();
                viewQuestionnaireDto.QuestionnaireId = questionnaire.QuestionnaireId;
                viewQuestionnaireDto.Details = Details(questionnaire.QuestionnaireId);
                viewQuestionnaireDto.TotalScore = questionnaire.TotalScore;
                viewQuestionnaireDto.DateTaken = questionnaire.CompletedDate.ToString("dd-MM-yyyy");
                viewQuestionnaireDtos.Add(viewQuestionnaireDto);
            }
            
            
            return viewQuestionnaireDtos;
        }

        public double GetLastQuestionnaireScore(string patientId)
        {
            var result =dbContext.Questionnaires.Where(q => q.Patient.Id == patientId).OrderByDescending(a => a.CompletedDate).FirstOrDefault();
            if(result != null)
            {
                return result.TotalScore;
            }
            return 0;
        }

        public List<QuestionnaireDetails> Details(string id)
        {

            List<QuestionnaireDetails> answers = dbContext.PatientAnswers.Join(dbContext.Answers, pa => pa.AnswerId, a => a.AnswerId, (pa, a) => new { Text = a.Text, QuestionnaireId = pa.QuestionnaireId, Question = a.QuestionId })
                .Join(dbContext.Questions, a => a.Question, q => q.QuestionId, (a, q) => new QuestionnaireDetails { QuestionStatment = q.Statement, Answer = a.Text, QuestionnaireId = a.QuestionnaireId })
                .Where(p => p.QuestionnaireId == id).ToList();
            return answers;
        }


    }

}
