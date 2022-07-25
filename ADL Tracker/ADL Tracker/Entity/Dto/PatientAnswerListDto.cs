using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    [Serializable]
    public class PatientAnswerListDto
    {
        public List<PatientAnswerDto> PatientAnswerDtos { get; set; }
    }
}
