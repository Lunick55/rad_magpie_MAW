﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoconapperBossBehavior : EnemyBehavior
{
    [SerializeField] GameObject walkingPuffs;
    [SerializeField] Collider[] hurtbox;
    [SerializeField] List<AudioClip> deathSounds;
    [SerializeField] AudioClip fightSong;
    [SerializeField] AudioClip levelSong;

    [SerializeField] float sightRange = 0, attackRange = 0;
    [SerializeField] float rangedAttackCooldown = 1f;
    private string playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle";

    private bool canRotate = false;

    [SerializeField] GameObject rangedAttackObject;

    void Start()
    {
        if (isDead)
        {
            gameObject.SetActive(false);
            return;
        }

        playerClosest = GameManager.Instance.GetPlayer(playerIndex);
        playerTransClosest = GameManager.Instance.GetPlayerTrans(playerIndex);

        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        SceneLinkedSMB<CoconapperBossBehavior>.Initialise(anim, this);

        currentHealth = stats.health;

        agent.stoppingDistance = attackRange;

        hurtbox[0].enabled = false;
        hurtbox[1].enabled = false;
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
            anim.SetBool(playerInSight, true);
            EnableAgent();
            AudioManager.Instance.Stop("MenuMusic");
            AudioManager.Instance.SetClip("MenuMusic", fightSong);
            AudioManager.Instance.Play("MenuMusic");
        }

        if (agent.enabled)
        {
            agent.destination = transform.position;
        }
    }

    public void ChasePlayer()
    {
        timer += Time.deltaTime;
        if(timer >= rangedAttackCooldown)
		{
            canRotate = false;
            timer = 0;

            anim.SetTrigger("Ranged");
            return;
        }

        //if player is within attack range, stop and attack
        if (GetPlayerDistanceSquared() < (attackRange * attackRange))
        {
            canRotate = true;
            EnableObstacle();

            //if (IsFacingPlayer())
            //{
                anim.SetTrigger(playerInRange);
            //}

        }
        //else if the player is out of sight, go back to idle
        else if (GetPlayerDistanceSquared() > (sightRange * sightRange))
        {
            canRotate = false;
            anim.SetBool(playerInSight, false);
            EnableObstacle();
            AudioManager.Instance.Stop("MenuMusic");
            AudioManager.Instance.SetClip("MenuMusic", levelSong);
            AudioManager.Instance.Play("MenuMusic");
        }
        else
        {
            EnableAgent();

            canRotate = false;
            agent.destination = playerTransClosest.position;
        }

    }

    public void AttackPlayer()
    {
        if (GetPlayerDistanceSquared() > (attackRange * attackRange))                                //player too far, chase
        {
            anim.SetBool(playerInRange, false);
            timer = 0;
            EnableAgent();
            canRotate = false;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= stats.timeBetweenAttacks && IsFacingPlayer())
        {
            timer = 0;
            canRotate = false;
            int randNum = Random.Range(0, 3);

            switch (randNum)
            {
                case 0:
                    anim.SetTrigger("AttackBoth");
                    AudioManager.Instance.Play("CoconapperDualAttack");
                    break;

                case 1:
                    anim.SetTrigger("AttackLeft");
                    AudioManager.Instance.Play("CoconapperAttack");
                    break;

                case 2:
                    anim.SetTrigger("AttackRight");
                    AudioManager.Instance.Play("CoconapperAttack");
                    break;

                default:
                    anim.SetTrigger("AttackBoth");
                    AudioManager.Instance.Play("CoconapperDualAttack");
                    break;
            }

            return;
        }
    }

    public void RangedAttackPlayer()
	{
        canRotate = false;
        EnableObstacle();

        //create sword, let that exist on it's own

        Vector3 spawnPos = playerTransClosest.position;
        spawnPos.y -= 0.62f;
        GameObject newRangedAttack = Instantiate(rangedAttackObject, spawnPos, playerTransClosest.rotation);
    }

    public void EndRangedAttack()
	{
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

    public void MeleeAttack(bool leftMelee, bool rightMelee)
    {
        hurtbox[0].enabled = leftMelee;
        hurtbox[1].enabled = rightMelee;
    }

    public void FinishMeleeAttack()
    {
        hurtbox[0].enabled = false;
        hurtbox[1].enabled = false;
        canRotate = true;
    }

    public void EnableAgent()
    {
        agent.enabled = true;
        obstacle.enabled = false;
        walkingPuffs.SetActive(true);
    }

    public void EnableObstacle()
    {
        agent.enabled = false;
        obstacle.enabled = true;
        walkingPuffs.SetActive(false);
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

    private bool IsFacingPlayer()
    {
        float minAngle = 15;

        Vector3 dirToPlayer = playerTransClosest.position - transform.position;

        dirToPlayer.y = 0;

        if (Vector3.Angle(transform.forward, dirToPlayer) < minAngle)
        {
            canRotate = false;
            return true;
        }

        return false;
    }

    float GetPlayerDistanceSquared()
    {
        return (playerTransClosest.position - transform.position).sqrMagnitude;
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
                    AudioManager.Instance.Play("CoconapperDamaged");
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
                    AudioManager.Instance.Play("CoconapperDamaged");
                    StartCoroutine(GameManager.Instance.GetPlayer(0).PauseHit(GameManager.Instance.GetPlayer(0).stats.pauseDuration));
                }
            }
        }
    }

    IEnumerator Die()
    {
        print("Enemy is Dead and You Killed Them You Monster");
        int randNum = Random.Range(0, deathSounds.Count);
        AudioManager.Instance.SetClip("BossDeath", deathSounds[randNum]);
        AudioManager.Instance.Play("BossDeath");
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
        
        AudioManager.Instance.Stop("MenuMusic");
        AudioManager.Instance.SetClip("MenuMusic", levelSong);
        AudioManager.Instance.Play("MenuMusic");

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
