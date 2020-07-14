using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    [SerializeField] Transform player;

    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    [SerializeField] float outerRange = 0, innerRange = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.destination = player.position;

        SceneLinkedSMB<RangerBehavior>.Initialise(anim, this);

        //hurtbox.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void FollowPlayer()
    {
        agent.destination = player.position;
    }

    public void FleePlayer()
    {
        if ((player.position - transform.position).magnitude < innerRange)
        {
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 fleePos = transform.position + dirToPlayer;
            agent.SetDestination(fleePos);
        }
        else
		{
            anim.SetTrigger("Idle");
		}
    }

    public void CheckAction()
    {
        if ((player.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, take a breather
        {
            anim.SetTrigger("Attack");
            obstacle.enabled = true;
            agent.enabled = false;
        }
        if ((player.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        {
            anim.SetTrigger("Flee");
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 fleePos = transform.position + dirToPlayer;
            agent.SetDestination(fleePos);
        }
        else                                                                            //chase a bitch
        {
            obstacle.enabled = false;
            agent.enabled = true;
            FollowPlayer();
        }
    }

    public void StartChase()
    {
        agent.isStopped = false;
    }

    public void StopChase()
    {
        agent.isStopped = true;
    }

    public void MeleeAttack()
    {
        //attack logic?
    }
}