﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public interface IJoint
    {
        int ID { get; set; }
        Tuple<int, int> BonesID { get; set; } 
    }
}
