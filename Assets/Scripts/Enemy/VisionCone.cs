using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionCone : MonoBehaviour
{
    Transform LookOrigin;
    private const float FieldOfView = 20f;
    private const float MaxViewDistance = 9f;

    //Each time IsInLineOfSight is called, this value is replaced
    private Transform _firstVisiblePoint;
    //used to determine if the actor can see the enemy's critical hit point
    private bool _isCriticalAvailable = false;

    private void Start()
    {
        LookOrigin = gameObject.transform;
    }

    public Transform FirstVisiblePoint
    {
        get { return _firstVisiblePoint; }
    }

    public bool IsCriticalAvailable
    {
        get { return _isCriticalAvailable; }
    }

    /// <summary>
    /// Determines if this actor is currently within the line of sight.  Must be called before any other methods,
    /// populates VisiblePoints list and clears out previous data.
    /// </summary>
    /// <param name="actor">The gameObject in question</param>
    /// <returns>true if actor is within line of sight, false otherwise</returns>
    public bool IsInLineOfSight(GameObject actor)
    {
        _firstVisiblePoint = null;
        _isCriticalAvailable = false;

        //In order to have line of sight, the actor must be within this unit's maximum view distance
        if (Vector3.Distance(transform.position, actor.transform.position) <= MaxViewDistance)
        {
            //next, the actor target must be sufficiently in front of this unit, based on their field of view
            if (Vector3.Angle(actor.transform.position - transform.position, transform.forward) <= FieldOfView)
            {
                //finally, there must be no obstacles between this unit and the target
               
                    RaycastHit hitInfo;

                    if (Physics.Linecast(LookOrigin.position, actor.transform.position, out hitInfo))
                    {
                        //if the linecast detects a target point inside the target's character controller,
                        //add that target point to the visible points list
                        if (hitInfo.transform.tag == "Player")
                        {
                            
                                _isCriticalAvailable = true;
                            

                            _firstVisiblePoint = actor.transform;
                            //Debug.Log("unit can see the enemy's torso");
                            return true;
                        }
                    }
                    //otherwise if the linecast was unsuccessful, there is nothing between the unit and it's target
                    else
                    {
                        
                            _isCriticalAvailable = true;
                        

                        _firstVisiblePoint = actor.transform;
                        //Debug.Log("unit can see other spots");
                        return true;
                    }
                
            }
        }

        return false;
    }
}
