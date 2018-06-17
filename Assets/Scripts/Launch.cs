using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
public class Launch : MonoBehaviour {

    [Header("Simulation Parameters")]
        [Rename("Population")]
        [Tooltip("Number of organisms in every iteration.")]
        public int numberofOrganisms = 12;
        [Rename("Iterations")]
        [Tooltip("Number of genetic algorithm iterations.")]
        public int numberOfIterations = 20;
        [Rename("Iteration Duration")]
        [Tooltip("Number of clock ticks during iteration. During each clock tick a muscle movements are performed by the organism.")]
        public int iterationLength = 50;
        [Rename("Mutations")]
        [Tooltip("Number of mutations performed on random organisms in the population.")]
        public int numberOfMutations = 1;

    [Header("Genetic Algorithm Parameters")]
        [Rename("Fitnes Score Ratio")]
        [Tooltip("Magnitude defining the diversity level of the score according to the distance that organism has reached. (Typycally set to 2 or 3)")]
        public int fitnesDeterminationScale = 2;
        [Rename("Parent Selection Ratio")]
        [Tooltip("Magnitude definind how the differenc between probabilities will be amplified depending on the fitness score. (Typycally set to 3)")]
        public int parentProbabilityScale = 3;

    [Header("Organisms Parameters")]
        [Rename("Min Complexity")]
        [Tooltip("Minimum number of parts that the organism can consist of.")]
        public int minComplexity = 2;
        [Rename("Max Complexity")]
        [Tooltip("Maximum number of parts that the organism can consist of.")]
        public int maxComplexity = 10;
        [Rename("Min Muscle Strength")]
        [Tooltip("Minimum number of parts that the organism can consist of.")]
        public int minMotorForce = 10;
        [Rename("Max Muscle Strength")]
        [Tooltip("Maximum number of parts that the organism can consist of.")]
        public int maxMotorForce = 200;
        [Rename("Connection Probability")]
        [Tooltip("The probability of connecting two existing nodes with bone.")]
        public float oldNodeChoiceThreashold = 0.85f;
        [Rename("Min Distance")]
        [Tooltip("Minimum distance, from last selected node, of the newly generated node.")]
        public int minDistance = 1;
        [Rename("Max Distance")]
        [Tooltip("Maximum possible distance, from last selected node, of the newly generated node.")]
        public int randomScale = 4;
    void Start()
    {
        Simulator Sim = new Simulator(numberofOrganisms, numberOfIterations, iterationLength, numberOfMutations, minComplexity, maxComplexity, minMotorForce, maxMotorForce, fitnesDeterminationScale, parentProbabilityScale, oldNodeChoiceThreashold, minDistance, randomScale); // Create and run simulation with given parameters
        int counter = 0;
        while (counter <= Sim.GetNumberOfIterations())
        {
            Debug.Log(counter);
            //logger.Info("Running Iteration " + counter++);
            //Debug.Log("Running Iteration " + counter++);
            //Sim.RunIteration();
            //Sim.LogChanges();
        }
        //logger.Info("Simulation completed!");
        //Debug.Log("Simulation completed!");
    }
}
