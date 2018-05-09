using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {

    private GameObject model;
    private Vector3 center;

    public string modelToLoad;

	// Use this for initialization
	void Start () {
        LoadOrganism();
        center = model.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        int childCount = 0;
        foreach(var child in model.GetComponentsInChildren<Transform>())
        {
            center += child.position;
            childCount++;
        }
        center /= childCount;
        center.y = 0.008f;
        Debug.Log(center);
        

        transform.position = center;
        //Debug.Log(model.transform.position);
    }

    public void LoadOrganism()
    {
        if (!modelToLoad.Equals(string.Empty))
        {
            model = Instantiate(Resources.Load(modelToLoad)) as GameObject;
        }
    }

    public float[] GetPosition()
    {
        return new float[] { center.x, center.y, center.z };
    }
}
