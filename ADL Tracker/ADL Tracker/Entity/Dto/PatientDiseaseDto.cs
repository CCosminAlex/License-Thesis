using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    public class PatientDiseaseDto
    {
        public string PatientDiseaseId { get; set; }
        public string DiseaseId { get; set; }
 
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Treatment { get; set; }
        
        public DateTime Discovered { get; set; }
        public DateTime Ended { get; set; }
       
        public string PatientId { get; set; }
       
    }
}