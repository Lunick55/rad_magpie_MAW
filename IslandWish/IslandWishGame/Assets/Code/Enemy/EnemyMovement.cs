using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    [SerializeField] Transform player;

    [SerializeField] GameObject hitbox, hurtbox;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        agent.stoppingDistance = 5;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePlayerPosition();

    }

    public void UpdatePlayerPosition()
	{
        agent.destination = player.position;
    }

    void MeleeAttack()
	{

	}
}
