using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource soundManagerBGM;
    public Interaction objectInteract;
    GameObject player;

    float defaultPitch = 1f;
    float defaultVolume = 0.025f;

    // Start is called before the first frame update
    private void Awake()
    {
        soundManagerBGM = GetComponent<AudioSource>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            Debug.Log("player object is still null");
            player = GameObject.FindWithTag("Player");
        }
        if (objectInteract.GetInteractionOngoing().Equals("Pause")) {
            SetBGMSettings(defaultPitch, defaultVolume/4f);
        }
        else {
            if (objectInteract.GetInteractionOngoing().Equals("PlayerHurt"))
            {
                //Debug.Log("volume pitch down");
                SetBGMSettings(0.25f, 0.1f);
            }
            else if (!objectInteract.GetInteractionOngoing().Equals(""))
            {
                SetBGMSettings(0.5f, defaultVolume);
            }
            else
            {
                SetBGMSettings(defaultPitch, defaultVolume);
            }
        }
    }

    void SetBGMSettings(float p, float v) {
        soundManagerBGM.pitch = Mathf.Lerp(soundManagerBGM.pitch, p, 0.3f);
        soundManagerBGM.volume = Mathf.Lerp(soundManagerBGM.volume, v, 0.3f);
    }
}
