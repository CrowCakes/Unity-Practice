using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /*public*/ float speed;

    bool scaleToTime = true;
    Rigidbody rb;
    float force = 100f;
    //private bool isJumping = false;
    float maxLength = 7.5f;
    Vector3 movement;

    Camera mainCamera;
    private float camRayLength = 100f;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = this.gameObject.GetComponent<Rigidbody>();
        speed = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (scaleToTime) { speed = Time.deltaTime; }
    }

    public void Move(float moveX, float moveZ)
    {
        /*
        movement.Set(moveX, 0f, moveZ);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
        */

        movement = new Vector3(force * moveX * speed, 0, force * moveZ * speed);
        
        rb.AddForce(movement, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLength);
        
    }

    public void Rotate() {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camRayLength))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            transform.GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

    /*
    public void Jump() {
        if (!isJumping) {
            Debug.Log("Jump");
            //rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z) * speed;
            rb.AddForce(0, force*speed, 0, ForceMode.VelocityChange);
            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLength);
            
            //Vector3 movingVector = new Vector3(rb.velocity.x, force*10, rb.velocity.z);
            //movingVector *= speed;
            //rb.velocity = movingVector;
        }
    }
    */

    public void FreezePlayer() {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezePlayer() {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "Floor")
        {
            /*
            Debug.Log("Player is in contact with "
                + collision.collider.name
                + " of "
                + collision.collider.transform.parent.name);*/
            //isJumping = false;
        }
        else {
            /*
            Debug.Log("Player is in contact with "
                + collision.collider.name
                );*/
        }
        
    }

    void OnCollisionExit(Collision jump)
    {
        /*
        if (jump.collider.tag == "Floor")
        {
            Debug.Log("Player is in the air");
            //isJumping = true;
        }
        */
    }
}

/*
    public Rigidbody rb;
    public float force = 500f;
    private bool moveRight = false;
    private bool moveLeft = false;
    private bool moveForward = false;
    private bool moveBack = false;
    private bool moveJump = false;
	private bool moveStop = false;
    float distance_to_ground;

    // Use this for initialization
    void Start () {
        distance_to_ground = GetComponent<Collider>().bounds.extents.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("d"))
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }

        if (Input.GetKey("a"))
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }

        if (Input.GetKey("s"))
        {
            moveBack = true;
        }
        else
        {
            moveBack = false;
        }

        if (Input.GetKey("w"))
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }

        if (Input.GetKey("w"))
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            moveJump = true;
        }
        else {
            moveJump = false;
        }
		
		if (Input.GetKey("c"))
        {
            moveStop = true;
        }
		else {
			moveStop = false;
		}
    }

    // Update is called once per frame
    // physics
    void FixedUpdate()
    {
        if (moveRight)
        {
            //rb.AddForce(force * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            rb.velocity = new Vector3(10,rb.velocity.y,rb.velocity.z);
        }
        if (moveLeft)
        {
            //rb.AddForce(-force * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            rb.velocity = new Vector3(-10, rb.velocity.y, rb.velocity.z);
        }
        if (moveForward)
        {
            //rb.AddForce(0, 0, force * Time.deltaTime, ForceMode.VelocityChange);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 10);
        }
        if (moveBack) {
            //rb.AddForce(0, 0, -force * Time.deltaTime, ForceMode.VelocityChange);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -10);
        }
        if (isJumping == false & moveJump) {
            //rb.AddForce(0, force * Time.deltaTime, 0);
			rb.velocity = new Vector3(rb.velocity.x, 10, rb.velocity.z);
            isJumping = true;
        }
		if (moveStop & !isJumping) {
			rb.velocity = new Vector3(0,0,0);
		}
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distance_to_ground + 0.1f);
    }
	
	
    */
