using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObject : MonoBehaviour {

    public string fileName;
    private SaveData data;

	// Use this for initialization
	void Start () {
        data = SaveData.Load(Application.streamingAssetsPath + "\\" + fileName + ".uml");
        var position = data.GetValue<Vector3>("position");
        var rotation = data.GetValue<Quaternion>("rotation");
        var scale = data.GetValue<Vector3>("scale");
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.rotation = rotation;
        cube.transform.localScale = scale;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
