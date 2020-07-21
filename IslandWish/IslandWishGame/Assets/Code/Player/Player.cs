using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackLevel
{
    LEVEL0 = -1,
    LEVEL1,
    LEVEL2,
    LEVEL3,
    MAX_LEVEL
}

public class Player : MonoBehaviour
{
    public PlayerStats stats;
    [SerializeField] Animator anim;

    [HideInInspector]
    public AttackLevel currentAttackLevel = AttackLevel.LEVEL0;

    [SerializeField] GameObject hurtBox;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(TakeDamage, EventTag.DAMAGE);

        SceneLinkedSMB<Player>.Initialise(anim, this);

        hurtBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.health <= 0)
		{
            Debug.Log("ur dead bruh");
		}

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    public void TakeDamage(Event newDamageEvent)
	{
        DamageEvent damageEvent = (DamageEvent)newDamageEvent;

        stats.health -= damageEvent.damage;
	}

    public void StartAttack()
    {
        hurtBox.SetActive(true);
        canMove = false;

        if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
        {
            currentAttackLevel++;
            print(currentAttackLevel);
        }
    }

    public void EndAttack()
	{
        hurtBox.SetActive(false);
        canMove = true;

    }

    public void ResetAttack()
	{
        currentAttackLevel = AttackLevel.LEVEL0;
	}

    public void Idle()
	{

	}

    public void Moving()
	{

	}
}
