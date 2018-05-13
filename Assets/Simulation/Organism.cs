using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism { // : MonoBehaviour

    public double[] CurrentPosition { get; }
    public byte[] Chromosome { get; }
    public int ID { get; }
    // public double InternalClock { get; }
    //public List<GameObject> Bones { get; }
    //public List<GameObject> Joints { get; }

    // For Testing Purposes Only
    public double[] MovementAbility { get; } // Movement simulation

    public Organism(int in_id, byte[] in_chromosome, System.Random random)
    {
        ID = in_id;
        Chromosome = in_chromosome;
        CurrentPosition = new double[3] { 0, 0, 0 };
        MovementAbility = new double[3] { random.NextDouble(), random.NextDouble(), random.NextDouble() }; // Generating random movement ability
        Debug.Log("Movement Ability: (" + MovementAbility[0].ToString("n3") + ", " + MovementAbility[1].ToString("n3") + ", " + MovementAbility[2].ToString("n3") + ")");
    }

 //   // Use this for initialization
 //   void Start () {
        
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    // For testing purposes only
    public float Move(float x, float y, float z)
    {

        CurrentPosition[0] = CurrentPosition[0] + MovementAbility[0];
        CurrentPosition[1] = CurrentPosition[1] + MovementAbility[1];
        CurrentPosition[2] = CurrentPosition[2] + MovementAbility[2];

        //Debug.Log("Current Position: (" + CurrentPosition[0].ToString("n3") + ", " + CurrentPosition[1].ToString("n3") + ", " + CurrentPosition[2].ToString("n3") + ")");
        double deltaX = x - CurrentPosition[0];
        double deltaY = y - CurrentPosition[1];
        double deltaZ = z - CurrentPosition[2];
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }
}
