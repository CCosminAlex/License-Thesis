using ADL_Tracker.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    public class ElderDto
    {

        
        public string Id { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "Only digits and length must be 13.")]
        public string CNP { get; set; }
        [Required]
        public string EmergencyContact { get; set; }
        [Required]
        public string EmergencyContactPhoneNumber { get; set; }
        
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        
        public List<PatientDisease> PatientDisease { get; set; }
        public List<Disease> Disease { get; set; }
        // public Questionnaire Questionnaire { get; set; }
        public string DoctorId { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
