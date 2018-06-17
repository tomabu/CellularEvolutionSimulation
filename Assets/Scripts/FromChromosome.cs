using Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromChromosome : MonoBehaviour {
    
    private Chromosome chrom;
	// Use this for initialization
	void Start () {
        var rand = new System.Random();
        rand.Next();
        var chromosome = new Chromosome(Simulation.Simulator.GenerateChromosome(2,10,10,10000,0.85f,1,4,rand));
        chrom = chromosome;
        Generate(chromosome);	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public static void Generate(Chromosome chromosome)
    {
        var features = chromosome.features;
        foreach (var feature in features)
        {
            // Adding two joints
            GameObject joint1 = Instantiate(Resources.Load("joint"), new Vector3(feature.firstPosX, feature.firstPosY, feature.firstPosZ), Quaternion.Euler(feature.firstRotX, feature.firstRotY, feature.firstRotZ)) as GameObject;
            GameObject joint2 = Instantiate(Resources.Load("joint"), new Vector3(feature.secondPosX, feature.secondPosY, feature.secondPosZ), Quaternion.Euler(feature.secondRotX, feature.secondRotY, feature.secondRotZ)) as GameObject;

            joint1.name = feature.firstID.ToString();
            joint2.name = feature.secondID.ToString();

            // DO NOT TOUCH - WORKS FINE!
            var bonePos = new Vector3((feature.firstPosX + feature.secondPosX) / 2, (feature.firstPosY + feature.secondPosY) / 2, (feature.firstPosZ + feature.secondPosZ) / 2);
            var rotation = Quaternion.FromToRotation(Vector3.up, joint1.transform.position - joint2.transform.position);
            GameObject bone = Instantiate(Resources.Load("bone"), bonePos, rotation) as GameObject;
            bone.transform.localScale = new Vector3(0.2999f, Vector3.Distance(joint1.transform.position, joint2.transform.position) / 2, 0.2999f);
            bone.name = feature.firstID.ToString() + "->" + feature.secondID.ToString();
            // U CAN TOUCH CODE NOW

            joint1.AddComponent<Rigidbody2D>();
            joint2.AddComponent<Rigidbody2D>();

            var boneRigidBody = bone.GetComponent<Rigidbody2D>();

            if(feature.firstType) // true - hingejoint, false - fixedjoint
            {
                var hinge1 = joint1.AddComponent<HingeJoint2D>();
                var limits = hinge1.limits;
                limits.max = feature.firstUppAng;
                limits.min = feature.firstLowAng;
                hinge1.limits = limits;
                hinge1.connectedBody = boneRigidBody;
            } else
            {
                var fixed1 = joint1.AddComponent<FixedJoint2D>();
                fixed1.connectedBody = boneRigidBody;
            }

            if (feature.secondType) // true - hingejoint, false - fixedjoint
            {
                var hinge2 = joint2.AddComponent<HingeJoint2D>();
                var limits2 = hinge2.limits;
                limits2.max = feature.secondUppAng;
                limits2.min = feature.secondLowAng;
                hinge2.limits = limits2;
                hinge2.connectedBody = boneRigidBody;
            }
            else
            {
                var fixed2 = joint2.AddComponent<FixedJoint2D>();
                fixed2.connectedBody = boneRigidBody;
            }

        }
        
    }
}
