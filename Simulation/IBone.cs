using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public interface IBone
    {
        int ID { get; set; }
        double Length { get; set; }
        double Mass { get; set; }
        double Diameter { get; set; }

        List<MuscleAnchorPoint> AnchorPoints { get; set; }
        IJoint UpperJoint { get; set; }
        IJoint LowerJoint { get; set; }
    }

}
