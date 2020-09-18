using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockElementalBehavior : EnemyBehavior
{
    [SerializeField] GameObject walkingPoofs;
    [SerializeField] GameObject lobbedAttack;
    [SerializeField] Collider[] smashAttack;
    [SerializeField] Transform throwSpawn;

    [SerializeField] float sightRange = 0, outerRange = 0, innerRange = 0, lobbedAngle = 45;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    private bool canRotate = false;

    void Start()
    {
        playerClosest = GameManager.Instance.GetPlayer(playerIndex);
        playerTransClosest = GameManager.Instance.GetPlayerTrans(playerIndex);

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
        playerIndex = GameManager.Instance.GetClosestPlayer(transform.position, out playerTransClosest);

        if (GetPlayerDistanceSquared() < (sightRange * sightRange))      //if the player is within sight of the enemy, enable agent, and give chase
        {
            playerClosest = GameManager.Instance.GetPlayer(playerIndex);
            playerTransClosest = GameManager.Instance.GetPlayerTrans(playerIndex);

            EnableAgent();
            anim.SetTrigger(playerInSight);

            canRotate = false;
            return;
        }
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
            EnableObstacle();
            return;
        }

        agent.destination = playerTransClosest.position;
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
        AudioManager.Instance.Play("RockElementalAttack");

        GameObject newLobbedAttack = Instantiate(lobbedAttack, throwSpawn.position, transform.rotation);
        newLobbedAttack.GetComponent<RangedAttackCollision>().InitDamage(stats.attack, 3);
        Vector3 target = playerTransClosest.position;

        Vector3 targetDir = target - throwSpawn.position; // get Target Direction
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
        AudioManager.Instance.Play("RockElementalAttack");
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
        walkingPoofs.SetActive(true);
    }

    void EnableObstacle()
    {
        agent.enabled = false;
        obstacle.enabled = true;
        walkingPoofs.SetActive(false);
    }

    void RotateTowardsPlayer()
    {
        // from https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html

        // Determine which direction to rotate towards
        Vector3 targetDirection = playerTransClosest.position - transform.position;
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
        return (playerTransClosest.position - transform.position).sqrMagnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeleeAttack")
        {
            AudioManager.Instance.Play("SpearHit");
            currentHealth -= playerClosest.stats.spearDamage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                AudioManager.Instance.Play("RockElementalDamaged");
            }
        }
        else if (other.tag == "SlingshotAttack")
        {
            AudioManager.Instance.Play("SlingHit");
            currentHealth -= playerClosest.stats.slingDamage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                AudioManager.Instance.Play("RockElementalDamaged");
            }
        }
    }

    IEnumerator Die()
    {
        print("Enemy is Dead and You Killed Them You Monster");
        AudioManager.Instance.Play("RockElementalDeath");
        if (aggro)
        {
            DeAggro();
        }
        isDead = true;

        EnableObstacle();
        modelHolder.gameObject.SetActive(false);
        anim.enabled = false;
        enabled = false;
        deathPoof.SetActive(true);

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
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
