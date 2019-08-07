using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    float cameraOffset;
    Vector3 zero = Vector3.zero;
    float smoothTime = 0f;

    GameManager manager;
    
    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        cameraOffset = 225f;
    }

    // Update is called once per frame
    void Update()
    {
        //get the center of the room the game view is centered on
        Vector3 foo = manager.GetCurrentRoomCenter();
        //this function works only when foo returns a position the camera is not on
        DragCamera(foo);
    }

    void DragCamera(Vector3 target)
    {
        //Debug.Log("DragCamera: " + new Vector3(target.x, cameraOffset, target.z));
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(target.x, cameraOffset, target.z),
            ref zero,
            smoothTime);
    }
}
