using System;
using UnityEngine;

using NLog;
namespace Simulation
{
    class Program : MonoBehaviour
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        void Start()
        {

            // Get simulation parameters
            Debug.Log("Number of organisms in Population: ");
            int numberofOrganisms = 12; // Convert.ToInt32(Console.ReadLine());
            Debug.Log("Simulation Iterations Number: ");
            int numberOfIterations = 20; // Convert.ToInt32(Console.ReadLine());
            Debug.Log("Simulation Iteration Duration (ms): ");
            int iterationLength = 50; // Convert.ToInt32(Console.ReadLine());
            Debug.Log("Target place to reach: ");
            string[] tokens = "3 3 3".Split(); //Console.ReadLine().Split();
            float[] target = new float[3] { Convert.ToInt32(tokens[0]), Convert.ToSingle(tokens[1]), Convert.ToSingle(tokens[2]) };
            Simulator Sim = new Simulator(numberofOrganisms, numberOfIterations, iterationLength, target); // Create and run simulation with parameters
            int counter = 0;
            while (counter <= Sim.GetNumberOfIterations())
            {
                logger.Info("Running Iteration " + counter++);
                //Debug.Log("Running Iteration " + counter++);
                Sim.RunIteration();
                //Sim.LogChanges();
            }
            logger.Info("Simulation completed!");
            //Debug.Log("Simulation completed!");
            Console.ReadKey();
        }
    }
}
