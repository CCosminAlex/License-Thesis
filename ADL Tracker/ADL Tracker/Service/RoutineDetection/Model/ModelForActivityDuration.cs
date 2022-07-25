using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Processing
{
    public class ModelForActivityDuration
    {
        public string ActivityName { get; set; }
        public DateTime Day { get; set; }
        public string PartOfTheDay { get; set; }

        public double Duration { get; set; }
    }
}
