using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    public class QuestionDto
    {
        public string QuestionId { get; set; }

        public string Statement { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
