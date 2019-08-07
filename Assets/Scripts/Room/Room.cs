using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private const float smoothTime = 1f;
    private const float maxIntensity = 1f;

    //the physical center of the Room in Unity
    Vector3 center;

    //the n-th room created
    int number;

    //distance from Room 1
    int depth = -1;

    //the coordinates as recorded in the Graph 
    int[] coord = new int[2];

    //player-defined mark status for display on minimap
    bool isMarked = false;

    //self-explanatory
    GameManager manager;

    //the light source of this room
    Light spotlight;

    //the map icon of this room
    SpriteRenderer icon;
    //the icon for room marking
    public SpriteRenderer markIcon;

    //the original color of this map icon
    int iconColor = 0;

    //the barriers
    List<GameObject> barriers = new List<GameObject>();

    //check for whether telescope is zooming or not
    bool isZoom = false;

    private Color itsuki = new Color(1, 255 / 255f, 0);
    private Color nothing = new Color(0, 100/255f, 212/255f);
    private Color lamp = new Color(212/255f, 0, 0);
    private Color furnace = new Color(166 / 255f, 0, 1);
    private Color telescope = new Color(63 / 255f, 173 / 255f, 212 / 255f);

    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        spotlight = GetComponentInChildren<Light>();
        //turn all the lights off
        spotlight.intensity = 0;

        icon = GetComponentInChildren<SpriteRenderer>();
        //turn barriers off
        //ToggleBarriersOff();
    }

    void Update()
    {
        //if player is in this room OR telescope is zooming
        if (manager.GetPlayerCurrentRoom() == number || isZoom) {
            //gradually turn lights back on
            if (spotlight.intensity < maxIntensity) {
                TurnLightOn();
            }
        }
        //dim lights off if the player isn't in the room
        else {
            if (spotlight.intensity > 0)
            {
                TurnLightOff();
            }
        }
    }

    public int GetDepth()
    {
        return depth;
    }

    public void SetDepth(int depth)
    {
        this.depth = depth;
    }

    public int GetNum() {
        return number;
    }

    ///<summary>
    ///Changes the color of the Room's map icon according to the passed parameter.
    ///0 = current room,
    ///1 = other enemies,
    ///2 = furnace enemy,
    ///3 = telescope,
    ///4 = nothing/heal capsule,
    ///5 = undiscovered
    ///</summary>
    public void ChangeIconColor(int num) {
        //Debug.Log("SetIconColor Room number: " + number + ", num = " + num);
        switch (num) {
            //current room
            case 0:
                icon.color = itsuki;
                break;
            //other enemy types
            case 1:
                icon.color = lamp;
                break;
            //furnace
            case 2:
                icon.color = furnace;
                break;
            // telescope
            case 3:
                icon.color = telescope;
                break;
            case 4:
                icon.color = nothing;
                break;
            case 5:
                icon.color = Color.white;
                break;
        }
    }

    public void SetIconColor(int num) {
        iconColor = num;
    }

    public void RevertIconColor() {
        ChangeIconColor(iconColor);
    }

    public void MarkRoom() {
        markIcon.enabled = !markIcon.enabled;
    }

    //only called once, during initialization
    public void SetNum(int num) {
        number = num;
        gameObject.name = "Room" + number;
        //Debug.Log("Created a room with name: " + gameObject.name);
    }

    public Vector3 GetCenter()
    {
        return center;
    }

    public void SetCenter(Vector3 pos) {
        center = pos;
        transform.position = pos;
    }

    public int GetCoordX() {
        return coord[0];
    }

    public int GetCoordY()
    {
        return coord[1];
    }

    public void SetCoord(int x, int y)
    {
        coord[0] = x;
        coord[1] = y;
    }

    public bool GetIsZoom() {
        return isZoom;
    }

    public void SetIsZoom(bool b) {
        isZoom = b;
    }

    /// <summary>
    /// Sets the Player's current Room as this Room.
    /// </summary>
    public void PlayerArrived() {
        manager.ReceivePlayerCurrentRoom(number);
    }

    /// <summary>
    /// Gradually brightens the Room.
    /// </summary>
    public void TurnLightOn() {
        spotlight.intensity += Time.deltaTime * smoothTime;
    }

    /// <summary>
    /// Gradually dims the Room.
    /// </summary>
    public void TurnLightOff()
    {
        spotlight.intensity -= Time.deltaTime * smoothTime;
    }

    void SetBarriers() {
        barriers.Add(transform.GetChild(14).gameObject);
        barriers.Add(transform.GetChild(15).gameObject);
        barriers.Add(transform.GetChild(16).gameObject);
        barriers.Add(transform.GetChild(17).gameObject);
    }

    /// <summary>
    /// Toggles the Barriers in the Room on
    /// </summary>
    public void ToggleBarriersOn()
    {
        if (barriers.Count < 4) SetBarriers();
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(true);
        }
    }

    /// <summary>
    /// Toggles the Barriers in the Room off
    /// </summary>
    public void ToggleBarriersOff()
    {
        if (barriers.Count < 4) SetBarriers();
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(false);
        }
    }
}
