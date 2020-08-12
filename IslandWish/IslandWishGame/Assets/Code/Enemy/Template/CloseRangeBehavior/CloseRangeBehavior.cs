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
    private string playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    public EnemyStats stats;
    private int currentHealth;

    private bool canRotate = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        SceneLinkedSMB<CloseRangeBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        //hurtbox.SetActive(false);
    }

    private void Update()
    {
        if (canRotate)
        {
            RotateTowardsPlayer();
        }
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
            anim.SetBool(playerInSight, true);
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
            anim.SetTrigger(playerInRange);
            EnableObstacle();
        }
        //else if the player is out of sight, go back to idle
        else if ((player.position - transform.position).magnitude > sightRange)
        {
            anim.SetBool(playerInSight, false);
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

    void RotateTowardsPlayer()
    {
        // from https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html

        // Determine which direction to rotate towards
        Vector3 targetDirection = player.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = 5 * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
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
#if UNITY_EDITOR 
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
#endif
    }
}
