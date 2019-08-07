using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeTrigger : MonoBehaviour {

    Interaction manager;
    
	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<Interaction>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player entered the telescope");
            manager.SetInteraction("Telescope");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player left the telescope");
            manager.SetInteraction("");
        }
    }
}
