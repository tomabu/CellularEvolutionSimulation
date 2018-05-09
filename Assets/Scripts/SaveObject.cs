using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour {

    public string fileName;
    private SaveData data;
    private GameObject cube; 

	// Use this for initialization
	void Start () {
        data = new SaveData(fileName);
        cube = GameObject.FindGameObjectWithTag("Cube");

        var position = cube.transform.position;
        var rotation = cube.transform.rotation;
        var scale = cube.transform.localScale;

        data["position"] = position;
        data["rotation"] = rotation;
        data["scale"] = scale;

        data.Save();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
