using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name: "patientAnswer")]
    public class PatientAnswer
    {

        [Key]
        public string PatientAnswerId { get; set; }

        [Required]
        public Question Question { get; set; }

        public string AnswerId { get;set;}

        public string QuestionnaireId { get;set; }

        public double Score { get; set; }
    }
}