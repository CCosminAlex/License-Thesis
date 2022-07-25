using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity.Dto
{
    public class ViewQuestionnaireDto
    {
        public string QuestionnaireId { get; set; }

        public List<QuestionnaireDetails> Details { get; set; }
        public double TotalScore { get; set; }

        public string DateTaken { get; set; }
    }

    public class QuestionnaireDetails {
        public string QuestionnaireId { get; set; }

        public string QuestionStatment { get; set; }

        public string Answer { get; set; }
    }

}
