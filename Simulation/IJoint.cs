namespace Simulation
{
    public interface IJoint
    {
        int ID { get; set; }
        Pair<int, int> BonesID { get; set; } 
    }
}
