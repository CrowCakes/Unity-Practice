using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomIndicator : MonoBehaviour {

    GameManager manager;

	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<GameManager>();		
	}
	
	// Update is called once per frame
	void Update () {
        //display the room the Player is in
        //display the Player's distance away from Room 1
        gameObject.GetComponent<Text>().text = "Room " + manager.GetPlayerCurrentRoom() 
            + " | "  + "Depth " + manager.GetCurrentDepth();
    }
}
