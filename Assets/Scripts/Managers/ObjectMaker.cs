using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will not generate rooms because the game manager has to operate on those
//this class is also responsible for destroying objects other than rooms
public class ObjectMaker : MonoBehaviour {
    //0 = Detector
    //1 = Lamp
    //2 = Furnace
    public List<GameObject> enemyPrefabs;

    //0 = Telescope
    //1 = HealCapsule
    public List<GameObject> objectPrefabs;

    private const float telescopeBoundX = 4.5f;
    private const float telescopeBoundY = 4.5f;

    public float healCapsuleChance = 0.99f;
    public float telescopeChance = 0.95f;
    public float detectorEnemyChance = 0.6f;
    public float furnaceEnemyChance = 0.8f;
    public float lampEnemyChance = 0.7f;

    List<GameObject> enemies = new List<GameObject>();

    /// <summary>
    /// Populates a Room with objects based on arbitrarily given chances.
    /// </summary>
    /// <param name="pos">Physical position of the Room</param>
    /// <param name="room">The Room's number</param>
    /// <returns>0 if it made a non-furnace Enemy, 1 if it made a furnace Enemy, 2 if it made a Telescope, 3 otherwise</returns>
    public int PopulateRoom(Vector3 pos, int room) {
        int result = 3;
        //if a telescope/healcapsule appears, there must be no enemies in the room
        
        //1% chance for healcapsule
        //not allowed in room 1
        if (RollDice() > healCapsuleChance)
        {
            if (room > 1)
            {
                MakeHealCapsule(pos);
                result = 3;
            }
        }
        //5% chance for telescope
        else if (RollDice() > telescopeChance)
        {
            //Debug.Log("Making telescope for: " + pos);
            MakeTelescope(pos);
            result = 2;
        }
        //94% chance for enemy
        else {
            //no enemies allowed in room 1
            if (room > 1) {
                //40% chance for detector
                if (RollDice() > detectorEnemyChance)
                {
                    MakeDetectorEnemy(pos, room);
                    result = 0;
                    //don't return yet because lamp can still spawn
                }
                //furnace must be the only enemy in a room
                //20% chance for furnace
                else if (RollDice() > furnaceEnemyChance) {
                    MakeFurnaceEnemy(pos, room);
                    return 1;
                }

                //30% chance for lamp
                if (RollDice() > lampEnemyChance)
                {
                    MakeLampEnemy(pos, room);
                    result = 0;
                }
            }
            else { return 3; }
        }
        return result;
    }

    /// <summary>
    /// Makes a Telescope within the bounds of a given Room center.
    /// </summary>
    /// <param name="pos">The physical center of a given Room</param>
    public void MakeTelescope(Vector3 pos) {
        GameObject foo = (GameObject)Instantiate(objectPrefabs[0]);
        foo.name = "Telescope";
        if (foo != null)
        {
            /*
            Vector3 center = new Vector3(
                pos.x + Random.Range(-telescopeBoundX, telescopeBoundX),
                pos.y,
                pos.z + Random.Range(-telescopeBoundY, telescopeBoundY)
                );

            foo.transform.position = center;
                */
            Vector3 center = pos;
            
            foo.GetComponent<Telescope>().SetCenter(center);
        }
        else {
            Debug.Log("null telescope?");
        }
    }

    /// <summary>
    /// Makes a HealCapsule in the given position of the given Room.
    /// </summary>
    /// <param name="pos">Position in the Room</param>
    public void MakeHealCapsule(Vector3 pos) {
        GameObject foo = (GameObject)Instantiate(objectPrefabs[1]);
        foo.name = "HealCapsule";
        if (foo != null)
        {
            foo.transform.position = new Vector3(pos.x, foo.transform.position.y, pos.z);
        }
        else {
            Debug.Log("null healcapsule?");
        }
    }

    /// <summary>
    /// Makes a DetectorEnemy within the bounds of a given Room center.
    /// </summary>
    /// <param name="pos">The physical center of a given Room</param>
    /// <param name="room">The Room's number</param>
    public void MakeDetectorEnemy(Vector3 pos, int room) {
        GameObject foo = (GameObject)Instantiate(enemyPrefabs[0]);
        foo.name = "Detector" + room;
        if (foo != null)
        {
            Vector3 center = pos;
            //foo.transform.position = center;
            foo.GetComponent<DetectorEnemy>().SetCenter(center);
            foo.GetComponent<DetectorEnemy>().SetRoom(room);
            enemies.Add(foo);
        }
        else
        {
            Debug.Log("null enemy?");
        }

        //Debug.Log("Made enemy in room " + room);
    }

    /// <summary>
    /// Makes a LampEnemy within the bounds of a given Room center.
    /// </summary>
    /// <param name="pos">The physical center of a given Room</param>
    /// <param name="room">The Room's number</param>
    public void MakeLampEnemy(Vector3 pos, int room)
    {
        //new WaitForSeconds(1);
        GameObject foo = (GameObject)Instantiate(enemyPrefabs[1]);
        foo.name = "Lamp" + room;
        if (foo != null)
        {
            Vector3 center = pos;
            //foo.transform.position = center;
            foo.GetComponent<LampEnemy>().SetCenter(center);
            foo.GetComponent<LampEnemy>().SetRoom(room);
            enemies.Add(foo);
        }
        else
        {
            Debug.Log("null enemy?");
        }

        //Debug.Log("Made enemy in room " + room);
    }

    /// <summary>
    /// Makes a FurnaceEnemy within the bounds of a given Room center.
    /// </summary>
    /// <param name="pos">The physical center of a given Room</param>
    /// <param name="room">The Room's number</param>
    public void MakeFurnaceEnemy(Vector3 pos, int room)
    {
        //new WaitForSeconds(2);
        GameObject foo = (GameObject)Instantiate(enemyPrefabs[2]);
        foo.name = "Furnace" + room;
        if (foo != null)
        {
            Vector3 center = pos;
            //foo.transform.position = center;
            foo.GetComponent<FurnaceEnemy>().SetCenter(center);
            foo.GetComponent<FurnaceEnemy>().SetRoom(room);
            enemies.Add(foo);
        }
        else
        {
            Debug.Log("null enemy?");
        }

        //Debug.Log("Made enemy in room " + room);
    }

    /// <summary>
    /// Return the List of Enemies created so far.
    /// </summary>
    /// <returns>The List of Enemies created so far</returns>
    public List<GameObject> GetEnemies() {
        return enemies;
    }

    /// <summary>
    /// Returns a random float between 0 and 1.
    /// </summary>
    /// <returns>A float between 0 and 1</returns>
    float RollDice() {
        return Random.Range(0f, 1f);
    }
}
