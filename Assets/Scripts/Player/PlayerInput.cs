using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    PlayerMovement movement;
    GameManager manager;
    Interaction interaction;

	// Use this for initialization
	void Start () {
        //find the specific script attached to player
        movement = this.GetComponent<PlayerMovement>();

        manager = FindObjectOfType<GameManager>();

        interaction = FindObjectOfType<Interaction>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        //float rotateX = Input.GetAxis("RotateX");
        //float rotateZ = Input.GetAxis("RotateY");

        movement.Move(moveX, moveZ);
        movement.Rotate();

        bool fire1 = Input.GetButtonDown("Fire1");
        if (fire1)
        {
            Pulse();
        }

        //bool interact = Input.GetButtonDown("Interact");
        bool interactHold = Input.GetButton("Interact");
        if (interactHold)
        {
            if (interaction.GetInteraction() == "Telescope")
            {
                movement.FreezePlayer();
                manager.TelescopeZoomOn();
            }
        }
        else
        {
            manager.TelescopeZoomOff();
            movement.UnfreezePlayer();
        }

        /*
        bool jump = Input.GetButtonDown("Jump");
        if (jump) {
            movement.Jump();
        }*/

        if (Input.GetKeyDown("r")) {
            manager.Restart();
        }

        if (Input.GetKeyDown("m")) {
            manager.ToggleMap();
        }

        if (Input.GetKeyDown("g"))
        {
            manager.MarkRoom();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            manager.TogglePause();
        }
    }

    private void Pulse() {
        Debug.Log("fire1");
    }
}
