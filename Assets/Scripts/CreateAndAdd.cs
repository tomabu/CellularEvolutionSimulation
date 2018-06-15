using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAndAdd : MonoBehaviour {
    // Use this for initialization
    GameObject Create()
    {
        var go = new GameObject();
        var filter = go.AddComponent<MeshFilter>() as MeshFilter;
        var meshes = new Mesh[2];
        filter.mesh = meshes[0];
        var renderer = go.AddComponent<MeshRenderer>() as MeshRenderer;
        renderer.material.color = Color.black;
        return go/*GameObject.CreatePrimitive(PrimitiveType.Sphere);*/;
    }
    void Start()
    {
        //SphereCollider sc = gameObject.AddComponent<SphereCollider>() as SphereCollider;
        // var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.AddComponent<HingeJoint2D>();
        GameObject go = Create();//Instantiate(Resources.Load("Model001")) as GameObject;
        go.AddComponent<HingeJoint2D>();
        //var hinge = go.GetComponent<HingeJoint2D>();
    }
        //// Update is called once per frame
    void Update () {
        //HingeJoint2D hinge = sphere.GetComponent<HingeJoint2D>();
        //// sphere.GetComponent<HingeJoint2D>();
        //JointMotor2D motor = hinge.motor;
        //motor.motorSpeed = 100f;
        //hinge.motor = motor;
    }
}
