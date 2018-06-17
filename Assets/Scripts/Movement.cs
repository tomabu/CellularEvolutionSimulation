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
    private bool forward;
    private GameObject model;
    private float prevTime;

    //public Organism model;
    public float motorBackSpeed;
    public float motorSpeed;
    public int movementInterval;

    private float initAngle;
    // Use this for initialization
    void Start()
    {
        //LoadOrganism();
        preUpdateComplete = false;
        //motorSpeed = 200;
        //motorBackSpeed = 200;
        movementInterval = 40;
        forward = true;
        //hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        float angle = hingeJoint.jointAngle;
        motor = hingeJoint.motor;
        //Debug.Log("initAngle: " + angle);
        if (hingeJoint != null)
        {
            StartCoroutine(MovementFunction());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //preUpdate();
        // up and down keys, range [-1, 1]
        //float acceleration = Input.GetAxis("Vertical");
    }

    //private void preUpdate()
    //{
    //    if (!preUpdateComplete)
    //    {
    //        preUpdateComplete = true;
    //        //var model = GameObject.FindGameObjectWithTag("Organism");
    //        //ObjectSerializer.SaveOrganism(model);
    //    }
    //}

    private void Move()
    {
        var acceleration = 0.2f;
        motor = hingeJoint.motor;
        // key up is powah * 1, key down is powah * -1, no key is powah * 0
        motor.motorSpeed = this.motorSpeed * acceleration;
        hingeJoint.motor = motor;
        GoBack();
    }

    private void GoBack()
    {
        var acceleration = 0.0f;
        if (acceleration == 0)
        {
            motor.motorSpeed = -Mathf.Log(counter) * this.motorSpeed * 4;
            hingeJoint.motor = motor;
            counter += 0.002f;
        }
        else
        {
            counter = 0.0024f;
        }
        //Debug.Log("Acceleration: " + acceleration);
        //Debug.Log("Counter: " + counter);
    }

    IEnumerator MovementFunction()
    {
        float a;
        //int frames = 20;
        //var acceleration = 200.0f;
        for (int i = 0; i <= movementInterval; i++)
        {
            if (forward)
            {
                a = 1.0f;
                motor.motorSpeed = a * motorSpeed;
            }
            else
            {
                a = -1.0f;
                motor.motorSpeed =  a * motorSpeed;
            };
            
            hingeJoint.motor = motor;
            if (i >= movementInterval)
            {
                i = 0;
                forward = !forward;
            }
            //Debug.Log("i: " + i + "; Forward: " + forward + "; motorSpeed: " + motor.motorSpeed);
            yield return null;
        }
    }
}
