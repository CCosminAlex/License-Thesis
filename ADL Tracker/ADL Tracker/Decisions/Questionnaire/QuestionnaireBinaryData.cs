using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Decisions
{
    public class QuestionnaireBinaryData
    {
        [LoadColumn(0)]
        public Guid Question { get; set; }

        [LoadColumn(1,5)]
        [VectorType(5)]
        public Guid Answer { get; set; }
    }
}
