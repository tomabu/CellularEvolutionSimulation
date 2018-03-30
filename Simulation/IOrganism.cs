using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public interface IOrganism
    {
        int ID { get; set; }
        string Name { get; set; }
        //double Health { get; set; }
        double Clock { get; set; }
        List<IBone> Bones { get; set; }
        List<IMuscle> Muscles { get; set; }
        List<IJoint> Joints { get; set; }

        void Move(int x, int y, int z);
    }
}
