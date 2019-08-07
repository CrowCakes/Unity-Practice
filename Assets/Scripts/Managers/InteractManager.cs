using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private string interactionType;
    private string currentInteraction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInteractType(string text)
    {
        interactionType = text;
    }

    public string GetInteractType()
    {
        return interactionType;
    }

    public void SetCurrentInteractType(string text)
    {
        currentInteraction = text;
    }

    public string GetCurrentInteractType()
    {
        return currentInteraction;
    }
}
