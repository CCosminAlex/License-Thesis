using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name:"disease")]
    public class Disease
    {
        [Key]
        [Required]
        public string DiseaseId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
