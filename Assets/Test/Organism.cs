using Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism {
    public Vector3 CurrentPosition { get; set; }
    public Vector3 LastPosition {get; set;}
    public byte[] Chromosome { get; }
    public int ID { get; }
    // public double InternalClock { get; }

    // For Testing Purposes Only
    //public static Vector3 MovementAbility { get; set;} // Movement simulation

    public Organism(int in_id, byte[] in_chromosome, System.Random random)
    {
        ID = in_id;
        Chromosome = in_chromosome;
        CurrentPosition = new Vector3( 0, 0, 0 );
        //MovementAbility = new Vector3( (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble() ); // Generating random movement ability
        //Debug.Log("Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
    }

    public Organism(int in_id, byte[] in_chromosome, Vector3 in_currentPosition)//, double[] in_ma)
    {
        ID = in_id;
        Chromosome = in_chromosome;
        CurrentPosition = in_currentPosition;
        //MovementAbility = in_ma;
        //Debug.Log("[" + ID + "]" + "Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
    }

    public void Move(int iterationLength, FromChromosome fromchromosome)
    {
        LastPosition = CurrentPosition;
        var chromosome = new Chromosome(this.Chromosome);
        //FromChromosome.Generate(chromosome);
        fromchromosome.Generate(chromosome, iterationLength);
    }

    public float GetDeltaPosition()
    {
        CurrentPosition = GlobalVariables.Position;
        return Vector3.Distance(CurrentPosition, LastPosition);
    }

    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    // For testing purposes only
    //public float Move(float x, float y, float z)
    //{

    //    CurrentPosition[0] = CurrentPosition[0] + MovementAbility[0];
    //    CurrentPosition[1] = CurrentPosition[1] + MovementAbility[1];
    //    CurrentPosition[2] = CurrentPosition[2] + MovementAbility[2];

    //    //Debug.Log("Current Position: (" + CurrentPosition[0].ToString("n3") + ", " + CurrentPosition[1].ToString("n3") + ", " + CurrentPosition[2].ToString("n3") + ")");
    //    double deltaX = x - CurrentPosition[0];
    //    double deltaY = y - CurrentPosition[1];
    //    double deltaZ = z - CurrentPosition[2];
    //    return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    //}
}
