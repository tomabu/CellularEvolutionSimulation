using Eppy;

namespace Simulation
{
    public interface IJoint
    {
        int ID { get; set; }
        Tuple<int, int> BonesID { get; set; } 
    }
}
