using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Repository;
using ADL_Tracker.Service;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    public class ElderController : ControllerBase
    {
        private ElderRepository elderRepository;
        private readonly MonitoringDataRepository monitoringDataRepository;

        public ElderController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.elderRepository = new ElderRepository(dbContext, mapper);
            monitoringDataRepository = new MonitoringDataRepository(dbContext);
        }
        // GET:api/<ElderController>/GetAll
        [HttpGet("/GetAll/{id}")]
        public IEnumerable<ElderByDoctorDto> GetAll(string id)
        {
            return elderRepository.AllEldersOfADoctor(id);
        }

        // GET api/<ElderController>/5
        [HttpGet("{id}")]
        public ElderDto Get(string id)
        {
           return elderRepository.GetElderById(id);
            
        }

        // POST api/<ElderController>
        [HttpPost]
        public async Task<IActionResult> Post(string doctorId, [FromBody] ElderDto elderDto)
        {
            
            return Ok();
        }

        // PUT api/<ElderController>/5
        [HttpPut]
        public void Put([FromBody] EditElderDto elderDto)
        {
            elderRepository.Edit(elderDto);
        }

        // DELETE api/<ElderController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            elderRepository.Delete(id);
        }

        [HttpPost("/MonitoringData")]
        public IEnumerable<TimelineFormated> GetMonitoringDatas([FromBody] IntervalMonitoringData data)
        {
            return  monitoringDataRepository.GetMonitoringDatas(data);
        }

    }
}
