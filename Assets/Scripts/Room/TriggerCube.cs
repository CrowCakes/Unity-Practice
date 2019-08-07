using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour {

    GameManager gameManager;
    int roomNumber;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        
        System.Int32.TryParse(this.gameObject.transform.parent.name.Replace("Room", ""), 
            out roomNumber);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            //Debug.Log("Triggered Room " + roomNumber + " " + gameObject.name);
            switch (gameObject.name)
            {
                case "North":
                    gameManager.TrippedRoomExitTrigger(roomNumber, 0);
                    break;

                case "West":
                    gameManager.TrippedRoomExitTrigger(roomNumber, 1);
                    break;

                case "East":
                    gameManager.TrippedRoomExitTrigger(roomNumber, 2);
                    break;

                case "South":
                    gameManager.TrippedRoomExitTrigger(roomNumber, 3);
                    break;
            }
        };
    }
}
