using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToCanvas : MonoBehaviour {

    RectTransform canvas;
    RectTransform fog;

    // Use this for initialization
    void Start () {
        canvas = transform.parent.GetComponent<RectTransform>();
        fog = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        fog.sizeDelta = canvas.sizeDelta;
	}
}
