using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    public class Answer
    {
        [Key]
        public string AnswerId { get; set; }
        [Required]
        public string Text { get; set; }

        public double Score { get; set; }
        
        public string QuestionId { get; set; }
    }
}
