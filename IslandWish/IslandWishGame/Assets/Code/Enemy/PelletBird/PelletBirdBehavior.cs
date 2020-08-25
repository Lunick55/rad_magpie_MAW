using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PelletBirdBehavior : EnemyBehavior
{
    [SerializeField] GameObject rangedAttack;

    [SerializeField] float outerRange = 0, innerRange = 0, sightRange = 0;
    [SerializeField] Transform pelletSpawn;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

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

        if (GetPlayerDistanceSquared() < (sightRange * sightRange))      //if the player is within sight of the enemy, enable agent, and give chase
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
        if (GetPlayerDistanceSquared() < (agent.stoppingDistance * agent.stoppingDistance))
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
        
        agent.destination = playerTrans.position;
    }

    public void AttackPlayer()
	{
        if(GetPlayerDistanceSquared() < (innerRange * innerRange)) //player too close, flee
		{
            anim.SetTrigger(playerTooClose);
            timer = 0;
            EnableAgent();
            canRotate = false;
            return;
		}
        else if(GetPlayerDistanceSquared() > (outerRange * outerRange))                                //player too far, chase
        {
            anim.SetTrigger(playerInSight);
            timer = 0;
            EnableAgent();
            canRotate = false;
            return;
        }

        timer += Time.deltaTime;
        if(timer >= stats.timeBetweenAttacks)
		{
            anim.SetTrigger("Shoot");
            //RangedAttack();
        }

        //something??
    }

    public void FleePlayer()
    {
        //if the player is in range, attack
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

    public void RangedAttack()
    {
        print("ATTACK");
        //instantiate attack, send it out

        timer = 0;
        GameObject newRangedAttack = Instantiate(rangedAttack, pelletSpawn.position, transform.rotation);
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
                isDead = true;
                gameObject.SetActive(false);
                //Destroy(gameObject);
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
                isDead = true;
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);
#endif
    }
}
