using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfDeerBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    Player player;
    Transform playerTrans;
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
    private bool aggro = false;

    void Start()
    {
        player = GameManager.Instance.player;
        playerTrans = GameManager.Instance.playerTrans;

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

        //if ((playerTrans.position - transform.position).magnitude < innerRange)              //if too close, back dat ass up
        //{
        //    EnableAgent();
        //    anim.SetTrigger(playerTooClose);

        //    canRotate = false;
        //}
        //else if ((playerTrans.position - transform.position).magnitude < agent.stoppingDistance)  //if in range, beat 'em up
        //{
        //    EnableObstacle();
        //    anim.SetTrigger(playerInRange);

        //    canRotate = true;
        //}
        if (GetPlayerDistanceSquared() < (sightRange * sightRange))                   //if the player is within sight of the enemy, enable agent, and give chase
        {
            EnableAgent();
            anim.SetTrigger(playerInSight);

            canRotate = false;
            return;
        }

        if (agent.enabled)
            agent.destination = transform.position;
    }

    public void ChasePlayer()
    {
        print("Chase Player");

        //if player is within attack range, stop and attack
        if (GetPlayerDistanceSquared() < (outerRange * outerRange))
        {
            anim.SetTrigger(playerInRange);
            EnableObstacle();
            canRotate = true;
            return;
        }
        //else if the player is out of sight, go back to idle
        else if (GetPlayerDistanceSquared() > (sightRange * sightRange))
        {
            anim.SetTrigger(idle);
            EnableAgent();
            return;
        }
        else
        {
            //EnableAgent();

            agent.destination = playerTrans.position;
        }
    }

    public void FleePlayer()
    {
        //if the player is too far, attack
        if (GetPlayerDistanceSquared() > (innerRange * innerRange))
        {
            agent.stoppingDistance = outerRange;
            anim.SetTrigger(playerInRange);
            EnableObstacle();
            canRotate = true;
            return;
        }

        agent.stoppingDistance = 0;
        Vector3 dirToPlayer = transform.position - playerTrans.position;
        Vector3 fleePos = transform.position + dirToPlayer;
        agent.destination = fleePos;

    }

    public void Aim()
    {
        print("Aiming...");

        if(GetPlayerDistanceSquared() < (innerRange * innerRange))      //player too close flee
		{
            anim.SetTrigger(playerTooClose);
            EnableAgent();
            canRotate = false;
            return;
        }
        else if(GetPlayerDistanceSquared() > (outerRange * outerRange)) //player too far chase
		{
            anim.SetTrigger(playerInSight);
            EnableAgent();
            canRotate = false;
            return;
        }

        if (IsFacingPlayer())
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
        canRotate = true;
    }

    public void Aggro()
    {
        GameManager.Instance.IncreaseAggro();
        aggro = true;
    }

    public void DeAggro()
    {
        GameManager.Instance.DecreaseAggro();
        aggro = false;
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

        Vector3 dirToPlayer = playerTrans.position - transform.position;
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

    float GetPlayerDistanceSquared()
    {
        return (playerTrans.position - transform.position).sqrMagnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeleeAttack")
        {
            GameManager.Instance.audioManager.Play("SpearHit");
            currentHealth -= player.stats.spearDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                if (aggro)
                {
                    DeAggro();
                }
                Destroy(gameObject);
            }
        }
        else if (other.tag == "SlingshotAttack")
        {
            GameManager.Instance.audioManager.Play("SlingHit");
            currentHealth -= player.stats.slingDamage;
            if (currentHealth <= 0)
            {
                print("Enemy is Dead and You Killed Them You Monster");
                if (aggro)
                {
                    DeAggro();
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);

        UnityEditor.Handles.DrawLine(transform.position, transform.position + transform.forward * attackDistance);
        UnityEditor.Handles.DrawWireDisc(destination, Vector3.up, 1);
#endif
    }
}
