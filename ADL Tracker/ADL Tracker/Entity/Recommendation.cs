using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name:"recommendation")]
    public class Recommendation
    {
        [Key]
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string PatientId { get; set; }
        public int SleepHours { get; set; }
        public bool TakeMedication { get; set; }
        public double QuestionnaireScore { get; set; }
        public bool IsDeviated { get; set; }

        public string Deviation { get; set; }
        public string Text { get; set; }
    }
}
