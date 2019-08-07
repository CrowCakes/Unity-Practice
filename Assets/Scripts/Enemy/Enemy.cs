using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //the player being kept track of by the gamemanager
    protected GameObject player;

    protected const float on = 1f;
    protected const float hue = 80;

    //damage dealt by enemy
    protected int damage = 10;

    // the room they are in
    protected int roomNumber = 0;

    // whether or not telescope is zooming
    protected bool isZoom = false;

    // whether or not it sees an enemy
    protected bool isAlerted = false;

    protected GameManager manager;

    /// <summary>
    /// Sets the position of the Enemy. Typically, the position will be the center of the Room they will be placed in.
    /// </summary>
    /// <param name="pos">The center of the Room the Enemy will be found in</param>
    public virtual void SetCenter(Vector3 pos)
    {
        transform.position = pos;
    }
    
    /// <summary>
    /// Gets the number of the Room the Enemy is found.
    /// </summary>
    /// <returns></returns>
    public int GetRoom()
    {
        return roomNumber;
    }

    /// <summary>
    /// Sets the number of the Room the Enemy is found.
    /// </summary>
    /// <param name="num"></param>
    public void SetRoom(int num)
    {
        roomNumber = num;
    }

    /// <summary>
    /// Returns whether or not the Room is currently being viewed through a Telescope.
    /// </summary>
    /// <returns></returns>
    public bool GetIsZoom()
    {
        return isZoom;
    }

    /// <summary>
    /// Sets whether or not the Room is currently being viewed through a Telescope.
    /// </summary>
    /// <param name="b"></param>
    public void SetIsZoom(bool b)
    {
        isZoom = b;
    }

    /// <summary>
    /// Gets whether or not the Enemy currently has line of sight with the Player.
    /// </summary>
    /// <returns></returns>
    public bool GetIsAlerted()
    {
        return isAlerted;
    }

    /// <summary>
    /// Sets whether or not the Enemy currently has line of sight with the Player.
    /// </summary>
    /// <param name="b"></param>
    public void SetIsAlerted(bool b)
    {
        isAlerted = b;
    }

    /// <summary>
    /// Checks whether or not the Enemy is inside of its Room, then adjusts its position if it is.
    /// </summary>
    /// <returns></returns>
    protected virtual void IsOutOfBounds() {
        
    }
}
