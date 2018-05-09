using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class Organism : MonoBehaviour //TODO: Add IOrganism interface to Organism
    {
        public double[] CurrentPosition { get; } // Current position of an organism in environment.
        public double[] MovementAbility { get; } // Movement simulation
        public byte[] Chromosome { get; } // Chromosome of an organism
        public static int FeatureLength { get; } = 4;
        public int ID { get; } // Unique ID of an organism
        public double Clock { get; } // Internal clock of an organism
        public Organism(int lid, System.Random random) //TODO: Delete random generator from constructor.
        {
            Chromosome = new byte[12];
            random.NextBytes(Chromosome);
            ID = lid; // Assigning unique ID
            CurrentPosition = new double[3] { 0, 0, 0 }; // Setting current position to 0
            MovementAbility = new double[3] { random.NextDouble(), random.NextDouble(), random.NextDouble() }; // Generating random movement ability
            Console.WriteLine("Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
        }
        /// <summary>
        /// Creates an Organism.
        /// </summary>
        /// <param name="ch">Chromosome byte array.</param>
        /// <param name="lid">Unique ID of the organism.</param>
        /// <param name="random">Temporary random generator reference.</param>
        public Organism(byte[] ch, int lid, System.Random random) //TODO: Delete random generator from constructor.
        {
            Chromosome =  ch;
            ID = lid; // Assigning unique ID
            CurrentPosition = new double[3] { 0, 0, 0 }; // Setting current position to 0
            MovementAbility = new double[3] { random.NextDouble(), random.NextDouble(), random.NextDouble() }; // Generating random movement ability
            //Console.WriteLine("Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
        }
        /// <summary>
        /// Performs a move in the target direction.
        /// </summary>
        /// <param name="x">Target x axis.</param>
        /// <param name="y">Target y axis.</param>
        /// <param name="z">Target z axis.</param>
        /// <returns>Difference between target and organism position after movement.</returns>
        public float Move(float x, float y, float z)
        {

            CurrentPosition[0] = CurrentPosition[0] + MovementAbility[0];
            CurrentPosition[1] = CurrentPosition[1] + MovementAbility[1];
            CurrentPosition[2] = CurrentPosition[2] + MovementAbility[2];

            //Console.WriteLine("Current Position: (" + CurrentPosition[0].ToString("n3") + ", " + CurrentPosition[1].ToString("n3") + ", " + CurrentPosition[2].ToString("n3") + ")");
            double deltaX = x - CurrentPosition[0];
            double deltaY = y - CurrentPosition[1];
            double deltaZ = z - CurrentPosition[2];
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }
}
