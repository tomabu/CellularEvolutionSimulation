using Eppy;

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
