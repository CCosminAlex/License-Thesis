using ADL_Tracker.Entity;
using ADL_Tracker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class RecommendationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RecommendationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Recommendation recommendation)
        {
            if (_dbContext.Recommendations.FirstOrDefault(x => x.Date.Date == recommendation.Date.Date) == null)
            {
                _dbContext.Recommendations.Add(recommendation);
                _dbContext.SaveChanges();
            }
        }

        public void Edit(DateTime date, string patientId, string Text)
        {
            var reccomendation = _dbContext.Recommendations.FirstOrDefault(x => x.Date == date && x.PatientId == patientId);
            reccomendation.Text = Text;
            _dbContext.Recommendations.Update(reccomendation);
            _dbContext.SaveChanges();
        }

        public List<Recommendation> GetAll(string patientId)
        {
            return _dbContext.Recommendations.Where(x => x.PatientId == patientId).ToList();
        }

        public List<Recommendation> GetWithRecommendation(string patientId)
        {
            return _dbContext.Recommendations.Where(x => x.PatientId == patientId && x.Text!=null).ToList();
        }



        public List<Recommendation> GetByDate(string patientId)
        {
            return _dbContext.Recommendations.Where(x => x.Text == "" && x.PatientId == patientId).ToList();
        }

        public Recommendation GetLast(string patientId)
        {
            return _dbContext.Recommendations.Where(x=>x.PatientId == patientId && x.Text!=null).OrderByDescending(y=>y.Date).FirstOrDefault();
        }
    }
}
