using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoconutPetBehavior : MonoBehaviour
{
    public Transform cocoHolder;
    public GameObject body;
    [HideInInspector] public int bodyIndex;
    public GameObject accessory;
    [HideInInspector] public int accessoryIndex;
    //TODO: maybe also make an ID var to save, that correlates to the position of the child. this way I can just get rid of the cocoData object

    public ParticleSystem beamingParticle;
    [SerializeField] GameObject walkingPuffs;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    Transform playerTrans;
    Player player;
    [SerializeField] EnemyCage cage;
    public bool hide = false;

    Animator anim;

    [SerializeField] float wanderRange = 0, wanderHeight = 1, findRange = 1;
    private float maxRangeReciprical;
    public bool isWandering = false;
    //private float timeBetweenWander;

    private string playerInCombat = "PlayerInCombat", playerTooFar = "PlayerTooFar", wander = "Wander";

    Vector3 destination = Vector3.zero;
    private Vector3 debugDest = Vector3.zero;

    public bool displayMode = false;

    void Start()
    {
        if(displayMode)
		{
            //GetComponent<Animator>().enabled = false;
            cocoHolder.GetChild(0).gameObject.SetActive(false);
            body.SetActive(true);
            anim = body.GetComponent<Animator>();
            anim.enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
            enabled = false;
            return;
		}

        //create a LOOK kiddo
        int bodyOptions = cocoHolder.childCount;
        bodyIndex = Random.Range(0, bodyOptions);
        cocoHolder.GetChild(0).gameObject.SetActive(false);
        body = cocoHolder.GetChild(bodyIndex).gameObject;
        body.SetActive(true);
        anim = body.GetComponent<Animator>();
        

        player = GameManager.Instance.GetPlayer(0);
        playerTrans = GameManager.Instance.GetPlayerTrans(0);

        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        //agent.stoppingDistance = outerRange;
        EnableObstacle();
        SceneLinkedSMB<CoconutPetBehavior>.Initialise(anim, this);

        maxRangeReciprical = 1 / wanderRange;

        CoconutManager.Instance.AddCoconut(this);
    }

    public void Flee()
	{
        if (!player.inCombat)
        {
            if (GetPlayerDistanceSquared() > (wanderRange * wanderRange)) //if out of wanderRange follow the player directly
            {
                anim.SetTrigger(playerTooFar);
                isWandering = false;
                return;
            }
            if (GetPlayerDistanceSquared() < (wanderRange * wanderRange))                  //if within wanderRange, wander
            {
                anim.SetTrigger(wander);
                return;
            }
            else if ((cage && !cage.isBroken) || hide) //caged again, or commanded to hide
            {
                anim.SetTrigger("Hide");
                isWandering = false;
                EnableObstacle();
                return;
            }
        }
        else
        {
            if((transform.position - destination).magnitude < 1f)
			{
                anim.SetTrigger("Hide");
                isWandering = false;
                EnableObstacle();
                return;
            }
        }
    }

    public void Idle()
	{
        if (GetPlayerDistanceSquared() > (wanderRange * wanderRange)) //if out of wanderRange follow the player directly
        {
            anim.SetTrigger(playerTooFar);
            isWandering = false;
            return;
        }
        else if (player.inCombat)                   //if the player is in combat, flee
        {
            anim.SetTrigger(playerInCombat);
            isWandering = false;
            return;
        }
        else if ((cage && !cage.isBroken) || hide) //caged again, or commanded to hide
        {
            anim.SetTrigger("Hide");
            isWandering = false;
            EnableObstacle();
            return;
        }


    }

    public void Wander()
    {
        walkingPuffs.SetActive(true);
        if (GetPlayerDistanceSquared() > (wanderRange * wanderRange)) //if out of wanderRange follow the player directly
        {
            anim.SetTrigger(playerTooFar);
            isWandering = false;
            return;
        }
        else if (player.inCombat)                   //if the player is in combat, flee
        {
            anim.SetTrigger(playerInCombat);
            isWandering = false;
            return;
        }
        else if ((cage && !cage.isBroken) || hide) //caged again, or commanded to hide
		{
            anim.SetTrigger("Hide");
            EnableObstacle();
            isWandering = false;
            return;
		}

        if (!isWandering)
        {
            //TODO: fixed, but the navmesh is bonkers. Maybe implement safety timer?
            //Pick a point somwhere inside of a x-sized unit sphere
            Vector3 randomDestination = Random.insideUnitSphere;

            float randDestHeight = randomDestination.y * wanderHeight;
            randomDestination *= wanderRange;
            randomDestination.y = randDestHeight;
            //put that point in context of a position
            randomDestination += playerTrans.position;
            debugDest = randomDestination;

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
        walkingPuffs.SetActive(true);
        if (GetPlayerDistanceSquared() < (wanderRange * wanderRange))                  //if within wanderRange, wander
        {
            anim.SetTrigger(wander);
            return;
        }
        else if (player.inCombat)                   //if the player is in combat, hide
        {
            anim.SetTrigger(playerInCombat);
            return;
        }
        else if ((cage && !cage.isBroken) || hide) //caged again, or commanded to hide
        {
            anim.SetTrigger("Hide");
            EnableObstacle();
            return;
        }

        agent.destination = playerTrans.position;
    }

    public void FindHidingSpot()
	{
        destination = CoconutManager.Instance.GetClosestHidingSpot(transform).position;
        agent.destination = destination;
    }

    public void Hide()
    {
        if ((cage && !cage.isBroken) || hide || player.inCombat)
        {
            walkingPuffs.SetActive(false);
            EnableObstacle();

            if (hide && (GetPlayerDistanceSquared() < (findRange * findRange)))
            {
                hide = false;
            }

            return;
        }


        if (GetPlayerDistanceSquared() > (wanderRange * wanderRange)) //if out of wanderRange follow the player directly
        {
            anim.SetTrigger(playerTooFar);
            EnableAgent();
            CoconutManager.Instance.CoconutFreed(this);
            hide = false;
            return;
        }
        else if (GetPlayerDistanceSquared() < (wanderRange * wanderRange))                  //if within wanderRange, wander
        {
            anim.SetTrigger(wander);
            EnableAgent();
            CoconutManager.Instance.CoconutFreed(this);
            hide = false;
            return;
        }
        else if (player.inCombat)                   //if the player is in combat, flee
        {
            anim.SetTrigger(playerInCombat);
            EnableAgent();
            CoconutManager.Instance.CoconutFreed(this);
            hide = false;
            return;
        }

    }

    float GetPlayerDistanceSquared()
    {
        return (playerTrans.position - transform.position).sqrMagnitude;
    }

    void EnableAgent()
    {
        obstacle.enabled = false;
        agent.enabled = true;
    }

    void EnableObstacle()
    {
        walkingPuffs.SetActive(false);
        agent.enabled = false;
        obstacle.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && !displayMode)
        {
            //wander range
            UnityEditor.Handles.DrawWireDisc(playerTrans.position, Vector3.up, wanderRange);
            //find range
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, findRange);
            //upper wander range, to create cylinder
            Vector3 seekRange = playerTrans.position;
            seekRange.y += wanderHeight;
            UnityEditor.Handles.DrawWireDisc(seekRange, Vector3.up, wanderRange);

            UnityEditor.Handles.DrawWireDisc(destination, Vector3.up, 1);

            Gizmos.DrawWireSphere(debugDest, wanderHeight);
        }
#endif
    }
}
