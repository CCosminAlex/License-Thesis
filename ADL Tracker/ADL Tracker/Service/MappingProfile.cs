using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Elder, ElderDto>();
            CreateMap<ElderDto, Elder>();
            CreateMap<Disease, DiseaseDto>();
            CreateMap<DiseaseDto, Disease>();
            CreateMap<PatientAnswer, PatientAnswerDto>();
            CreateMap<PatientAnswerDto, PatientAnswer>();
            CreateMap<PatientDisease, PatientDiseaseDto>();
            CreateMap<PatientDiseaseDto, PatientDisease>();



        }
    }
}
