using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected NavMeshObstacle obstacle;

    protected Player playerClosest;
    protected Transform playerTransClosest;
    protected int playerIndex = 0;

    [SerializeField] protected Animator anim;

    public EnemyStats stats;
    protected int currentHealth;
    protected bool aggro = false;
    protected float timer = 0;

    public bool isDead = false;
}
