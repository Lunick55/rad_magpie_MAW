using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LongRangeBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    
    [SerializeField] Transform player;
    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    [SerializeField] float outerRange = 0, innerRange = 0, sightRange = 0;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    public EnemyStats stats;
    private int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.stoppingDistance = outerRange;

        SceneLinkedSMB<LongRangeBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        //hurtbox.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void Idle()
    {
        print("Idle");
        //agent should already be enabled

        if ((player.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        {
            EnableAgent();
            anim.SetTrigger(playerTooClose);
        }
        else if ((player.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, take a breather
        {
            EnableObstacle();
            anim.SetTrigger(playerInRange);
        }
        else if ((player.position - transform.position).magnitude < sightRange)                   //if the player is within sight of the enemy, enable agent, and give chase
        {
            EnableAgent();
            anim.SetTrigger(playerInSight);
        }

        if(agent.enabled)
        agent.destination = transform.position;
    }

    public void ChasePlayer()
    {
        print("Chase Player");

        //if player is within attack range, stop and attack
        if ((player.position - transform.position).magnitude < agent.stoppingDistance)
        {
            anim.SetTrigger(idle);
            EnableObstacle();
        }
        //else if the player is out of sight, go back to idle
        else if ((player.position - transform.position).magnitude > sightRange)
        {
            anim.SetTrigger(idle);
            EnableAgent();
        }
        else
        {
            agent.destination = player.position;
        }
    }

    public void FleePlayer()
    {
        //if the player is too close, flee
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
            anim.SetTrigger(idle);
        }
    }

    public void RangedAttack()
    {
        print("ATTACK");
        //instantiate attack, send it out
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

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PlayerAttack")
		{
            currentHealth -= 1;
            if(currentHealth <= 0)
			{
                print("Enemy is Dead and You Killed Them You Monster");
			}
		}
	}

	private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);
    }
}