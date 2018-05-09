using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terain : MonoBehaviour {


    public float heightScale = 5.0f;
    public float detailScale = 5.0f;

    private Mesh myMesh;
    private Vector3[] vertices;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Generate()
    {
        myMesh = this.GetComponent<MeshFilter>().mesh;
        vertices = myMesh.vertices;

        int counter = 0;
        int yLevel = 0;

        for(int i = 0; i<11; i++)
        {
        
        }
    }
}
