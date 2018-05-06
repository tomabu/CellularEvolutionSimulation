using System;

namespace Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get simulation parameters
            Console.Write("Number of organisms in Population: ");
            int numberofOrganisms = 10;// Convert.ToInt32(Console.ReadLine());
            Console.Write("Simulation Iterations Number: ");
            int numberOfIterations = 20;// Convert.ToInt32(Console.ReadLine());
            Console.Write("Simulation Iteration Duration (ms): ");
            int iterationLength = 50;// Convert.ToInt32(Console.ReadLine());
            Console.Write("Target place to reach: ");
            string[] tokens = "3 3 3".Split(); //Console.ReadLine().Split(); 
            Tuple<int, int, int> target = new Tuple<int, int, int>(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]));
            // Create and run simulation with parameters
            Simulator Sim = new Simulator(numberofOrganisms,numberOfIterations,iterationLength, target);
            int counter = 0;
            while(counter <= Sim.GetNumberOfIterations()) //Console.ReadKey().Key != ConsoleKey.Escape)
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
