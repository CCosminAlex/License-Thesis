using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    [Serializable]
    public class Day
    {
        public List<Details> DayDetails { get; set; }
    }
}
