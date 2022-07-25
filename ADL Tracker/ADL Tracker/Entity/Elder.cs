using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name:"elder")]
    public class Elder
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string CNP { get; set; }
        [Required]
        public string EmergencyContact { get; set; }
        [Required]
        public string EmergencyContactPhoneNumber { get; set; }
        public byte[] MonitoringFile { get; set; }
        public List<PatientDisease> Disease { get; set; }
       // public Questionnaire Questionnaire { get; set; }
       public string DoctorId { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
