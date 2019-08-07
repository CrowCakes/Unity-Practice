using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : MonoBehaviour
{
    public float timeToDetonate = 1f;
    float timeElapsed = 0f;
    bool isExploding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeToDetonate && !isExploding) {
            Debug.Log(transform.name + " exploded harmlessly");
            Explode();
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(transform.name + " collided with " + collision.transform.name);
        Explode();
    }

    void Explode() {
        isExploding = true;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        //Destroy(gameObject,1f);
    }
}
