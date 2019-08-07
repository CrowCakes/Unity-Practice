using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //private const float speed = 30f;
    public float smoothTime = 0.3f;
    public float maxCameraOffset = 50f;
    public float minCameraOffset = 18f;
    float cameraOffset;

    Vector3 zero = Vector3.zero;
    GameManager manager;

    private void Awake()
    {
        cameraOffset = minCameraOffset;
        manager = FindObjectOfType<GameManager>();

        //set camera view to top-down view
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        //set camera to center of room 1
        transform.position = new Vector3(0, cameraOffset, 0);
    }

    private void Update()
    {
        //get the center of the room the game view is centered on
        Vector3 foo = manager.GetCurrentRoomCenter();
        //this function works only when foo returns a position the camera is not on
        DragCamera(foo);

        //transform.position = new Vector3(foo.x, cameraOffset, foo.z);
    }

    public void DragCamera(Vector3 target) {
        //Debug.Log("DragCamera: " + new Vector3(target.x, cameraOffset, target.z));
        transform.position = Vector3.SmoothDamp(transform.position, 
            new Vector3(target.x, cameraOffset, target.z), 
            ref zero, 
            smoothTime);
    }

    public void TelescopeZoom() {
        cameraOffset = maxCameraOffset;
    }

    public void TelescopeZoomOff() {
        cameraOffset = minCameraOffset;
    }
}

/*
 public Transform target;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 goalPos = target.position;
        goalPos.y = transform.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
     */

/*
 public Transform player;
    public Vector3 offset;
    
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.position + offset;
        
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        
yaw += speedH* Input.GetAxis("RotateX");
pitch -= speedV* Input.GetAxis("RotateY");

transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        //float rotateX = Input.GetAxis("RotateX");
        //float rotateZ = Input.GetAxis("RotateY");
        
    }


     */
