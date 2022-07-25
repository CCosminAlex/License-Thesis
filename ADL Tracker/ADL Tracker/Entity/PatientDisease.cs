using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name:"patientDisease")]
    public class PatientDisease
    {
        [Key]
        [Required]
        public string PatientDiseaseId { get; set; }
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string DiseaseId { get; set; }
        [Required]
        public DateTime Discovered { get; set; }
        public DateTime Ended { get; set; }
        [Required]
        public string Treatment { get; set; }

    }
}
