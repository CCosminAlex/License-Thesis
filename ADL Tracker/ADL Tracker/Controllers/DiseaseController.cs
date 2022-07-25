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
    public class DiseaseController : ControllerBase
    {
        private readonly DiseaseRepository _diseaseRepository;

        public DiseaseController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _diseaseRepository = new DiseaseRepository(dbContext, mapper);
        }

        // GET: api/<DiseaseController>
        [HttpGet]
        public IEnumerable<DiseaseDto> Get()
        {
            return _diseaseRepository.GetAll();
        }

        // GET api/<DiseaseController>/5
        [HttpGet("{id}")]
        public DiseaseDto Get(string id)
        {
            return _diseaseRepository.Get(id);
        }

        // POST api/<DiseaseController>
        [HttpPost]
        public void Post([FromBody] DiseaseDto disease)
        {
            disease.DiseaseId = Guid.NewGuid().ToString();
            _diseaseRepository.Create(disease);
        }

        // PUT api/<DiseaseController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] DiseaseDto diseaseDto)
        {
            diseaseDto.DiseaseId = id;
            _diseaseRepository.Edit(diseaseDto);
        }

        // DELETE api/<DiseaseController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _diseaseRepository.Delete(id);
        }
    }
}