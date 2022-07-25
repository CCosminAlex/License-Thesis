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
    public class DiseaseRepository
    {
        private ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public DiseaseRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;

        }

        public void Create(DiseaseDto diseaseDto)
        {
            diseaseDto.DiseaseId = Guid.NewGuid().ToString();
            var disease = _mapper.Map<Disease>(diseaseDto);
            dbContext.Diseases.Add(disease);
            dbContext.SaveChanges();
        }

        public List<DiseaseDto> GetAll()
        {
            return _mapper.Map<List<DiseaseDto>>(dbContext.Diseases.ToList());
        }

        public DiseaseDto Get(string id)
        {
            return _mapper.Map<DiseaseDto>(dbContext.Diseases.FirstOrDefault(d => d.DiseaseId.Equals(id)));
        }

        public void Edit(DiseaseDto diseaseDto)
        {
            var disease = dbContext.Diseases.FirstOrDefault(d => d.DiseaseId.Equals(diseaseDto.DiseaseId));
            disease.Description = diseaseDto.Description;
            disease.Name = diseaseDto.Name;
            dbContext.Diseases.Update(disease);
            dbContext.SaveChanges();

        }

        public void Delete(string id)
        {
            var disease = dbContext.Diseases.FirstOrDefault(d => d.DiseaseId.Equals(id));
            dbContext.Diseases.Remove(disease);
            dbContext.SaveChanges();
        }
    }
}