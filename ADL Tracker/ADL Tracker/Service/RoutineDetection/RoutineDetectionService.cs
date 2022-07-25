using ADL_Tracker.Entity;
using ADL_Tracker.Repository;
using Pre_Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Service.RoutineDetection
{
    public class RoutineDetectionService
    {
        private readonly MonitoringDataRepository _monitoringDataRepository;
        private readonly TestingDaysRepo testingDaysRepo;
        private readonly Helper helper;
        private readonly QuestionnaireRepository _questionnaireRepository;
        private readonly RecommendationRepository _recommendationRepository;
        private Individ Routine { get; set; }
        public RoutineDetectionService(ApplicationDbContext dbContext)
        {
            _monitoringDataRepository = new MonitoringDataRepository(dbContext);
            testingDaysRepo = new TestingDaysRepo(dbContext);
            helper = new Helper();
            _questionnaireRepository = new QuestionnaireRepository(dbContext);
            _recommendationRepository = new RecommendationRepository(dbContext);
        }

        public Individ HarrisHawksAlgorithm(string id, int populationSize)
        {
            
            var monitoringdatas = _monitoringDataRepository.GetAllMonitoringDatas(id, 7);
            this.Routine =helper.HarrisHawksAlgorithm(10, monitoringdatas);
            return Routine;
        }

        public List<ModelForActivityDuration> Duration(string id, int populationSize)
        {
            var monitoringdatas = _monitoringDataRepository.GetAllMonitoringDatas(id, populationSize);
            var routineDuration= helper.DurationRoutine(monitoringdatas);
            var activity = routineDuration.Select(x => x.ActivityName).Distinct().ToList();
            var partofday = routineDuration.Select(x => x.PartOfTheDay).Distinct().ToList();
            List<ModelForActivityDuration> modelForActivityDuration = new List<ModelForActivityDuration>();
            foreach (var p in partofday)
            {
                foreach(var a in activity)
                {
                    var x = routineDuration.FindAll(x => x.ActivityName == a && x.PartOfTheDay == p).Sum(z=>z.Duration);
                    modelForActivityDuration.Add(new ModelForActivityDuration { ActivityName = a, PartOfTheDay = p, Duration = x / populationSize });
                }
            }
            return modelForActivityDuration.Where(x=>x.Duration != 0).ToList();
        }

        public Recommendation DetectDeviation(DateTime date,string patientId, Individ routine)
        {
            var questionnaireScore = _questionnaireRepository.GetLastQuestionnaireScore(patientId);
            Recommendation recommandation = new Recommendation() { Text = "", PatientId = patientId, QuestionnaireScore=questionnaireScore, IsDeviated=true, Date = date, Deviation="Order"};

            if (routine != null)
            {
                var monitoringdatas = _monitoringDataRepository.GetAllMonitoringDatas(patientId, 14);
                var day = testingDaysRepo.GetTestingDay(date);
                var result = helper.IsNormal(routine.Activities, day, monitoringdatas);
                if (helper.CosineSimilarity(routine, day.ToArray()) > 0.77)
                {
                    
                    recommandation.IsDeviated = result.IsDeviated;
                    recommandation.Deviation = result.Deviation;

                }
                recommandation.SleepHours = result.SleepHours;
                recommandation.TakeMedication = result.TakeMedication;
                
                _recommendationRepository.Add(recommandation);
                return recommandation;
            }
            return null;
           
        }

        public List<Recommendation> GetRecommendations(string patientId)
        {
            return _recommendationRepository.GetAll(patientId);
        }
        public List<Recommendation> GetFinalRecommendations(string patientId)
        {
            return _recommendationRepository.GetWithRecommendation(patientId);
        }

        public List<Recommendation> GetWekaRecommendation(string patientId)
        {
            return _recommendationRepository.GetByDate(patientId);
        }

        public void UpdateRecommendationText(DateTime dateTime, string patientId, string Text)
        {
            _recommendationRepository.Edit(dateTime, patientId, Text);
        }

        public Recommendation GetLastRecommendation(string patientId)
        {
            return _recommendationRepository.GetLast( patientId);
        }



    }
}
