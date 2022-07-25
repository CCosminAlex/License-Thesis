using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    public class QuestionnaireDto
    {

        public string QuestionnaireId { get; set; }

        public string PatientId { get; set; }

        public DateTime CompletedDate { get; set; }

        public double TotalScore { get; set; }
    }
}
