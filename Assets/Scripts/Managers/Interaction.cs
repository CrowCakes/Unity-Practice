using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//manages the ongoing interactions
public class Interaction : MonoBehaviour {

    //GameManager manager;

    //the hp bar of the player
    public Slider hpSlider;
    public Image hpBar;
    public Text hpText;

    //interaction prompt
    public Text interactText;

    //interaction that is currently available
    string interaction = "";

    //interaction currently taking place
    string interactionOngoing = "";

    private void Start()
    {
        //manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (interaction == "Telescope")
        {
            interactText.text = "Hold down [E] to zoom out";
        }
        else if (interaction == "")
        {
            interactText.text = "";
        }
    }

    public string GetInteraction() {
        return interaction;
    }

    public void SetInteraction(string obj) {
        interaction = obj;
    }

    public string GetInteractionOngoing()
    {
        return interactionOngoing;
    }

    public void SetInteractionOngoing(string obj)
    {
        interactionOngoing = obj;
    }

}
