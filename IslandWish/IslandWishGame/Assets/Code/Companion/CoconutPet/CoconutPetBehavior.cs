using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoconutPetBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    [SerializeField] Transform playerTrans;
    [SerializeField] Player player;

    Animator anim;

    [SerializeField] float wanderRange = 0, wanderHeight = 1;
    private float maxRangeReciprical;
    bool isWandering = false;
    //private float timeBetweenWander;

    private string playerInCombat = "PlayerInCombat", playerTooFar = "PlayerTooFar", wander = "Wander";

    //private bool canRotate = false;

    Vector3 destination = Vector3.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        //agent.stoppingDistance = outerRange;
        EnableAgent();
        SceneLinkedSMB<CoconutPetBehavior>.Initialise(anim, this);
        maxRangeReciprical = 1 / wanderRange;

        CoconutManager.Instance.AddCoconut(this);
    }

    // Update is called once per frame
    void Update()
    {
        //if (canRotate)
        //{
        //    RotateTowardsPlayer();
        //}
    }

    public void Wander()
    {
        float distanceToPlayer = (playerTrans.position - transform.position).magnitude;

        if (distanceToPlayer > wanderRange) //if out of wanderRange follow the player directly
        {
            anim.SetTrigger(playerTooFar);
            isWandering = false;
            return;
            //canRotate = true;
        }
        else if (player.inCombat)                   //if the player is in combat, hide
        {
            anim.SetTrigger(playerInCombat);
            isWandering = false;
            return;
            //canRotate = false;
        }

        if (!isWandering)
        {
            //Pick a point somwhere inside of a x-sized unit sphere
            Vector3 randomDestination = Random.insideUnitSphere * wanderRange;

            //put that point in context of a position
            randomDestination += playerTrans.position;
            float randDestHeight = randomDestination.y * wanderHeight * maxRangeReciprical;
            randomDestination.y = randDestHeight;

            NavMeshHit navHit;

            if (NavMesh.SamplePosition(randomDestination, out navHit, wanderHeight, NavMesh.AllAreas))
            {
                print("I got your orders baby");
                agent.destination = destination = navHit.position;
                isWandering = true;
            }
            else
            {
                print("Sorry, couldn't find a good place");
            }
        }
        else if (Vector3.Distance(destination, transform.position) <= agent.stoppingDistance)
		{
            isWandering = false;
        }
    }

    public void FollowPlayer()
    {
        print("Chase Player");

        float distanceToPlayer = (playerTrans.position - transform.position).magnitude;

        if (distanceToPlayer < wanderRange)                  //if within wanderRange, wander
        {
            anim.SetTrigger(wander);

            //canRotate = false;
        }
        else if (player.inCombat)                   //if the player is in combat, hide
        {
            anim.SetTrigger(playerInCombat);

            //canRotate = false;
        }
        else
        {
            agent.destination = playerTrans.position;
        }
    }

    public void FindHidingSpot()
	{
        destination = CoconutManager.Instance.GetClosestHidingSpot(transform).position;
	}
    public void Hide()
    {
        float distanceToPlayer = (playerTrans.position - transform.position).magnitude;

        if (!player.inCombat)
        {
            if (distanceToPlayer > wanderRange) //if out of wanderRange follow the player directly
            {
                anim.SetTrigger(playerTooFar);

                //canRotate = true;
            }
            else if (distanceToPlayer < wanderRange)                  //if within wanderRange, wander
            {
                anim.SetTrigger(wander);

                //canRotate = false;
            }
        }
        else
        {
            agent.destination = destination;
        }


        //pick a hiding place
        //then pathfind there
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
            //canRotate = false;
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

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, wanderRange);
        Vector3 seekRange = transform.position;
        seekRange.y += wanderHeight;
        UnityEditor.Handles.DrawWireDisc(seekRange, Vector3.up, wanderRange);

        UnityEditor.Handles.DrawWireDisc(destination, Vector3.up, 1);
    }
}
