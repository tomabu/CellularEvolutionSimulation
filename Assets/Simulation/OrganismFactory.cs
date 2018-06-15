using Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismFactory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static GameObject OrganismFromChromosome(byte[] chromosome)
    {
        // Preparation
        Feature f = new Feature(chromosome);
        GameObject g = new GameObject();
        return g;
    }
}
