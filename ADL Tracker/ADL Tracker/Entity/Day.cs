using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Entity
{
    [Serializable]
    public class Day
    {
        public List<TestingDays> Days { get; set; }
    }
}
