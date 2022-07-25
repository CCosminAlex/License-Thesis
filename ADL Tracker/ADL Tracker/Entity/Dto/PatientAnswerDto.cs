using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    [Serializable]
    public class PatientAnswerDto
    {
        public string PatientAnswerId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
        public string QuestionnaireId { get; set; }
        public double Score { get; set; }
    }
}
