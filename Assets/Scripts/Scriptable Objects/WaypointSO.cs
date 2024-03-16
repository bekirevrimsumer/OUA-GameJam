using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waypoints", menuName = "Item/Waypoints")]
public class WaypointSO : ScriptableObject
{
    public string TurnDirection;
    public float TurnAngle;
    public float TurnTime;
    public bool IsMisseable;
}
