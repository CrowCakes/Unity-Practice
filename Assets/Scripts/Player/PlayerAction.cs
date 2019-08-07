using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Transform lockOnTarget;
    public int maxSpheres = 5;
    public float boltSpeed = 3f;
    public GameObject sphere;
    public GameObject bolt;
    public float timeBetweenFoxFire = 1.5f;
    public float timeBetweenFirebolt = 0.5f;

    List<GameObject> activeSpheres;
    float timerFoxFire = 0f;
    float timerFirebolt = 0f;
    GameObject currentBolt;

    // Start is called before the first frame update
    void Start()
    {
        sphere.GetComponent<FoxFire>().owner = transform.gameObject;
        activeSpheres = new List<GameObject>(maxSpheres);
    }

    // Update is called once per frame
    void Update()
    {
        timerFoxFire += Time.deltaTime;
        timerFirebolt += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timerFoxFire >= timeBetweenFoxFire) Sphere();
        else if (Input.GetButtonDown("Fire2") && timerFirebolt >= timeBetweenFirebolt) Firebolt();
        foreach (GameObject item in activeSpheres) {
            if (item == null) activeSpheres.Remove(item);
            break;
        }
    }

    void Sphere() {
        timerFoxFire = 0f;

        if (activeSpheres.Count < maxSpheres)
        {
            activeSpheres.Add(
                Instantiate(sphere,
                new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z),
                new Quaternion(0, 0, 0, 0)
                )
            );
            Debug.Log("Making sphere");
        }

        else Debug.Log("Max number of spheres reached!");
    }

    void Firebolt() {
        timerFirebolt = 0f;
        currentBolt = Instantiate(bolt, 
            transform.position + transform.forward * 0.75f + transform.up * 0.4f, 
            Quaternion.Euler(0,0,0));
        if (lockOnTarget != null)
        {
            currentBolt.transform.LookAt(lockOnTarget);
            currentBolt.GetComponent<Rigidbody>().AddForce(
                currentBolt.transform.forward * boltSpeed);
        }
        else
        {
            currentBolt.GetComponent<Rigidbody>().AddForce(
            transform.forward.normalized * boltSpeed);
        }
        Debug.Log("Firebolt launched");
    }
}
