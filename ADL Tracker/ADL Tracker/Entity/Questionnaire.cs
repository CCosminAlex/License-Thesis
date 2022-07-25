using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name: "questionnaire")]
    public class Questionnaire
    {
        [Key]
        [Required]
        public string QuestionnaireId { get; set; }

        public Elder Patient { get; set; }

        public DateTime CompletedDate { get; set; }

        public List<PatientAnswer> PatientAnswers { get; set; }

        public double TotalScore { get; set; }
    }
}