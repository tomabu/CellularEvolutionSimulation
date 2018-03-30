using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class MuscleAnchorPoint
    {
        int ID { get; set; }
        int MuscleID { get; set; }
        Tuple<int, int> BonesID { get; set; }
        double Distance { get; set; } // from beginning
        double Angle { get; set; }
    }
}
