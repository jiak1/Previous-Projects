using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyZombieAI : MonoBehaviour
{

    public float wanderRadius = 200;
    public float wanderTimer = 10;
    float acnoweldgePlayerDistance = 50f;
    public float sightDist = 25f;
    Transform player;
    float heightMultiplier = 1.0f;
    private NavMeshAgent agent;
    private float timer;
    public bool debugMode = false;
    bool chasingPlayer = false;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player1").transform;
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player1").transform;
            Debug.Log("Cant find player...");
        }


        //If enemy is further then say 50 metres from player lets not bother with raycasting
        if (Vector3.Distance(agent.transform.position, player.transform.position) > acnoweldgePlayerDistance)
        {
            DoWander();
        }
        else
        {

            RaycastHit hit;

            if (debugMode)
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward, Color.green);
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward + transform.right, Color.green);
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward - transform.right, Color.green);
            }

            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDist))
            {
                if (hit.collider.transform == player)
                {
                    //Can see player
                    chasingPlayer = true;
                }

            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward + transform.right, out hit, sightDist))
            {
                if (hit.collider.transform == player)
                {
                    //Can see player
                    chasingPlayer = true;
                }

            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward - transform.right, out hit, sightDist))
            {
                if (hit.collider.transform == player)
                {
                    //Can see player
                    chasingPlayer = true;
                }

            }
            if (chasingPlayer)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                DoWander();
            }
        }




    }

    void DoWander()
    {
        chasingPlayer = false;
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = PickRandomLocationOnNavMeshWithinDistance(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }


    public static Vector3 PickRandomLocationOnNavMeshWithinDistance(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        //Layermask -1 is everything
        NavMesh.SamplePosition(randDirection, out navHit, dist, -1);

        return navHit.position;
    }
}
