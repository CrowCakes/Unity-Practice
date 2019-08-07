using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpSpeed = 5f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    
    bool isGrounded = true;
    
    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 0.06f))
            {
                isGrounded = true;
                //transition from `!Grounded` to `Landing` animation state
                anim.SetTrigger("Landing");
                Debug.Log("Landing");
                anim.SetBool("IsGrounded", isGrounded);
            }
        }
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;
        playerRigidbody.MovePosition(transform.position + movement);
        transform.LookAt(transform.position + movement);
    }

    void Turning() {
        /*
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            Vector3 playerToMouse = floorHit.point - transform.position;
            //playerToMouse.x = Mathf.Clamp(playerToMouse.x, -15f, 15f);
            playerToMouse.y = Mathf.Clamp(playerToMouse.y, 0f, 5f);

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
            //Debug.Log("newRotation: " + newRotation);
        }
        
        else {
            float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
            float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
            playerRigidbody.MoveRotation(
                Quaternion.Euler(
                    new Vector4(
                        Mathf.Clamp(-1f * (mouseY * 180f), -30f, 30f),
                        Mathf.Clamp(mouseX * 360f, -90f, 90f),
                        playerRigidbody.transform.localRotation.z)
                    )
            );
        }
        */
    }

    void Jump() {
        Debug.Log("Jump");
        isGrounded = false;
        anim.SetTrigger("Jumping");
        anim.SetBool("IsGrounded", isGrounded);
        //anim.SetBool("IsJumping", isJumping);
        //anim.SetBool("IsGrounded", isGrounded);
        playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    void Animating(float h, float v) {
        bool walking = h != 0 | v != 0;
        anim.SetBool("IsWalking", walking);
    }
}
