using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float wheelSpeed = 100.0f;
    public List<GameObject> wheels = new List<GameObject>();
    public float currentAngle = 0;

    private void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        WheelRotation();
    }

    public void WheelRotation()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right * wheelSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "RotationWaypoint")
        {
            var waypoint = other.gameObject.GetComponent<Waypoint>();
            if (waypoint.waypointSO.IsMisseable)
            {
                var random = UnityEngine.Random.Range(0, 2);
                Debug.Log(random);
                if (random == 0)
                {
                    return;
                }
            }
            var rotation = currentAngle + waypoint.waypointSO.TurnAngle;
            
            switch (waypoint.waypointSO.TurnDirection)
            {
                case "Left":
                    transform.DORotate(new Vector3(0, rotation, 0), waypoint.waypointSO.TurnTime).SetEase(Ease.Linear);
                    break;
                case "Right":
                    transform.DORotate(new Vector3(0, rotation, 0), waypoint.waypointSO.TurnTime).SetEase(Ease.Linear);
                    break;
            }
            
            currentAngle = rotation;
        }
    }
}
