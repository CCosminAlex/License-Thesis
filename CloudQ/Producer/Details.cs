using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    [Serializable]
    public class Details
    {
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Activity { get; set; }
    }
}
