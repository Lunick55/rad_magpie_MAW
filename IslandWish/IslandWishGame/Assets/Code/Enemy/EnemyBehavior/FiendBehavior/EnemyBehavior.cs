using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    [SerializeField] Transform player;

    [SerializeField] GameObject hitbox, hurtbox;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.destination = player.position;

        SceneLinkedSMB<EnemyBehavior>.Initialise(anim, this);

        //hurtbox.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void UpdatePlayerPosition()
    {
        agent.destination = player.position;
    }

    public void CheckAction()
    {
        if ((player.position - transform.position).sqrMagnitude < Mathf.Pow(agent.stoppingDistance, 2))
        {
            anim.SetTrigger("Attack");
            obstacle.enabled = true; 
            agent.enabled = false;
        }
        else
		{
            obstacle.enabled = false;
            agent.enabled = true;
            //anim.ResetTrigger("Attack");
            UpdatePlayerPosition();
        }
    }

    public void StartChase()
	{
        agent.isStopped = false;
	}

    public void StopChase()
	{
        agent.isStopped = true;
    }

    public void MeleeAttack()
    {
        //attack logic?
    }
}
