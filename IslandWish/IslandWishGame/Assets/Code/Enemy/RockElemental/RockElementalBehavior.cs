using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockElementalBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    Player player;
    Transform playerTrans;
    [SerializeField] GameObject hitbox, lobbedAttack, smashAttack;

    Animator anim;

    [SerializeField] float sightRange = 0, outerRange = 0, innerRange = 0, lobbedAngle = 45;
    private string playerTooClose = "PlayerTooClose", playerInSight = "PlayerInSight", playerInRange = "PlayerInRange", idle = "Idle", smash = "Smash";

    public EnemyStats stats;
    public float timeBetweenAttacks, timer;
    private int currentHealth;
    private bool canRotate = false;
    private bool aggro = false;

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
        if (timer >= timeBetweenAttacks)
        {
            LobbedAttack();
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
        if (timer >= timeBetweenAttacks)
        {
            timer = 0;
            anim.SetTrigger(smash);
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
        print("ATTACK");
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

    public void StartSmash()
    {
        Debug.Log("FIST OF HAVOK");
        smashAttack.SetActive(true);
    }

    public void EndSmash()
    {
        smashAttack.SetActive(false);
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
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sightRange);
		UnityEditor.Handles.color = Color.yellow;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, outerRange);
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, innerRange);
	}
}
