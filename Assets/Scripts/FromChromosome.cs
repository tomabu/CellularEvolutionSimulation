using ConsoleApp1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromChromosome : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var rand = new System.Random();
        rand.Next();
        var chromosome = Simulation.Simulator.GenerateChromosome(rand);
        Generate(chromosome);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Generate(byte[] chromosome)
    {
        var features = new Feature(chromosome);

        // Adding two joints
        GameObject joint1 = Instantiate(Resources.Load("joint"), new Vector3(features.firstPosX, features.firstPosY, features.firstPosZ), Quaternion.Euler(features.firstRotX, features.firstRotY, features.firstRotZ)) as GameObject;
        GameObject joint2 = Instantiate(Resources.Load("joint"), new Vector3(features.secondPosX, features.secondPosY, features.secondPosZ), Quaternion.Euler(features.secondRotX, features.secondRotY, features.secondRotZ)) as GameObject;

        joint1.name = features.firstID.ToString();
        joint2.name = features.secondID.ToString();

        var hinge1 = joint1.GetComponent<HingeJoint2D>();
        var limits = hinge1.limits;
        limits.max = features.firstUppAng;
        limits.min = features.firstLowAng;
        hinge1.limits = limits;

        var hinge2 = joint2.GetComponent<HingeJoint2D>();
        var limits2 = hinge2.limits;
        limits2.max = features.secondUppAng;
        limits2.min = features.secondLowAng;
        hinge2.limits = limits2;

        var bonePos = new Vector3((features.firstPosX + features.secondPosX) / 2, (features.firstPosY + features.secondPosY) / 2, (features.firstPosZ + features.secondPosZ) / 2);
        var boneRot = Vector3.Angle(joint1.transform.position, joint2.transform.position);
        //var z = (float)Math.Atan2(features.firstPosX - features.secondPosX, features.firstPosY - features.secondPosY) * (float)(180 / Math.PI);
        var rotation = Quaternion.FromToRotation(Vector3.up, joint1.transform.position - joint2.transform.position);
        GameObject bone = Instantiate(Resources.Load("bone"), bonePos, rotation) as GameObject;
        bone.transform.localScale = new Vector3(0.2999f, Vector3.Distance(joint1.transform.position, joint2.transform.position) / 2, 0.2999f);
    }
}
