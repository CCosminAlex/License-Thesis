using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Table(name: "question")]
    public class Question
    {
        [Key]
        [Required]
        public string QuestionId { get; set; }

        [Required]
        public string Statement { get; set; }

        public List<Answer> Answers { get; set; }
    }
}