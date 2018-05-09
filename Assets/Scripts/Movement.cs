using Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionEngine;

public class Movement : MonoBehaviour
{
    
    private HingeJoint2D hingeJoint;
    private JointMotor2D motor;
    private float counter = 0.0024f;
    private bool preUpdateComplete = false;
    private GameObject model;

    //public Organism model;
    public float motorBackSpeed;
    public float motorSpeed;

    private float initAngle;
    // Use this for initialization
    void Start()
    {
        //LoadOrganism();
        preUpdateComplete = false;
        motorSpeed = 200;
        motorBackSpeed = 200;
        hingeJoint = GetComponent<HingeJoint2D>();
        float angle = hingeJoint.jointAngle;
        //Debug.Log("initAngle: " + angle);
    }

    // Update is called once per frame
    void Update()
    {
        preUpdate();
        // up and down keys, range [-1, 1]
        float acceleration = Input.GetAxis("Vertical");
        Move(acceleration);
    }

    private void preUpdate()
    {
        if (!preUpdateComplete)
        {
            preUpdateComplete = true;
            var model = GameObject.FindGameObjectWithTag("Organism");
            ObjectSerializer.SaveOrganism(model);
        }
    }

    private void Move(float acceleration)
    {
        motor = hingeJoint.motor;
        // key up is powah * 1, key down is powah * -1, no key is powah * 0
        motor.motorSpeed = this.motorSpeed * acceleration;
        hingeJoint.motor = motor;

        GoBack(acceleration);

    }

    private void GoBack(float acceleration)
    {
        if(acceleration == 0)
        {
            motor.motorSpeed = -Mathf.Log(counter) * this.motorBackSpeed;
            hingeJoint.motor = motor;
            counter += 0.002f;
        } else
        {
            counter = 0.0024f;
        }
        //Debug.Log("Acceleration: " + acceleration);
        //Debug.Log("Counter: " + counter);
    }
}
