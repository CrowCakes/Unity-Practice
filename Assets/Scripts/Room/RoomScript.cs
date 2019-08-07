using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player") {
            //Debug.Log("Player is in the room");
            transform.GetComponentInParent<Room>().ChangeIconColor(0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            //Debug.Log("Player has left the room");
            transform.GetComponentInParent<Room>().RevertIconColor();
        }
    }
}
