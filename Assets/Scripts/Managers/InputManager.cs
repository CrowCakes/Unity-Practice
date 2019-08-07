using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player movement is handled in PlayerAvatarMovement, not here
public class InputManager : MonoBehaviour
{
    public InteractManager interact;
    public FollowPlayerSandbox playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Interact"))
        {
            if (interact.GetInteractType().Equals("telescope"))
            {
                interact.SetCurrentInteractType("telescope");
                playerCamera.SetFollowMode("telescope");
            }
        }
        else {
            interact.SetCurrentInteractType("");
            playerCamera.SetFollowMode("");
        }
    }

}
