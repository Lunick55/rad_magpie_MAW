using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoconapperBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    [SerializeField] Transform player;
    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    [SerializeField] float sightRange = 0, attackRange = 0;
    private string playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    public EnemyStats stats;
    private int currentHealth;

    private bool canRotate = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        SceneLinkedSMB<CoconapperBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        agent.stoppingDistance = attackRange;

        hurtbox.SetActive(false);
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

        if (agent.enabled)
        {
            agent.destination = transform.position;
        }
    }

    public void ChasePlayer()
    {
        print("Chase Player");

        //if player is within attack range, stop and attack
        if ((player.position - transform.position).magnitude < attackRange)
        {
            canRotate = true;
            EnableObstacle();

            if (IsFacingPlayer())
            {
                anim.SetTrigger(playerInRange);
            }

        }
        //else if the player is out of sight, go back to idle
        else if ((player.position - transform.position).magnitude > sightRange)
        {
            canRotate = false;
            anim.SetBool(playerInSight, false);
            EnableAgent();
        }
        else
        {
            EnableAgent();

            canRotate = false;
            agent.destination = player.position;
        }

    }

    public void MeleeAttack()
    {
        print("ATTACK");
        hurtbox.SetActive(true);
    }

    public void FinishMeleeAttack()
    {
        hurtbox.SetActive(false);
        EnableAgent();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeleeAttack")
        {
            currentHealth -= GameManager.Instance.player.stats.spearDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                Destroy(gameObject);
            }
        }
        else if (other.tag == "SlingshotAttack")
        {
            currentHealth -= GameManager.Instance.player.stats.slingDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
}
