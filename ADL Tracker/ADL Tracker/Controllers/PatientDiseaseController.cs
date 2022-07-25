using ADL_Tracker.Entity;
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
    public class PatientDiseaseController : ControllerBase
    {


        #region Properties
        private PatientDiseaseRepository patientDiseaseRepository;
        public PatientDiseaseController(ApplicationDbContext dbContext, IMapper mapper)
        {
            patientDiseaseRepository = new PatientDiseaseRepository(dbContext, mapper);
        }
        #endregion

        #region Methods




        // GET: api/<PatientDiseaseController>
        [HttpGet("{patientId}")]
        public IEnumerable<PatientDiseaseDto> GetAll(string patientId)
        {
            return patientDiseaseRepository.GetAll(patientId);
        }

        // GET api/<PatientDiseaseController>/5


        // POST api/<PatientDiseaseController>
        [HttpPost]
        public void Post([FromBody] PatientDiseaseDto patientDiseaseDto)
        {
            patientDiseaseRepository.Add(patientDiseaseDto);
        }

        // PUT api/<PatientDiseaseController>/5
        [HttpPut]
        public void Put([FromBody] PatientDiseaseDto patientDiseaseDto)
        {
            patientDiseaseRepository.Edit(patientDiseaseDto);
        }

        // DELETE api/<PatientDiseaseController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            patientDiseaseRepository.Delete(id);
        }
        #endregion
    }
}
