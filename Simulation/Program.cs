using System;

namespace Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get simulation parameters
            Console.Write("Number of organisms in Population: ");
            int numberofOrganisms = 12; // Convert.ToInt32(Console.ReadLine());
            Console.Write("Simulation Iterations Number: ");
            int numberOfIterations = 20; // Convert.ToInt32(Console.ReadLine());
            Console.Write("Simulation Iteration Duration (ms): ");
            int iterationLength = 50; // Convert.ToInt32(Console.ReadLine());
            Console.Write("Target place to reach: ");
            string[] tokens = "3 3 3".Split(); //Console.ReadLine().Split();
            float[] target = new float[3] { Convert.ToInt32(tokens[0]), Convert.ToSingle(tokens[1]), Convert.ToSingle(tokens[2]) };
            Simulator Sim = new Simulator(numberofOrganisms,numberOfIterations,iterationLength,target); // Create and run simulation with parameters
            int counter = 0;
            while(counter <= Sim.GetNumberOfIterations())
            {
                Console.WriteLine("Running Iteration " + counter++);
                Sim.RunIteration();
                //Sim.LogChanges();
            }
            Console.WriteLine("Simulation completed!");
            Console.ReadKey();
        }
    }
}
