using ADL_Tracker.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    [Serializable]
    public class RegisterElderDto
    {

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Password { get; set; }

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


        public IFormFile MonitoringFile { get; set; }
        public List<PatientDisease> Disease { get; set; }
        // public Questionnaire Questionnaire { get; set; }
        public string DoctorId { get; set; }
        public string Address { get; set; }
    }
}
