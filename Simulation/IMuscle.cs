using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public interface IMuscle
    {
        int ID { get; set; }
        // first and second bone id
        Tuple<MuscleAnchorPoint, MuscleAnchorPoint> AnchorPoints { get; set; }
        double MaxStrength { get; set; }
        double MinLength { get; set; }
        double InitLength { get; set; }
        double CurrentLength { get; set; } 
        double MaxLength { get; set; }
        double Flexibility { get; set; }

        void Contract(double clock, double strengthRatio);
    }
}
