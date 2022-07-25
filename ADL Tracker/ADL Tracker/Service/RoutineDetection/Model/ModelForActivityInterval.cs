using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Processing
{
    public class ModelForActivityInterval
    {
        public string ActivityName { get; set; }

        public string PartOfDay { get; set; }

        public TimeSpan Max { get; set; }

        public TimeSpan Min { get; set; }
    }

    
}
