using System.Collections.Generic;

namespace Simulation
{
    public interface IOrganism
    {
        int ID { get; set; }
        string Name { get; set; }
        //double Health { get; set; }
        double Clock { get; set; }
        List<IBone> Bones { get; set; }
        //List<IMuscle> Muscles { get; set; }
        List<IJoint> Joints { get; set; }

        float Move(float x, float y, float z);
    }
}
