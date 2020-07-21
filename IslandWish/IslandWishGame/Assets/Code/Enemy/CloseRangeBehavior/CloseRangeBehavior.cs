using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloseRangeBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    [SerializeField] Transform player;
    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    [SerializeField] float sightRange = 0;

    public EnemyStats stats;
    private int currentHealth;


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        SceneLinkedSMB<CloseRangeBehavior>.Initialise(anim, this);

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

        //if the player is within sight of the enemy, enable agent, and give chase
        if ((player.position - transform.position).magnitude < sightRange)
		{
            anim.SetBool("PlayerInSight", true);
            EnableAgent();
        }

        agent.destination = transform.position;
    }

    public void ChasePlayer()
    {
        print("Chase Player");

        //if player is within attack range, stop and attack
        if ((player.position - transform.position).magnitude < agent.stoppingDistance)
        {
            anim.SetTrigger("PlayerInRange");
            EnableObstacle();
        }
        //else if the player is out of sight, go back to idle
        else if ((player.position - transform.position).magnitude > sightRange)
        {
            anim.SetBool("PlayerInSight", false);
            EnableAgent();
        }
        else
		{
            agent.destination = player.position;
        }

    }

    public void MeleeAttack()
    {
        print("ATTACK");
        //attack logic
    }

    public void EnableAgent()
    {
        agent.enabled = true;
        obstacle.enabled = false;
    }

    public void EnableObstacle()
    {
        agent.enabled = false;
        obstacle.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            currentHealth -= 1;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
            }
        }
    }

    private void OnDrawGizmosSelected()
	{
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
	}
}
