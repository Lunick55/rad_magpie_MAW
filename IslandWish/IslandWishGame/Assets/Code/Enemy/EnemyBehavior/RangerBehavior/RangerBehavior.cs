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

        agent.stoppingDistance = outerRange;
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
            agent.stoppingDistance = 0;
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 fleePos = transform.position + dirToPlayer;
            agent.destination = fleePos;
        }
        else
		{
            agent.stoppingDistance = outerRange;
            anim.SetTrigger("Idle");
		}
    }

    public void CheckAction()
    {
        if ((player.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        {
            EnableAgent();
            anim.SetTrigger("Flee");
        }
        else if ((player.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, take a breather
        {
            EnableObstacle();
            anim.SetTrigger("Attack");
        }
        else                                                                            //chase a bitch
        {
            EnableAgent();
            FollowPlayer();
        }
    }

    void EnableAgent()
	{
        obstacle.enabled = false;
        agent.enabled = true;
    }

    void EnableObstacle()
	{
        obstacle.enabled = true;
        agent.enabled = false;
    }

    public void StartChase()
    {
        //agent.isStopped = false;
    }

    public void StopChase()
    {
        //agent.isStopped = true;
    }

    public void RangedAttack()
    {
        //instantiate attack, send it out
    }
}