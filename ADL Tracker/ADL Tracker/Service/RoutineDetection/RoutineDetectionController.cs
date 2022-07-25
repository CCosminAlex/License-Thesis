using ADL_Tracker.Entity;
using ADL_Tracker.Repository;
using Microsoft.AspNetCore.Mvc;
using Pre_Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADL_Tracker.Service.RoutineDetection
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineDetectionController : ControllerBase
    {
        private const string URL = "http://localhost:8082/";
        private readonly RoutineDetectionService routineDetectionService;
       
        private Individ Routine { get; set; }

        public RoutineDetectionController(ApplicationDbContext dbContext)
        {
            routineDetectionService = new RoutineDetectionService(dbContext);
         
        }
        // GET: api/<RoutineDetectionController>
        [HttpGet]
        [Route("{id}/{days}")]
        public IEnumerable<ModelForActivityDuration> Get(string id, int days )
        {
            return routineDetectionService.Duration(id, days);
        }

        // GET api/<RoutineDetectionController>/5
        [HttpGet("{id}")]
        public Individ Get(string id)
        {
             
              this.Routine= routineDetectionService.HarrisHawksAlgorithm(id, 14);
            return Routine;

        }

        // GET api/<RoutineDetectionController>/5
        [HttpGet("allRecomendation/{id}")]
        public List<Recommendation> GetRecommandations(string id)
        {

           return routineDetectionService.GetRecommendations(id);

        }



        [HttpPost]
        [Route("detection/{date}/{patientId}")]
        public Recommendation Detection(string date, string patientId, [FromBody] Individ individ)
        {
            
            return routineDetectionService.DetectDeviation(DateTime.Parse(date), patientId, individ);
        }

        [HttpGet]
        [Route("recommendation/{patientId}")]
        async public Task<List<Recommendation>> AllRecommendation(string patientId)
        {

            var toSendList = routineDetectionService.GetWekaRecommendation( patientId);
            var dataObjects = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var toSendObject in toSendList)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/decision", toSendObject);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        dataObjects = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll


                    }
                    routineDetectionService.UpdateRecommendationText(toSendObject.Date, patientId, dataObjects);
                }
                catch(Exception e)
                {

                }
            }
            return routineDetectionService.GetFinalRecommendations(patientId);
        }

        //[HttpGet]
        //[Route("recommendation/{patientId}")]
        //public List<Recommendation> AllRecommendation(string patientId)
        //{

        //    return routineDetectionService.GetFinalRecommendations(patientId);
        //}

        [HttpGet]
        [Route("lastrecommendation/{patientId}")]
        public string LastRecommendation(string patientId)
        {

            return routineDetectionService.GetLastRecommendation(patientId).Text;
        }

        // POST api/<RoutineDetectionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RoutineDetectionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoutineDetectionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
