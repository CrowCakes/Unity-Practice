using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoguelikeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text description;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoguelike()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.text = "Traverse an infinite catacomb and explore as many rooms as possible";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.text = "";
    }
}
