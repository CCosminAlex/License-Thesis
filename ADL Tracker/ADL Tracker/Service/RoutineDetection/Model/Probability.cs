using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Processing
{
    public class Probability
    {
        public string Activity { get; set; }

        public int Prob { get; set; }

        public double ProbNext { get; set; }

        public List<Probability> Next { get; set; }
    }
}
