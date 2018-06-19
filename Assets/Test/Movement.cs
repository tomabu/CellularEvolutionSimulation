using Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionEngine;
/// <summary>
/// Class proviging movement functions and methods.
/// </summary>
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
        movementInterval = 40;
        forward = true;
        hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        float angle = hingeJoint.jointAngle;
        motor = hingeJoint.motor;
        if (hingeJoint != null)
        {
            StartCoroutine(MovementFunction());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Method moving muscle.
    /// </summary>
    private void Move()
    {
        var acceleration = 0.2f;
        motor = hingeJoint.motor;
        // key up is powah * 1, key down is powah * -1, no key is powah * 0
        motor.motorSpeed = this.motorSpeed * acceleration;
        hingeJoint.motor = motor;
        GoBack();
    }

    /// <summary>
    /// Method getting muscle back to original state.
    /// </summary>
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

    /// <summary>
    /// Method setting motor direction and speed of a hinge joint (muscle).
    /// </summary>
    /// <returns></returns>
    IEnumerator MovementFunction()
    {
        float a;
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
