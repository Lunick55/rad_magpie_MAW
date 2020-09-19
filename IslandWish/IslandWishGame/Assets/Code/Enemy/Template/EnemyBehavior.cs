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

    [SerializeField] protected Transform modelHolder;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject deathPoof;

    public EnemyStats stats;
    protected int currentHealth;
    protected bool aggro = false;
    protected float timer = 0;

    public bool isDead = false;
    private float prevAnimSpeed;

    public void Pause()
	{
        prevAnimSpeed = anim.speed;
        anim.speed = 0;
	}

    public void Resume()
	{
        anim.speed = prevAnimSpeed;
    }
}
