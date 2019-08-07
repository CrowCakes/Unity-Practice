using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownCamera : MonoBehaviour {
    Camera viewCamera;

	// Use this for initialization
	void Start () {
        viewCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        cameraFollow();
	}

    public void cameraFollow()
    {
        float charPosX = transform.position.x;
        float charPosZ = transform.position.z;
        float cameraOffset = 9.0f;

        viewCamera.transform.position = new Vector3(charPosX, cameraOffset, charPosZ);
        viewCamera.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));

    }
}
