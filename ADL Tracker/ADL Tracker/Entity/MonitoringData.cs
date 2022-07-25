using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    public class MonitoringData
    {
        [Key]
        public string MonitoringDataId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string ActivityName { get; set; }

        public string ElderId { get; set; }
    }
}
