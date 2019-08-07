using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceEnemy : Enemy {
    
    new const float on = 50f;
    private const float maxTime = 7.5f;
    private const float dmgMult = 0.6f;

    // Use this for initialization
    void Awake () {
        manager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        //if the player is in its room, do the furnace's attack
        //if room revealed through telescope, only brighten
        if (roomNumber == manager.GetPlayerCurrentRoom() || isZoom)
        {
            //begin to brighten the room
            Brighten();
            
            //attack if in the same room
            if (roomNumber == manager.GetPlayerCurrentRoom()) {
                player.GetComponent<Player>().SetIsDamaged(true);

                //its damage grows as the light grows brighter, up to thrice the normal hp drain (50*0.6 = 30 damage)
                player.GetComponent<Player>().DrainHP(
                    Mathf.RoundToInt(transform.GetChild(1).gameObject.GetComponent<Light>().intensity * dmgMult)
                    );
            }
        }
        else
        {
            player.GetComponent<Player>().SetIsDamaged(false);
            Dim();
        }
    }

    /// <summary>
    /// Unlike other enemies, the Furnace has to gradually brighten if the player is in its room.
    /// This function turns its light to max intensity instantly.
    /// </summary>
    void TurnLightOn() {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = on;
    }

    /// <summary>
    /// Unlike other enemies, the Furnace has to gradually dim if the player is not in its room.
    /// This function turns its light intensity to 0 instantly.
    /// </summary>
    void TurnLightOff()
    {
        transform.GetChild(1).gameObject.GetComponent<Light>().intensity = 0;
    }

    /// <summary>
    /// Unlike other enemies, the Furnace has to gradually brighten if the player is in its room.
    /// This function turns its light to max intensity over maxTime.
    /// </summary>
    void Brighten() {
        if (transform.GetChild(1).gameObject.GetComponent<Light>().intensity < on)
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity += (on / maxTime) * Time.deltaTime;
        else {
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity = on;
        }
    }

    /// <summary>
    /// Unlike other enemies, the Furnace has to gradually dim if the player is not in its room.
    /// This function turns its light intensity to 0 over half of the maxTime.
    /// </summary>
    void Dim()
    {
        if (transform.GetChild(1).gameObject.GetComponent<Light>().intensity > 0)
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity -= (on / (maxTime / 2)) * Time.deltaTime;
        else {
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity = 0;
        }
    }
}
