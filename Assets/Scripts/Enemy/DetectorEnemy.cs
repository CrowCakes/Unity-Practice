using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectorEnemy : Enemy {
    private float degs = 360;
    private VisionCone vision;

    //NavMeshObstacle obstacle;

	// Use this for initialization
	void Awake () {
        manager = FindObjectOfType<GameManager>();
        player = GameObject.FindWithTag("Player");
        vision = transform.GetChild(0).gameObject.GetComponent<VisionCone>();
        degs /= Random.Range(3, 5);
        //obstacle = transform.GetChild(0).GetComponent<NavMeshObstacle>();
        //obstacle.enabled = false;
        //KillLights();
	}
	
	// Update is called once per frame
	void Update () {
        CheckVision();

        if (!isAlerted)
        {
            player.GetComponent<Player>().SetIsDamaged(false);
            transform.GetChild(1).gameObject.GetComponent<Light>().color = new Color(0, hue, 0);
            transform.Rotate(0, degs * Time.deltaTime, 0);
        }
        else {
            transform.GetChild(1).gameObject.GetComponent<Light>().color = new Color(hue, 0, 0);
            player.GetComponent<Player>().SetIsDamaged(true);
            player.GetComponent<Player>().DrainHP(damage);
        }

        if (roomNumber == manager.GetPlayerCurrentRoom() || isZoom)
        {
            if (roomNumber == manager.GetPlayerCurrentRoom()) { vision.enabled = true; }
            TurnLightOn();
        }
        else {
            vision.enabled = false;
            TurnLightOff();
        }
	}

    void CheckVision() {
        if (vision.IsInLineOfSight(player)) {
            SetIsAlerted(true);
        }
        else { SetIsAlerted(false); }
    }

    /// <summary>
    /// Sets the position of the Enemy.
    /// </summary>
    /// <param name="pos">The center of the Room</param>
    public override void SetCenter(Vector3 pos)
    {
        base.SetCenter(pos);

        //obstacle.enabled = true;
    }

    /// <summary>
    /// Turns light intensity to a predefined maximum level.
    /// </summary>
    void TurnLightOn() {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = on;
    }

    /// <summary>
    /// Turns light intensity to 0.
    /// </summary>
    void TurnLightOff() {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = 0;
    }

    /// <summary>
    /// Returns the current intensity of this Enemy's light.
    /// </summary>
    /// <returns></returns>
    float LightIntensity() {
        return transform.GetChild(1).gameObject.GetComponent<Light>().intensity;
    }
}
