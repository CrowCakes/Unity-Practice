using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SandboxButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void LoadSandbox() {
        SceneManager.LoadScene("sandbox");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.text = "Collect the coin in the middle of the tiny castle";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.text = "";
    }
}
