using System;
using System.Collections.Generic;

namespace Simulation
{
    class Organism : IOrganism
    {
        public Tuple<float, float, float> CurrentPosition { get; set; }
        public Tuple<float, float, float> MovementAbility { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public double Clock { get; set; }
        public List<IBone> Bones { get; set; }
        public List<IMuscle> Muscles { get; set; }
        public List<IJoint> Joints { get; set; }
        public Organism(int lid, Random random)
        {
            // Assigning unique ID
            ID = lid;
            // Setting current position to 0
            CurrentPosition = new Tuple<float, float, float>(0,0,0);
            // Generating random movement ability
            MovementAbility = new Tuple<float, float, float>((float) random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Console.WriteLine("Movement Ability: (" + MovementAbility.Item1.ToString("n3") + ", " +
                                                      MovementAbility.Item2.ToString("n3") + ", " +
                                                      MovementAbility.Item3.ToString("n3") + ")");
        }
        public float Move(float x, float y, float z)
        {
            CurrentPosition = new Tuple<float, float, float>(CurrentPosition.Item1 + MovementAbility.Item1,
                                                             CurrentPosition.Item2 + MovementAbility.Item2,
                                                             CurrentPosition.Item3 + MovementAbility.Item3);
            Console.WriteLine("Current Position: (" + CurrentPosition.Item1.ToString("n3") + ", " +
                                                      CurrentPosition.Item2.ToString("n3") + ", " +
                                                      CurrentPosition.Item3.ToString("n3") + ")");
            float deltaX = x - CurrentPosition.Item1;
            float deltaY = y - CurrentPosition.Item2;
            float deltaZ = z - CurrentPosition.Item3;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }
}
