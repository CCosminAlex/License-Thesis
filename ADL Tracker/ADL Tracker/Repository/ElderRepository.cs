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
    public class ElderRepository
    {
        private ApplicationDbContext dbContext;
        private readonly IMapper _mapper;

        public ElderRepository(ApplicationDbContext DbContext, IMapper mapper)
        {
            dbContext = DbContext;
            _mapper = mapper;

        }

        public void CreateElder(Elder elder)
        {
            
            
            
           
            dbContext.Elders.Add(elder);
            dbContext.SaveChanges();
        }

        public List<ElderByDoctorDto> AllEldersOfADoctor(string id)
        {
            
            var elders = dbContext.Elders.Join(dbContext.Users, elder => elder.Id, user => user.Elder.Id,
                                      (elder, user) => new ElderByDoctorDto{ Id = elder.Id,FirstName = user.FirstName, MiddleName = user.MiddleName, LastName = user.LastName, CNP = elder.CNP, DoctorId = elder.DoctorId })
                                      .Where(e => e.DoctorId == id).ToList();

            return elders;
        }

        public ElderDto GetElderById(string id)
        {
            var elder = dbContext.Elders.FirstOrDefault(e => e.Id == id);
            var user = dbContext.Users.FirstOrDefault(u => u.Elder.Id == id);
            List<PatientDisease> diseases = dbContext.PatientDiseases.Where(d => d.PatientId == id).ToList();
            List<Disease> diseases1 = new List<Disease>();
            foreach(PatientDisease patientDisease in diseases)
            {
                diseases1.Add(dbContext.Diseases.FirstOrDefault(d => d.DiseaseId == patientDisease.DiseaseId));
            }
            ElderDto elderDto = new ElderDto();
            elderDto.Id = elder.Id;
            elderDto.FirstName = user.FirstName;
            elderDto.MiddleName = user.MiddleName;
            elderDto.LastName = user.LastName;
            elderDto.PhoneNumber = user.PhoneNumber;
            elderDto.Email = user.Email;
            elderDto.BirthDate = elder.BirthDate;
            elderDto.CNP = elder.CNP;
            elderDto.Address = elder.Address;
            elderDto.EmergencyContact = elder.EmergencyContact;
            elderDto.EmergencyContactPhoneNumber = elder.EmergencyContactPhoneNumber;
            elderDto.PatientDisease = (List<PatientDisease>)diseases;
            elderDto.Disease = (List<Disease>)diseases1;
            return elderDto;
        }

        public void Edit(EditElderDto elderDto)
        {
            //PatientDisease
            var elder = dbContext.Elders.FirstOrDefault(e=>e.Id.Equals(elderDto.Id));
            var user = dbContext.Users.FirstOrDefault(u => u.Elder.Id == elderDto.Id);
            user.FirstName = elderDto.FirstName;
            user.MiddleName = elderDto.MiddleName;
            user.LastName = elderDto.LastName;
            user.PhoneNumber = elderDto.PhoneNumber;
            user.Email = elderDto.Email;
            user.UserName = elderDto.Email;
            elder.BirthDate = DateTime.Parse(elderDto.BirthDate);
            elder.CNP = elderDto.CNP;
            elder.Address = elderDto.Address;
            elder.EmergencyContact = elderDto.EmergencyContact;
            elder.EmergencyContactPhoneNumber = elderDto.EmergencyContactPhoneNumber;
            
            dbContext.Elders.Update(elder);
            dbContext.Users.Update(user);
            dbContext.SaveChanges();

        }

        public void Delete(string id)
        {
            var elder = dbContext.Elders.FirstOrDefault(e => e.Id.Equals(id));
            var user = dbContext.Users.FirstOrDefault(e => e.Elder.Id.Equals(id));

            DeleteElderStuff(elder.Id);
            dbContext.Elders.Remove(elder);
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }

        public ElderDto Get(string id)
        {
            return _mapper.Map<ElderDto>(dbContext.Elders.FirstOrDefault(e => e.Id == id));
        }
        private void DeleteElderStuff(string elderId)
        {
            List<Questionnaire> questionnaires = new List<Questionnaire>();
            List<PatientAnswer> patientAnswers = new List<PatientAnswer>();
            List<PatientDisease> patientDiseases = new List<PatientDisease>();
            List<MonitoringData> monitoringDatas = new List<MonitoringData>();
            questionnaires = dbContext.Questionnaires.Where(x => x.Patient.Id == elderId).ToList();
            patientAnswers = dbContext.PatientAnswers.ToList();
            patientDiseases = dbContext.PatientDiseases.Where(x => x.PatientId == elderId).ToList();
            monitoringDatas = dbContext.MonitoringDatas.Where(x => x.ElderId == elderId).ToList();
            foreach (var r in questionnaires)
            {
                foreach(var pa in patientAnswers)
                {
                    if(r.QuestionnaireId == pa.QuestionnaireId)
                    {
                        dbContext.PatientAnswers.Remove(pa);
                    }
                }
                dbContext.Questionnaires.Remove(r);
                
            }
            foreach(var pd in patientDiseases)
            {
                dbContext.PatientDiseases.Remove(pd);
            }
            foreach(var md in monitoringDatas)
            {
                dbContext.MonitoringDatas.Remove(md);
            }
            dbContext.SaveChanges();
        }
    }
}
