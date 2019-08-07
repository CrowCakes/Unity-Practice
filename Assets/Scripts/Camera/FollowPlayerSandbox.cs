using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerSandbox : MonoBehaviour
{
    public Transform player;
    public float height = 1f;
    public float telescopeOffset = 50f;
    public float offset = -3f;
    public float turnSpeed = 3f;
    public float smoothTime = 15f;
    public float turnThreshhold = 0.025f;
    public PlayerAvatarMovement movement;
    
    private Vector3 offsetVector;
    private string followMode = "";
    Vector3 zero = Vector3.zero;
    private Vector3 fooPosition;

    // Start is called before the first frame update
    void Start()
    {
        followMode = "";
        offsetVector = new Vector3(
            player.position.x, 
            player.position.y + height, 
            player.position.z + offset);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (followMode.Equals("telescope"))
        {
            movement.enabled = false;

            //set camera view to top-down view
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(new Vector3(90, 0, 0)),
                Time.deltaTime * smoothTime);
            
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.position.x,
                player.position.y + telescopeOffset,
                player.position.z),
                Time.deltaTime * smoothTime);
        }
        else {
            movement.enabled = true;

            fooPosition = new Vector3(player.position.x,
                player.position.y + height,
                player.position.z + offset);

            float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
            float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

            mouseX = (Mathf.Abs(mouseX) < turnThreshhold) ? 0f : mouseX;
            
            mouseY = (mouseY >= -0.3f && mouseY <= 0f) ? 0f : mouseY;
            mouseY = (mouseY > 0f) ? Mathf.Clamp(mouseY, 0f, 0.3f) : mouseY;
            mouseY = (mouseY < -0.3f) ? Mathf.Clamp(mouseY, -0.6f, -0.3f) : mouseY;
            //Debug.Log("mouseX = " + mouseX + ", mouseY = " + mouseY);
            //Debug.Log("mouseY = " + mouseY);
            /*
            transform.localRotation = Quaternion.Euler(
                new Vector4(
                    -1f * (mouseY * 180f),
                    mouseX * 360f,
                    transform.localRotation.z))
                    /*Quaternion.Euler(
                    new Vector4(
                        Mathf.Clamp(-1f * (mouseY * 180f),-15f,15f), 
                        Mathf.Clamp(mouseX * 360f, -30f, 30f), 
                        transform.localRotation.z))*/

            //if (Input.GetAxisRaw("Rotate Y") != 0) Debug.Log("Rotating");
            //offsetVector = Quaternion.AngleAxis(Input.GetAxisRaw("Rotate Y") * turnSpeed, Vector3.up) * offsetVector;
            offsetVector = Quaternion.AngleAxis(mouseX * turnSpeed, Vector3.up) * offsetVector;
            offsetVector = Quaternion.AngleAxis(mouseY/3 * turnSpeed, Vector3.left) * offsetVector;
            fooPosition = Vector3.Lerp(player.position, player.position + offsetVector, 2f);

            transform.position = Vector3.MoveTowards(transform.position, fooPosition, Time.deltaTime * smoothTime * 2);

            transform.LookAt(player.position + Vector3.up * 1.5f);
        }
        //if (transform.position.y < 0) transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void SetFollowMode(string text) {
        followMode = text;
    }
}
