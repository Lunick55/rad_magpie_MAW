using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfDeerBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    [SerializeField] Transform player;
    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    [SerializeField] float outerRange = 0, innerRange = 0, sightRange = 0;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle", attack = "Attack";

    public EnemyStats stats;
    private int currentHealth;
    private bool canRotate = false;

    [SerializeField] float attackSpeed = 0, attackDistance = 0, safetyTimer = 1;
    float timer = 0;
    Vector3 destination = Vector3.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.stoppingDistance = outerRange;

        SceneLinkedSMB<WolfDeerBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        hurtbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
        {
            RotateTowardsPlayer();
        }
    }

    public void Idle()
    {
        print("Idle");
        //agent should already be enabled

        if ((player.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        {
            EnableAgent();
            anim.SetTrigger(playerTooClose);

            canRotate = false;
        }
        else if ((player.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, beat 'em up
        {
            EnableObstacle();
            anim.SetTrigger(playerInRange);

            canRotate = true;
        }
        else if ((player.position - transform.position).magnitude < sightRange)                   //if the player is within sight of the enemy, enable agent, and give chase
        {
            EnableAgent();
            anim.SetTrigger(playerInSight);

            canRotate = false;
        }

        if (agent.enabled)
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
            EnableAgent();

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

    public void Aim()
	{
        print("Aiming...");

        if(IsFacingPlayer())
		{
            canRotate = false;
            anim.SetTrigger(attack);
		}
	}

    public void StartAttack()
	{
        //start collisions n' stuff
        timer = 0;
        
        //TODO: maybe don't fly off the edge. try to math it out with spherecasts or something
        destination = transform.position + transform.forward * attackDistance;

    }

    public void ChargeAttack()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector3.forward * attackSpeed);

        if ((transform.position - destination).magnitude < 1f || timer > safetyTimer)
		{
            anim.SetTrigger("DoneAttacking");
		}
    }

    public void EndAttack()
	{
        //disable collisions n'stuff
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

    private bool IsFacingPlayer()
    {
        float minAngle = 15;

        Vector3 dirToPlayer = player.position - transform.position;
        dirToPlayer.y = 0;

        if (Vector3.Angle(transform.forward, dirToPlayer) < minAngle)
        {
            canRotate = false;
            return true;
        }

        return false;
    }

    void RotateTowardsPlayer()
    {
        // from https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html

        // Determine which direction to rotate towards
        Vector3 targetDirection = player.position - transform.position;
        targetDirection.y = 0;
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
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);

        UnityEditor.Handles.DrawLine(transform.position, transform.position + transform.forward * attackDistance);
        UnityEditor.Handles.DrawWireDisc(destination, Vector3.up, 1);
    }
}
