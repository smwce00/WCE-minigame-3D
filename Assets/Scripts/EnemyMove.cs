using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;

    private int waypointIndex;
    private float dist;


    private void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position,waypoints[waypointIndex].position);
        if (dist < 1f)
        {
            IncreaseIndex();

        }Patrol();
    
        
    }

    void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void IncreaseIndex()
    {
        waypointIndex++;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
    }
}
