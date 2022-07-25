using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Processing
{
    class ActivityProbabilityModel
    {
        public List<string> StartActivity { get; set; }
        public List<string> EndActivity { get; set; }
        public List<ActivityPerDay> activityPerDays { get; set; }

       // List<int> probabilityOfDaysLength = new List<int>();

        public ActivityProbabilityModel()
        {
            StartActivity = new List<string>();
            EndActivity = new List<string>();
            activityPerDays = new List<ActivityPerDay>();
        }
    }
}
