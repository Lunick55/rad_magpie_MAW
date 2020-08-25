using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockElementalBehavior : EnemyBehavior
{
    [SerializeField] GameObject lobbedAttack;
    [SerializeField] Collider[] smashAttack;

    [SerializeField] float sightRange = 0, outerRange = 0, innerRange = 0, lobbedAngle = 45;
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

        SceneLinkedSMB<RockElementalBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        //hurtbox.SetActive(false);
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

        agent.destination = playerTrans.position;
    }

    public void RangedAttack()
	{
        print("am in combat baybee");

        if (GetPlayerDistanceSquared() < (innerRange * innerRange)) //player too close, flee
        {
            anim.SetTrigger(playerTooClose);
            timer = 0;
            EnableAgent();
            canRotate = false;
            return;
        }
        else if (GetPlayerDistanceSquared() > (outerRange * outerRange))                                //player too far, chase
        {
            anim.SetTrigger(playerInSight);
            timer = 0;
            EnableAgent();
            canRotate = false;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= stats.timeBetweenAttacks)
        {
            anim.SetTrigger("Throw");
            LobbedAttack();
            return;
        }

        //something??
    }

    public void SmashAttack()
    {
        if (GetPlayerDistanceSquared() > (innerRange * innerRange))
        {
            agent.stoppingDistance = outerRange;
            anim.SetTrigger(playerInRange);
            EnableObstacle();
            canRotate = true;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= stats.timeBetweenAttacks)
        {
            timer = 0;

            int randNum = Random.Range(0, 2);

            switch (randNum)
            {
                case 0:
                    anim.SetTrigger("AttackLeft");
                    break;

                case 1:
                    anim.SetTrigger("AttackRight");
                    break;

                default:
                    anim.SetTrigger("AttackLeft");
                    break;
            }
        }

        //something??
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

    public void LobbedAttack()
    {
        //instantiate attack, send it out
        timer = 0;

        GameObject newLobbedAttack = Instantiate(lobbedAttack, transform.position, transform.rotation);
        newLobbedAttack.GetComponent<RangedAttackCollision>().InitDamage(stats.attack, 3);
        Vector3 target = playerTrans.position;

        Vector3 targetDir = target - transform.position; // get Target Direction
        float height = targetDir.y; // get height difference
        targetDir.y = 0; // retain only the horizontal difference
        float dist = targetDir.magnitude; // get horizontal direction
        float a = lobbedAngle * Mathf.Deg2Rad; // Convert angle to radians
        targetDir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        newLobbedAttack.GetComponent<Rigidbody>().velocity = velocity * targetDir.normalized;
        newLobbedAttack.GetComponent<Rigidbody>().useGravity = true;
    }

    public void StartSmash(bool leftSmash, bool rightSmash)
    {
        Debug.Log("FIST OF HAVOK");
        smashAttack[0].enabled = leftSmash;
        smashAttack[1].enabled = rightSmash;
    }

    public void EndSmash()
    {
        smashAttack[0].enabled = false;
        smashAttack[1].enabled = false;
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
