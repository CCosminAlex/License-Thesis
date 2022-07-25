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
    public class PatientDiseaseRepository
    {
        #region Properties
        private ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public PatientDiseaseRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;

        }
        #endregion

        #region Methods

        public List<PatientDiseaseDto> GetAll(string patientId)
        {
            var all = dbContext.PatientDiseases.Where(pd => pd.PatientId == patientId).ToList();
            return _mapper.Map<List<PatientDiseaseDto>>(all);

        }
        public void Add(PatientDiseaseDto patientDiseaseDto)
        {
            patientDiseaseDto.PatientDiseaseId = Guid.NewGuid().ToString();
            var patientDisease = _mapper.Map<PatientDisease>(patientDiseaseDto);
            dbContext.PatientDiseases.Add(patientDisease);
            dbContext.SaveChanges();
        }

        public void Edit(PatientDiseaseDto patientDisease)
        {
            var patientDiseasefromDb = dbContext.PatientDiseases.FirstOrDefault(d => d.PatientDiseaseId.Equals(patientDisease.PatientDiseaseId));
            patientDiseasefromDb.Treatment = patientDisease.Treatment;
            patientDiseasefromDb.Discovered = patientDisease.Discovered;
            patientDiseasefromDb.Ended = patientDisease.Ended;
            dbContext.PatientDiseases.Update(patientDiseasefromDb);
            dbContext.SaveChanges();
        }

        public void Delete(string patientDiseaseId)
        {
            var patientDiseasefromDb = dbContext.PatientDiseases.FirstOrDefault(d => d.PatientDiseaseId.Equals(patientDiseaseId));
            dbContext.PatientDiseases.Remove(patientDiseasefromDb);
            dbContext.SaveChanges();
        }
        #endregion
    }


}
