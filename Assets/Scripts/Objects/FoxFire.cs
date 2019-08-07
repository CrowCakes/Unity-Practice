using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire : MonoBehaviour
{
    public GameObject owner;
    public float timeToDetonate = 3f;
    public float timeToChainReaction = 0.5f;
    public float timeToExplode = 1f;
    //public GameObject rangeIndicator;
    public float range = 4f;
    public float explosionForce = 300f;
    public float baseIntensity = 1f;
    public float detonationIntensity = 5f;

    float timeElapsed = 0f;
    float timeStep;
    float initialSize = 0f;
    float finalSize;
    float stepSize;
    float stepSizeLight;
    bool isExploding = false;

    Transform child;
    Vector3 newSize;
    Transform activeIndicator;
    ParticleSystem explosion;
    Light lightComponent;
    List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        transform.name = owner.transform.name + "'s FoxFire";
        Debug.Log(transform.name + " spawned");

        child = transform.GetChild(0);
        activeIndicator = transform.GetChild(1);
        explosion = GetComponent<ParticleSystem>();
        lightComponent = GetComponent<Light>();
        lightComponent.intensity = baseIntensity;

        finalSize = (transform.localScale - child.localScale).x;
        timeStep = Time.deltaTime;
        stepSize = Mathf.Lerp(initialSize, finalSize, timeStep / timeToDetonate);
        stepSizeLight = Mathf.Lerp(0, detonationIntensity - baseIntensity, timeStep / timeToDetonate);
        /*
        activeIndicator = Instantiate(rangeIndicator, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            new Quaternion(0, 0, 0, 0)
            );
        */
        activeIndicator.transform.position = 
            new Vector3(transform.position.x, transform.position.y, transform.position.z);
        activeIndicator.transform.localScale += new Vector3(range - 1, range - 1, range - 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeElapsed >= timeToDetonate && !isExploding)
        {
            transform.GetChild(1).GetComponent<FoxFireRangeDetector>().enabled = false;
            Detonate();
        }
        else if (!isExploding) {
            timeElapsed += timeStep;

            child.localScale += new Vector3(stepSize, stepSize, stepSize);
            lightComponent.intensity += stepSizeLight;
        }
    }

    public void SetTarget(GameObject gameObject) {
        if (gameObject.tag == "FoxFire")
        {
            //transform.GetChild(1).GetComponent<FoxFireRangeDetector>().enabled = false;
            SpecialDetonate();
        }
        else if (gameObject != null) targets.Add(gameObject);
    }

    public void RemoveTarget(GameObject gameObject) {
        if (gameObject != null) targets.Remove(gameObject);
    }

    void Detonate() {
        isExploding = true;
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        AcquireTargets();
        Explosion();

        //Destroy(activeIndicator, 0f);
        Destroy(gameObject, timeToExplode);
    }

    void SpecialDetonate()
    {
        isExploding = true;
        Debug.Log(transform.name + " encountered another FoxFire and is primed!");
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        lightComponent.intensity = detonationIntensity;

        Invoke("AcquireTargets", timeToChainReaction);
        Invoke("Explosion", timeToChainReaction);

        Destroy(gameObject, timeToChainReaction + timeToExplode);
    }

    void AcquireTargets()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;

        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, range / 2f));
        colliders.RemoveAll(collider => collider.tag == "RangeIndicator");
        colliders.RemoveAll(collider => collider.tag == "FoxFire");

        if (colliders.Count == 0) Debug.Log(transform.name + " detonated harmlessly");
        else {
            foreach (Collider hit in colliders)
            {
                if (hit.transform.name != "RangeIndicator" ||
                    hit.gameObject != gameObject || hit.transform.name != owner.transform.name)
                {
                    Debug.Log(hit.transform.name +
                    " was in range of the explosion of " +
                    transform.name);
                    if (hit.GetComponent<Rigidbody>() != null)
                    {
                        //Debug.Log("KABOOM! " + hit.transform.name);
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(
                            explosionForce, transform.position, range / 2f);
                    }
                }
            }
        }
    }

    void Explosion() {
        Hide();
        explosion.Play();
    }

    void Hide() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }
}
