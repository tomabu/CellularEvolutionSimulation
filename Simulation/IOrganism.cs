using System.Collections.Generic;

namespace Simulation
{
    public interface IOrganism
    {
        int ID { get; }
        byte[] Chromosome { get; }
        //string Name { get; set; }
        //double Health { get; set; }
        double Clock { get; }
        //List<IBone> Bones { get; set; }
        //List<IMuscle> Muscles { get; set; }
        //List<IJoint> Joints { get; set; }

        float Move(float x, float y, float z);
    }
}