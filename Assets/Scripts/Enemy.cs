using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : TargetScript
{
    public Animator animator;
    public GameObject healthBarUI;
    public Slider slider;

    //Enemymove
    public Transform[] waypoints;
    public float speed;

    private int waypointIndex;
    private float dist;

 

    //public float health = 50; //enemy health
    public float health;
    public float Maxhealth;

    float coolDown = 0.5f;
    //public NavMeshAgent NavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {   //Health
        health = Maxhealth;
        slider.value = CalculateHealth();

        //moving
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {

        slider.value = CalculateHealth();
        healthBarUI.SetActive(true);

        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if (dist < 1f)
        {
            IncreaseIndex();

        }
        Patrol();
        

        if (isHit && coolDown <=0)
        {
            Debug.Log("HIT");
            animator.SetTrigger("Hurt");
            health -= 10;
            coolDown = 0.5f;
            if (health <= 0)
            {
                animator.SetTrigger("Dead");
                
                health = 0;
                waypointIndex = 0;
                //healthBarUI.SetActive(false);
                speed = 0;
            }
            else
            {
                animator.SetTrigger("Hurt");
                
            }
            isHit = false;

        }
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
    }


    //health calculate and see on UI
    float CalculateHealth()
    {
        return health / Maxhealth;
    }

    //enemy move
    void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
    }

}
