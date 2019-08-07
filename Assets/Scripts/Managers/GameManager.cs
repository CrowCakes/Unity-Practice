using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private const int roomDistanceOffset = 20;
    private const int goToNextRoom = 14;

    public GameObject playerPrefab;
    public GameObject roomPrefab;

    public Camera minimap;

    //reference to the player object
    GameObject player;

    //the main camera
    Camera mainCamera;

    //HP bar color
    public Image hpBarColor;

    //taking damage screen
    public Image damageFlash;

    //pause button
    public Image pauseButton;
    public Text pauseText;

    //make other objects in the game
    ObjectMaker objectMaker;

    //store all the rooms in a graph
    Graph<GameObject> rooms = new Graph<GameObject>(null);
    //all visited rooms
    HashSet<int> visited = new HashSet<int>();

    //number of the next room to be generated
    int roomNumber = 1;

    //the player's current location
    int playerCurrentRoom = 1;
    //the physical center of the room that player is in
    Vector3 currentRoomCenter = Vector3.zero;
    //the depth of the current room
    int currentDepth = 0;

	// Use this for initialization
	void Awake () {
        //initialize objectmaker
        objectMaker = FindObjectOfType<ObjectMaker>();

        //mum get the camera
        mainCamera = Camera.main;

        //make the first room
        GenerateRoom(Vector3.zero, 0, 0, 0);

        //for testing only
        //objectMaker.MakeDetectorEnemy(new Vector3(20,0,0), 4);

        //set the current room to 1
        ReceivePlayerCurrentRoom(roomNumber - 1);

        //make the player
        //put him in the first room
        player = (GameObject)Instantiate(playerPrefab, new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
        player.name = "Player";
        visited.Add(playerCurrentRoom);

        //i do not want to call this every frame, only every time a new room is entered
        PopulateGraph();
    }
	
	// Update is called once per frame
	void Update () {
        //CheckLinesOfSight();

        //if player is dead restart the game

	}

    /// <summary>
    /// Get the HPSlider's fill image.
    /// </summary>
    /// <returns>The Image corresponding to the HPSlider's Fill</returns>
    public Image GetHPBarFill() {
        return hpBarColor;
    }

    /// <summary>
    /// Get the Damage Flash effect Image.
    /// </summary>
    /// <returns>The Image corresponding to the Damage Flash</returns>
    public Image GetDamageFlash() {
        return damageFlash;
    }

    /// <summary>
    /// Get the MainCamera.
    /// </summary>
    /// <returns>The MainCamera.</returns>
    public Camera GetMainCamera() {
        return mainCamera;
    }

    /*
    /// <summary>
    /// Checks if player is in the line of sight of any DetectorEnemies.or LampEnemies
    /// </summary>
    void CheckLinesOfSight() {
        List<GameObject> enemies = objectMaker.GetEnemies();
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<DetectorEnemy>() != null)
            {
                if (enemy.transform.GetChild(0).gameObject.GetComponent<VisionCone>().IsInLineOfSight(player))
                {
                    enemy.GetComponent<DetectorEnemy>().SetIsAlerted(true);
                    //player.GetComponent<Player>().SetIsDamaged(true);
                }
                else
                {
                    enemy.GetComponent<DetectorEnemy>().SetIsAlerted(false);
                    //player.GetComponent<Player>().SetIsDamaged(false);
                }
            }
            else if (enemy.GetComponent<LampEnemy>() != null) {
                if (enemy.transform.GetChild(0).gameObject.GetComponent<VisionPoint>().IsInLineOfSight(player))
                {
                    enemy.GetComponent<LampEnemy>().SetIsAlerted(true);
                   //player.GetComponent<Player>().SetIsDamaged(true);
                }
                else
                {
                    enemy.GetComponent<LampEnemy>().SetIsAlerted(false);
                    //player.GetComponent<Player>().SetIsDamaged(false);
                }
            }
        }
    }*/

    /// <summary>
    /// Sets the Player's current Room location.
    /// </summary>
    /// <param name="x"></param>
    public void ReceivePlayerCurrentRoom(int x) {
        //does nothing if already visited room X
        visited.Add(x);

        //only change rooms if the parameter passed is different from the current room
        if (playerCurrentRoom != x)
        {
            playerCurrentRoom = x;
            //track the center of the room
            currentRoomCenter = FindRoomByNum(x).GetData().GetComponent<Room>().GetCenter();
            //track its depth
            currentDepth = FindRoomByNum(x).GetData().GetComponent<Room>().GetDepth();
            //update graph
            PopulateGraph();
            //Debug.Log("Player is now in room # " + playerCurrentRoom);
        }
        //nothing happens if the current room doesn't change
    }

    /// <summary>
    /// Returns the Player's current Room location.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerCurrentRoom() {
        return playerCurrentRoom;
    }

    /// <summary>
    /// Returns the physical coordinates of the current Room's center. 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentRoomCenter() {
        return currentRoomCenter;
    }

    public int GetCurrentDepth() {
        return currentDepth;
    }

    /// <summary>
    /// Returns the number of elements in the set of visited Rooms.
    /// </summary>
    /// <returns></returns>
    public int GetVisitedCount() {
        return visited.Count;
    }

    /// <summary>
    /// Sets Player location to the next Room adjacent to the exit DIR of room NUM.
    /// </summary>
    /// <param name="num">The Room number of the source room</param>
    /// <param name="dir">The cardinal direction that the Player left through. 0 = north, 1 = west, 2 = east, 3 = south</param>
    public void TrippedRoomExitTrigger(int num, int dir) {
        //checkIcon.SetActive(true);
        //find the neighbor connected to this exit
        //Debug.Log("TrippedRoomExitTrigger: Room " + num + "." + dir);
        switch (dir) {
            case 0:
                //throw player into the next room
                player.transform.position = new Vector3(
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().x, 
                    1,
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().z + goToNextRoom);
                //update current room
                ReceivePlayerCurrentRoom(FindRoomByNum(num).GetNeighbors()[dir].GetData().GetComponent<Room>().GetNum());
                break;

            case 1:
                player.transform.position = new Vector3(
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().x - goToNextRoom,
                    1,
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().z);
                ReceivePlayerCurrentRoom(FindRoomByNum(num).GetNeighbors()[dir].GetData().GetComponent<Room>().GetNum());
                break;

            case 2:
                player.transform.position = new Vector3(
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().x + goToNextRoom,
                    1,
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().z);
                ReceivePlayerCurrentRoom(FindRoomByNum(num).GetNeighbors()[dir].GetData().GetComponent<Room>().GetNum());
                break;

            case 3:
                player.transform.position = new Vector3(
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().x,
                    1,
                    FindRoomByNum(playerCurrentRoom).GetData().GetComponent<Room>().GetCenter().z - goToNextRoom);
                ReceivePlayerCurrentRoom(FindRoomByNum(num).GetNeighbors()[dir].GetData().GetComponent<Room>().GetNum());
                break;
        }

        //player.transform.position = new Vector3(0, 1, 0);
    }

    /// <summary>
    /// Enables the mark icon of the Room the player is in
    /// </summary>
    public void MarkRoom() {
        FindRoomByNum(GetPlayerCurrentRoom()).GetData().GetComponent<Room>().MarkRoom();
    }

    /// <summary>
    /// Reloads the game.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Gets the Graph Vertex that contains the Room with the corresponding number.
    /// </summary>
    /// <param name="num">The target Room's number</param>
    /// <returns></returns>
    Vertex<GameObject> FindRoomByNum(int num) {
        foreach (Vertex<GameObject> item in rooms.GetVertices())
        {
            //found it
            if (item.GetData().GetComponent<Room>().GetNum() == num)
            {
                return item;
            }
        }
        
        //couldn't find anything in the loop
        return null;
    }

    /// <summary>
    /// Generate a Room, initialize it, then add it to the Graph. It will also randomly generate an object inside it
    /// </summary>
    /// <param name="pos">The position in the world where the Room should be placed</param>
    /// <param name="x">The Room's virtual X-position in the Graph</param>
    /// <param name="y">The Room's virtual Y-position in the Graph</param>
    void GenerateRoom(Vector3 pos, int x, int y, int depth) {
        //instantiate the Room
        GameObject foo = (GameObject)Instantiate(roomPrefab);
        //initialize the Room, and set the number of the next room
        foo.GetComponent<Room>().SetNum(roomNumber++);
        foo.GetComponent<Room>().SetCenter(pos);
        foo.GetComponent<Room>().SetCoord(x, y);
        foo.GetComponent<Room>().SetDepth(depth);
        //add Room to Graph
        rooms.AddVertex(foo);
        //debug
        //Debug.Log("Room list size is " + rooms.GetGraphSize());

        //randomly generate objects within the room
        int bar = objectMaker.PopulateRoom(pos, roomNumber - 1);
        //Debug.Log(bar);
        if (bar > 1) {
            //turn off the barrier if it did not make an enemy
            foo.GetComponent<Room>().ToggleBarriersOff();
        }
        //set the map icon for minimap
        foo.GetComponent<Room>().SetIconColor(bar + 1);
        //set the color
        //undiscovered
        foo.GetComponent<Room>().ChangeIconColor(5);
    }

    /// <summary>
    /// Contrary to its name, it updates the Graph based on the Player's location.
    /// 
    /// It will create new Rooms adjacent to the Player's current room if none exist yet.
    /// </summary>
    void PopulateGraph() {
        //Debug.Log("PopulateGraph: Room # " + playerCurrentRoom);
        Vertex<GameObject> currentRoom = FindRoomByNum(playerCurrentRoom);
        
        //Get the adjacent coordinates of current room and fetch any of those existing vertices
        // 0 = north,
        // 1 = west,
        // 2 = east,
        // 3 = south
        List<Vertex<GameObject>> tempRoom = new List<Vertex<GameObject>>(4) {null, null, null, null };
        foreach (Vertex<GameObject> item in rooms.GetVertices())
        {
            if ((item.GetData().GetComponent<Room>().GetCoordX() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordX()) &
                (item.GetData().GetComponent<Room>().GetCoordY() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordY() + 1))
            {
                //Debug.Log("A northern vertex exists");
                tempRoom[0] = item;
            }

            else if ((item.GetData().GetComponent<Room>().GetCoordX() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordX() - 1) &
                (item.GetData().GetComponent<Room>().GetCoordY() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordY()))
            {
                //Debug.Log("A western vertex exists");
                tempRoom[1] = item;
            }

            else if ((item.GetData().GetComponent<Room>().GetCoordX() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordX() + 1) &
                (item.GetData().GetComponent<Room>().GetCoordY() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordY()))
            {
                //Debug.Log("An eastern vertex exists");
                tempRoom[2] = item;
            }

            else if ((item.GetData().GetComponent<Room>().GetCoordX() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordX()) &
                (item.GetData().GetComponent<Room>().GetCoordY() ==
                currentRoom.GetData().GetComponent<Room>().GetCoordY() - 1))
            {
                //Debug.Log("A southern vertex exists");
                tempRoom[3] = item;
            }
        }

        //check if such vertices exist
        // 0 = north,
        // 1 = west,
        // 2 = east,
        // 3 = south
        for (int i = 0; i < 4; i++)
        {
            Vertex<GameObject> item = tempRoom[i];
            //vertex does not exist, so generate room
            if (item == null)
            {
                if (i == 0)
                {
                    //Debug.Log("A northern vertex does not exist");
                    GenerateRoom(
                        new Vector3(
                            currentRoom.GetData().GetComponent<Room>().GetCenter().x,
                            0,
                            currentRoom.GetData().GetComponent<Room>().GetCenter().z + roomDistanceOffset),
                    currentRoom.GetData().GetComponent<Room>().GetCoordX(),
                    currentRoom.GetData().GetComponent<Room>().GetCoordY() + 1,
                    currentRoom.GetData().GetComponent<Room>().GetDepth() + 1);
                }

                else if (i == 1)
                {
                    //Debug.Log("A western vertex does not exist");
                    GenerateRoom(
                        new Vector3(
                            currentRoom.GetData().GetComponent<Room>().GetCenter().x - roomDistanceOffset,
                            0,
                            currentRoom.GetData().GetComponent<Room>().GetCenter().z),
                    currentRoom.GetData().GetComponent<Room>().GetCoordX() - 1,
                    currentRoom.GetData().GetComponent<Room>().GetCoordY(),
                    currentRoom.GetData().GetComponent<Room>().GetDepth() + 1);
                }

                else if (i == 2)
                {
                    //Debug.Log("An eastern vertex does not exist");
                    GenerateRoom(
                        new Vector3(
                            currentRoom.GetData().GetComponent<Room>().GetCenter().x + roomDistanceOffset,
                            0,
                            currentRoom.GetData().GetComponent<Room>().GetCenter().z),
                    currentRoom.GetData().GetComponent<Room>().GetCoordX() + 1,
                    currentRoom.GetData().GetComponent<Room>().GetCoordY(),
                    currentRoom.GetData().GetComponent<Room>().GetDepth() + 1);
                }

                else if (i == 3)
                {
                    //Debug.Log("A southern vertex does not exist");
                    GenerateRoom(
                        new Vector3(
                            currentRoom.GetData().GetComponent<Room>().GetCenter().x,
                            0,
                            currentRoom.GetData().GetComponent<Room>().GetCenter().z - roomDistanceOffset),
                    currentRoom.GetData().GetComponent<Room>().GetCoordX(),
                    currentRoom.GetData().GetComponent<Room>().GetCoordY() - 1,
                    currentRoom.GetData().GetComponent<Room>().GetDepth() + 1);
                }

                //remember to connect the two rooms
                rooms.AddUndirectedEdge(currentRoom, FindRoomByNum(roomNumber - 1), i, 1);
            }

            //vertex exists
            else {
                //connect the rooms
                //it should not matter if the vertices were already connected, nothing will change
                rooms.AddUndirectedEdge(currentRoom, item, i, 1);
            }
        }
    }

    /// <summary>
    /// Zooms the MainCamera out, then lights up adjacent Rooms to the Player's current Room.
    /// 
    /// It will then toggle any DetectorEnemy lights on if they are in the lit-up Rooms.
    /// </summary>
    public void TelescopeZoomOn() {
        mainCamera.GetComponent<FollowPlayer>().TelescopeZoom();

        Vertex<GameObject> foo = FindRoomByNum(playerCurrentRoom);
        List<int> rooms = new List<int>();
        foreach (Vertex<GameObject> item in foo.GetNeighbors())
        {
            item.GetData().GetComponent<Room>().SetIsZoom(true);
            rooms.Add(item.GetData().GetComponent<Room>().GetNum());
        }

        //turn on enemy lights
        List<GameObject> enemies = objectMaker.GetEnemies();
        foreach (int item in rooms)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<DetectorEnemy>() != null
                    &&
                    enemy.GetComponent<DetectorEnemy>().GetRoom() == item)
                {
                    enemy.GetComponent<DetectorEnemy>().SetIsZoom(true);
                }
                else if (enemy.GetComponent<LampEnemy>() != null
                    &&
                    enemy.GetComponent<LampEnemy>().GetRoom() == item) {
                    enemy.GetComponent<LampEnemy>().SetIsZoom(true);
                }
            }
        }
    }

    /// <summary>
    /// Zooms the MainCamera in, then darkens adjacent Rooms to the Player's current Room.
    /// 
    /// It will then toggle any DetectorEnemy lights off if they are in the darkened Rooms.
    /// </summary>
    public void TelescopeZoomOff() {
        mainCamera.GetComponent<FollowPlayer>().TelescopeZoomOff();

        Vertex<GameObject> foo = FindRoomByNum(playerCurrentRoom);
        List<int> rooms = new List<int>();
        foreach (Vertex<GameObject> item in foo.GetNeighbors())
        {
            item.GetData().GetComponent<Room>().SetIsZoom(false);
            rooms.Add(item.GetData().GetComponent<Room>().GetNum());
        }

        //turn off enemy lights
        List<GameObject> enemies = objectMaker.GetEnemies();
        foreach (int item in rooms)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<DetectorEnemy>() != null
                    &&
                    enemy.GetComponent<DetectorEnemy>().GetRoom() == item)
                {
                    enemy.GetComponent<DetectorEnemy>().SetIsZoom(false);
                }
                else if (enemy.GetComponent<LampEnemy>() != null
                    &&
                    enemy.GetComponent<LampEnemy>().GetRoom() == item) {
                    enemy.GetComponent<LampEnemy>().SetIsZoom(false);
                }
            }
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 1f) {
            pauseButton.color = new Color(58 / 255f, 0, 1);
            pauseText.enabled = true;
            Freeze();
        }
        else {
            pauseButton.color = Color.white;
            pauseText.enabled = false;
            Unfreeze();
        }
    }

    void Freeze() {
        Time.timeScale = 0f;
    }

    void Unfreeze() {
        Time.timeScale = 1f;
    }

    public void ToggleMap() {
        minimap.enabled = !minimap.enabled;
    }
}
