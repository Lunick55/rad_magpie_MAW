using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected NavMeshObstacle obstacle;

    protected Player player;
    protected Transform playerTrans;

    [SerializeField] protected Animator anim;

    public EnemyStats stats;
    protected int currentHealth;
    protected bool aggro = false;
    protected float timer = 0;

    public bool isDead = false;
}
