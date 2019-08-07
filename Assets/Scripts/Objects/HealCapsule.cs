using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCapsule : MonoBehaviour
{
    public float healAmount = 10f;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player") {
            player.GetComponent<Player>().HealHP(healAmount);
            Destroy(gameObject, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.GetComponent<Player>().HealHP(healAmount);
            Destroy(gameObject, 0);
        }
    }
}
