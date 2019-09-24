using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LampEnemy : Enemy {
    private const float corner = 5.75f;
    private const float maxLength = 10;
    new const float on = 1f;
    private const float speed = 3.5f;
    private VisionPoint vision;

    //patrol
    List<Vector3> points = new List<Vector3>();
    private int destPoint = 0;
    private NavMeshAgent agent;

    //the center of the room the enemy is in
    Vector3 roomCenter;

    // Use this for initialization
    void Awake () {
        manager = FindObjectOfType<GameManager>();
        player = GameObject.FindWithTag("Player");
        vision = transform.GetChild(0).gameObject.GetComponent<VisionPoint>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.Log("failed to fetch the NavMeshAgent component");
        }
        //agent.autoBraking = false;
        agent.enabled = false;
    }

    private void Start()
    {
        SetCenter(roomCenter);
    }

    // Update is called once per frame
    void Update () {

        CheckVision();

        if (!isAlerted)
        {
            player.GetComponent<Player>().SetIsDamaged(false);
            transform.GetChild(1).gameObject.GetComponent<Light>().color = new Color(0, hue, 0);
            //GotoNextPoint();
            if (!agent.enabled) agent.enabled = true;
            if (!agent.pathPending && agent.remainingDistance < 1f && !agent.isStopped) AgentGoToNextPoint();
            
            if (agent.isStopped) {
               agent.isStopped = false;
            }
            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //    GotoNextPoint();
        }
        else {
            agent.isStopped = true;

            transform.GetChild(1).gameObject.GetComponent<Light>().color = new Color(hue, 0, 0);
            player.GetComponent<Player>().SetIsDamaged(true);
            player.GetComponent<Player>().DrainHP(damage);
        }

        if (roomNumber == manager.GetPlayerCurrentRoom() || isZoom)
        {
            if (roomNumber == manager.GetPlayerCurrentRoom()) { vision.enabled = true; }
            TurnLightOn();
        }
        else
        {
            vision.enabled = false;
            TurnLightOff();
        }

        //IsOutOfBounds();

        //if (!agent.isOnNavMesh) {
            //ReadjustNavPosition();
        //}
    }

    void CheckVision()
    {
        if (vision.IsInLineOfSight(player))
        {
            SetIsAlerted(true);
        }
        else { SetIsAlerted(false); }
    }

    /// <summary>
    /// Sets the position of the Enemy. Because DetectorEnemies will always be placed in the center of the Room,
    /// LampEnemies will be set to the upper right corner of the Room instead.
    /// </summary>
    /// <param name="pos">The center of the Room the Enemy is found in.</param>
    public override void SetCenter(Vector3 pos) {
        //agent = gameObject.GetComponent<NavMeshAgent>();

        transform.position = new Vector3(pos.x + corner, 0, pos.z + corner);
        //agent.Warp(new Vector3(pos.x + corner, 0, pos.z + corner));

        //agent.enabled = true;
        //ReadjustNavPosition();

        roomCenter = pos;

        SetWaypoints(pos);
    }

    void ReadjustNavPosition() {
        var position = transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, 10.0f, NavMesh.AllAreas);
        position = hit.position; // usually this barely changes, if at all
        //agent.Warp(position);
    }

    protected override void IsOutOfBounds()
    {
        //if pos is out of bounds, return it to the corner of the room
        if ((transform.position.x > roomCenter.x + corner || transform.position.x < roomCenter.x - corner) ||
            (transform.position.z > roomCenter.z + corner || transform.position.x < roomCenter.z - corner) ||
            (transform.position.y != 0))
        {
            //agent.Warp(new Vector3(roomCenter.x + corner, 0, roomCenter.z + corner));
            transform.position = new Vector3(roomCenter.x + corner, 0, roomCenter.z + corner);
        }
    }

    /// <summary>
    /// Adds the corner of the Room to the list of patrol waypoints.
    /// </summary>
    /// <param name="center">The center of the Room.</param>
    void SetWaypoints(Vector3 center) {
        points.Add(new Vector3(center.x - corner, transform.position.y, center.z + corner));
        points.Add(new Vector3(center.x - corner, transform.position.y, center.z - corner));
        points.Add(new Vector3(center.x + corner, transform.position.y, center.z - corner));
        points.Add(new Vector3(center.x + corner, transform.position.y, center.z + corner));
    }

    /// <summary>
    /// Turns light intensity to a predefined maximum level.
    /// </summary>
    void TurnLightOn()
    {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = on;
    }

    /// <summary>
    /// Turns light intensity to 0.
    /// </summary>
    void TurnLightOff()
    {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = 0;
    }

    /// <summary>
    /// Tells the NavMeshAgent to move to the next waypoint.
    /// </summary>
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        //agent.destination = points[destPoint];
        transform.position = Vector3.MoveTowards(transform.position, points[destPoint], speed * Time.deltaTime);
        transform.LookAt(points[destPoint]);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        if (Vector3.Distance(transform.position, points[destPoint]) < 0.01f) {
            destPoint = (destPoint + 1) % points.Count;
        }
    }

    void AgentGoToNextPoint() {
        //Debug.Log("Reached waypoint #" + currentWaypoint);
        agent.SetDestination(points[destPoint]);
        destPoint = (destPoint + 1) % 4;
    }
}

/*
class Seeker
{
    public NavMeshAgent agent;
    public float stoppingDist = 1f;

    Vector3[] waypoints = {new Vector3(5.5f, 0f, 5.5f),
            new Vector3(-5.5f, 0f, 5.5f),
            new Vector3(-5.5f, 0f, -5.5f),
            new Vector3(5.5f, 0f, -5.5f) };
    int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting patrol");
        agent.SetDestination(waypoints[currentWaypoint]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < stoppingDist)
        {
            Debug.Log("Reached waypoint #" + currentWaypoint);
            currentWaypoint = (currentWaypoint + 1) % 4;
            agent.SetDestination(waypoints[currentWaypoint]);
        }
    }
}*/