using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PelletBirdBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    Player player;
    Transform playerTrans;
    [SerializeField] GameObject hitbox, rangedAttack;

    Animator anim;

    [SerializeField] float outerRange = 0, innerRange = 0, sightRange = 0;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    public EnemyStats stats;
    private int currentHealth;
    private bool canRotate = false;

    void Start()
    {
        player = GameManager.Instance.player;
        playerTrans = GameManager.Instance.playerTrans;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.stoppingDistance = outerRange;

        SceneLinkedSMB<PelletBirdBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        //hurtbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
		{
            RotateTowardsPlayer();
		}
    }

    public void Idle()
    {
        print("Idle");
        //agent should already be enabled

        if ((playerTrans.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        {
            EnableAgent();
            anim.SetTrigger(playerTooClose);

            canRotate = false;
        }
        else if ((playerTrans.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, beat 'em up
        {
            EnableObstacle();
            anim.SetTrigger(playerInRange);

            canRotate = true;
        }
        else if ((playerTrans.position - transform.position).magnitude < sightRange)                   //if the player is within sight of the enemy, enable agent, and give chase
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
        if ((playerTrans.position - transform.position).magnitude < agent.stoppingDistance)
        {
            anim.SetTrigger(idle);
            EnableObstacle();
        }
        //else if the player is out of sight, go back to idle
        else if ((playerTrans.position - transform.position).magnitude > sightRange)
        {
            anim.SetTrigger(idle);
            EnableAgent();
        }
        else
        {
            agent.destination = playerTrans.position;
        }
    }

    public void FleePlayer()
    {
        //if the player is too close, flee
        if ((playerTrans.position - transform.position).magnitude < innerRange)
        {
            agent.stoppingDistance = 0;
            Vector3 dirToPlayer = transform.position - playerTrans.position;
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

        GameObject newRangedAttack = Instantiate(rangedAttack, transform.position, transform.rotation);
        newRangedAttack.GetComponent<RangedAttackCollision>().InitDamage(stats.attack, 3);
        
        newRangedAttack.GetComponent<Rigidbody>().velocity = transform.forward * 8;
    }

    public void EndAttack()
	{
        EnableAgent();
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

    void RotateTowardsPlayer()
    {
        // from https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
        
        // Determine which direction to rotate towards
        Vector3 targetDirection = playerTrans.position - transform.position;
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
        if (other.tag == "MeleeAttack")
        {
            currentHealth -= player.stats.spearDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                Destroy(gameObject);
            }
        }
        else if (other.tag == "SlingshotAttack")
        {
            currentHealth -= player.stats.slingDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);
    }
}
