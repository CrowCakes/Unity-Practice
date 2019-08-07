using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameManager manager;

    // Use this for initialization
    void Start () {
        manager = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "Rooms visited: " + manager.GetVisitedCount();
    }
}
