using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telescope : MonoBehaviour {
    Vector3 center;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetCenter()
    {
        return center;
    }

    public void SetCenter(Vector3 pos)
    {
        center = pos;
        transform.position = pos;
    }
}
