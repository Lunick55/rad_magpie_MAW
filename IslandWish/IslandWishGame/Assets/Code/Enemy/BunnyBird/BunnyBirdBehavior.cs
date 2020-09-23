using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BunnyBirdBehavior : EnemyBehavior
{
    [SerializeField] GameObject hurtbox;
    public ParticleSystem featherPoofParticles;

    [SerializeField] float sightRange = 0, attackRange = 0;
    [SerializeField] float attackHeight, attackBuffer;
    private string playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";
    private Transform referenceTrans;

    private bool canRotate = false;
    private Vector3 preHuntPos = Vector3.zero;
    [SerializeField] int restTimer = 0;
    void Start()
    {
        if(isDead)
		{
            gameObject.SetActive(false);
            return;
		}

        playerClosest = GameManager.Instance.GetPlayer(playerIndex);
        playerTransClosest = GameManager.Instance.GetPlayerTrans(playerIndex);

        if (GetComponent<NavMeshAgent>())
        {
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
        }
        else
		{
            agent = transform.parent.GetComponent<NavMeshAgent>();
            obstacle = transform.parent.GetComponent<NavMeshObstacle>();
        }

        referenceTrans = agent.transform;

        SceneLinkedSMB<BunnyBirdBehavior>.Initialise(anim, this);

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

    public void Idle()
    {
        playerIndex = GameManager.Instance.GetClosestPlayer(transform.position, out playerTransClosest);

        //if the player is within sight of the enemy, enable agent, and give chase
        if (GetPlayerDistanceSquared() < (sightRange * sightRange))
        {
            playerClosest = GameManager.Instance.GetPlayer(playerIndex);
            playerTransClosest = GameManager.Instance.GetPlayerTrans(playerIndex);

            anim.SetTrigger(playerInSight);
            EnableAgent();
            return;
        }
    }

    public void ChasePlayer()
    {
        //if player is within attack range, stop and attack
        if (GetPlayerDistanceSquared() <= (attackRange * attackRange))
        {
            canRotate = true;
            EnableObstacle();

            anim.SetTrigger(playerInRange);

            return;
        }
        //else if the player is out of sight, go back to idle
        else if (GetPlayerDistanceSquared() > (sightRange * sightRange))
        {
            canRotate = false;
            anim.SetTrigger(idle);
            EnableObstacle();

            return;
        }

        canRotate = false;
        agent.destination = playerTransClosest.position;
    }

    public void Attack()
    {
        if (GetPlayerDistanceSquared() > (attackRange * attackRange))                                //player too far, chase
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
            timer = 0;
            if (IsFacingPlayer())
            {
                anim.SetTrigger("Swipe");
            }
        }
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

    public void MeleeAttack()
    {
        preHuntPos = modelHolder.position;

        //get the vector in the direction of the player
        Vector3 targetVector = playerTransClosest.position - modelHolder.position;
        //make it flat, I don't care about y-axis, so I won't include it
        targetVector.y = modelHolder.position.y;

        //gimme the buffer. 
        targetVector -= (targetVector.normalized * attackBuffer);

        //I don't want the bird going backwards, so I uh, send them straight down. Will it work? maybe.
        if (targetVector.sqrMagnitude < (attackBuffer * attackBuffer))
        {
            Debug.Log("TOO CLOSE");
            targetVector = modelHolder.position;
        }

        //put it back in relation to itself
        targetVector += modelHolder.position;

        //set my height up
        targetVector.y = (modelHolder.position.y - attackHeight);

        StartCoroutine(LerpToPos(targetVector, 0.15f));
        hurtbox.SetActive(true);
        AudioManager.Instance.Play("BunnyBirdAttack");
    }

    public void RestMeleeAttack()
	{
        timer += Time.deltaTime;
        if(timer >= restTimer)
		{
            timer = 0;
            StartCoroutine(LerpToPos(preHuntPos, 0.15f));
            anim.SetTrigger("EndRest");
		}
    }

    public void FinishMeleeAttack()
	{
        hurtbox.SetActive(false);
	}

    public void EnableAgent()
    {
        obstacle.enabled = false;
        agent.enabled = true;
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

    IEnumerator LerpToPos(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = modelHolder.position;

        while (time < duration)
        {
            modelHolder.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        modelHolder.position = targetPosition;
    }

    private bool IsFacingPlayer()
	{
        float minAngle = 15;

        Vector3 dirToPlayer = playerTransClosest.position - transform.position;

        dirToPlayer.y = 0;

        if(Vector3.Angle(transform.forward, dirToPlayer) < minAngle)
		{
            canRotate = false;
            return true;
		}

        return false;
	}

    float GetPlayerDistanceSquared()
    {
        return (playerTransClosest.position - referenceTrans.position).sqrMagnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
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
                    AudioManager.Instance.Play("BunnyBirdDamaged");
                    featherPoofParticles.Play();
                    StartCoroutine(GameManager.Instance.GetPlayer(0).PauseHit(GameManager.Instance.GetPlayer(0).stats.pauseDuration));
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
                    AudioManager.Instance.Play("BunnyBirdDamaged");
                    featherPoofParticles.Play();
                    StartCoroutine(GameManager.Instance.GetPlayer(0).PauseHit(GameManager.Instance.GetPlayer(0).stats.pauseDuration));
                }
            }
        }
    }

    IEnumerator Die()
    {
        print("Enemy is Dead and You Killed Them You Monster");
        AudioManager.Instance.Play("BunnyBirdDeath");
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
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
#endif
    }
}
