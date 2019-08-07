using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBorder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.position = new Vector3(0f, 0f, 0f);
            Debug.Log("Player out of bounds");
        };
    }
}
