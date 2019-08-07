using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 360 * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player")) {
            Debug.Log("Goal!");
            anim.SetTrigger("reachedGoal");
            Destroy(gameObject);
        }
    }
}
