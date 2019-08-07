using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceNPC : Enemy
{
    new const float on = 20f;
    private const float maxTime = 3f;
    public UIManager ui;
    public InteractManager interact;

    //private const float dmgMult = 0.6f;

    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is in its room, do the furnace's attack
        //if room revealed through telescope, only brighten
        if (Vector3.Distance(
            player.transform.GetChild(0).transform.position, 
           transform.position) < 2f)
        {
            //begin to brighten
            Brighten();
            if (!interact.GetInteractType().Equals("telescope"))
            {
                ui.SetInteractText("Hold down [E] to interact");
                interact.SetInteractType("telescope");
            }
            else if (interact.GetCurrentInteractType().Equals("")) {
                ui.SetInteractText("Hold down [E] to interact");
            }
            else if (interact.GetCurrentInteractType().Equals("telescope")) {
                ui.SetInteractText("");
            }
        }
        else
        {
            Dim();
            ui.SetInteractText("");
            interact.SetInteractType("");
        }
    }

    /// <summary>
    /// Unlike other enemies, the Furnace has to gradually brighten if the player is in its room.
    /// This function turns its light to max intensity instantly.
    /// </summary>
    void TurnLightOn()
    {
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
    void Brighten()
    {
        if (transform.GetChild(1).gameObject.GetComponent<Light>().intensity < on)
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity += (on / maxTime) * Time.deltaTime;
        else
        {
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
        else
        {
            transform.GetChild(1).gameObject.GetComponent<Light>().intensity = 0;
        }
    }
}
