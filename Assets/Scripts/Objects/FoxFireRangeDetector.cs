using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFireRangeDetector : MonoBehaviour
{
    FoxFire parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<FoxFire>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag != "RangeIndicator") {
            //Debug.Log(other.transform.name + " entered the range");
            parent.SetTarget(other.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.transform.tag != "RangeIndicator")
        {
            //Debug.Log(other.transform.name + " left the range");
            parent.RemoveTarget(other.gameObject);
        }
        
    }
}
