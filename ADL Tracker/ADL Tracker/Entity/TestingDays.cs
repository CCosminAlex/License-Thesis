using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Serializable]
    public class TestingDays
    {
        [Key]
        public string TestingDaysId { get; set; }
        [Required]
        public DateTime Start_date { get; set; }
        [Required]
        public DateTime End_date { get; set; }
        [Required]
        public string Activity { get; set; }
    }

}
