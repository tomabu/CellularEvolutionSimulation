using Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main class representing organisms in the simulation.
/// </summary>
public class Organism {
    public Vector3 CurrentPosition { get; set; }
    public Vector3 LastPosition {get; set;}
    public byte[] Chromosome { get; }
    public int ID { get; }

    public Organism(int in_id, byte[] in_chromosome, System.Random random)
    {
        ID = in_id;
        Chromosome = in_chromosome;
        CurrentPosition = new Vector3( 0, 0, 0 );
        //Debug.Log("Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
    }

    public Organism(int in_id, byte[] in_chromosome, Vector3 in_currentPosition)//, double[] in_ma)
    {
        ID = in_id;
        Chromosome = in_chromosome;
        CurrentPosition = in_currentPosition;
        //Debug.Log("[" + ID + "]" + "Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
    }
    /// <summary>
    /// Function moving organism.
    /// </summary>
    /// <param name="iterationLength">Iteration length.</param>
    /// <param name="fromchromosome">Organism Factory.</param>
    public void Move(int iterationLength, FromChromosome fromchromosome)
    {
        LastPosition = CurrentPosition;
        var chromosome = new Chromosome(this.Chromosome);
        fromchromosome.Generate(chromosome, iterationLength);
    }

    /// <summary>
    /// Getting difference between initial sim position of organism and the last recorded.
    /// </summary>
    /// <returns>Difference between current and initial position of organism.</returns>
    public float GetDeltaPosition()
    {
        CurrentPosition = GlobalVariables.Position;
        return Vector3.Distance(CurrentPosition, LastPosition);
    }
}
