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
    [Header("Player Info")]
    public PlayerStats stats;
    [HideInInspector] public int currentHealth;
    [SerializeField] Animator anim;

    [HideInInspector] public AttackLevel currentAttackLevel = AttackLevel.LEVEL0;
    [SerializeField] GameObject hurtBox;

    [Header("Shield Stats")]
    [SerializeField] GameObject shield;
    bool blocking = false;
    bool shieldBroken = false;
    public int shieldMaxHealth = 100;
    public int shieldCurrentHealth;
    public float shieldRechargeRate;
    public bool inCombat = false;

    [Header("Slingshot")]
    [SerializeField] GameObject slingshotBullet;


    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(TakeDamage, EventTag.DAMAGE);

        SceneLinkedSMB<Player>.Initialise(anim, this);

        hurtBox.SetActive(false);

        currentHealth = stats.health;

        StartCoroutine(RegenShield());
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
		{
            Debug.Log("ur dead bruh");
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
            {
                GameManager.Instance.audioManager.Play("SwingSpear");
                anim.SetTrigger("Attack");
            }
        }
        if(Input.GetMouseButtonDown(1))
		{
            GameManager.Instance.audioManager.Play("SlingshotPull");
        }
        else if(Input.GetMouseButtonUp(1))
		{
            GameManager.Instance.audioManager.Play("SlingshotRelease");
            FireSlingshotAttack();
		}

        if(Input.GetKeyDown(KeyCode.LeftControl) && !shieldBroken)
		{
            GameManager.Instance.audioManager.Play("ShieldReady");
            Block(true);
		}
        if(Input.GetKeyUp(KeyCode.LeftControl))
		{
            Block(false);
		}

        if(GameManager.Instance.GetCurrentAggro() > 0)
		{
            inCombat = true;
		}
        else
		{
            inCombat = false;
		}
    }

    public void TakeDamage(Event newDamageEvent)
	{
        DamageEvent damageEvent = (DamageEvent)newDamageEvent;

        if(blocking)
		{
            Vector3 damageDirection = damageEvent.position - transform.position;

            float damageAngle = Vector3.Angle(transform.forward, damageDirection);
            if(damageAngle < 90)
			{
                Debug.Log("BLOCKED BITCH");
                GameManager.Instance.audioManager.Play("ShieldHit");
                shieldCurrentHealth -= damageEvent.damage;
                return;
			}
		}
        print("OOF OUCH");
        GameManager.Instance.audioManager.Play("PCDamage");
        currentHealth -= damageEvent.damage;
    }

    public void FireSlingshotAttack()
	{
        GameObject newSlingshotBullet = Instantiate(slingshotBullet, transform.position, transform.rotation);
        newSlingshotBullet.GetComponent<SlingshotPellet>().InitSlingshot(stats.slingDuration);

        newSlingshotBullet.GetComponent<Rigidbody>().velocity = transform.forward * stats.slingSpeed;
    }

    public void StartAttack()
    {
        hurtBox.SetActive(true);

        if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
        {
            currentAttackLevel++;
            print(currentAttackLevel);
        }
    }

    public void EndAttack()
	{
        hurtBox.SetActive(false);
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

    public void Block(bool isBlock)
	{
        if (isBlock)
        {
            shield.SetActive(true);
            blocking = true;
            //other stuff i dunno
        }
        else
		{
            shield.SetActive(false);
            blocking = false;
		}
	}

    IEnumerator RegenShield()
	{
        while(true)
		{
            if (shieldCurrentHealth <= 0)
            {
                GameManager.Instance.audioManager.Play("ShieldBreak");
                shieldBroken = true;
                Block(false);
            }

            if (shieldCurrentHealth < shieldMaxHealth)
            {
                shieldCurrentHealth += 1;

                yield return new WaitForSeconds(shieldRechargeRate);
            }
            else
			{
                if(shieldCurrentHealth > shieldMaxHealth)
				{
                    shieldCurrentHealth = shieldMaxHealth;
				}
                shieldBroken = false;
                yield return null;
			}
		}
	}
}
