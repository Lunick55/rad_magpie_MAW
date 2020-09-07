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
    [SerializeField] public Animator anim;
    public HUDScript hud;
    [HideInInspector] public AttackLevel currentAttackLevel = AttackLevel.LEVEL0;
    [SerializeField] GameObject hurtBox;
    public bool canAttack = true;
    public bool armed = true;
    public List<GameObject> weapons;

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
    [SerializeField] public int slingCurrentAmmo = 1;
    [SerializeField] private Transform slingTrans;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        //EventManager.instance.AddListener(TakeDamage, EventTag.DAMAGE);
        //EventManager.instance.AddListener(HealDamage, EventTag.HEAL);
        EventManager.instance.AddListener(GoToCheckpoint, EventTag.FAILSTATE);

        SceneLinkedSMB<Player>.Initialise(anim, this);

        hurtBox.SetActive(false);

  //      if (SceneLoader.Instance.loadData)
  //      {
  //          SceneLoader.Instance.loadData = false;

  //          //load all the data calls needed here
  //          LoadPlayer();
  //      }
  //      else
		//{
  //          currentHealth = stats.health;
  //      }

        StartCoroutine(RegenShield());

        hud.InitLife();

        weapons[4].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
		{
            Debug.Log("ur dead bruh");
        }

        if (canAttack)
        {
            DrawWeapons();

            if (Input.GetMouseButtonDown(0))
            {
                if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
                {
                    AudioManager.Instance.Play("SwingSpear");
                    anim.SetTrigger("Attack");
                }
            }
            if (slingCurrentAmmo > 0)
            {

                if (Input.GetMouseButtonDown(1))
                {
                    AudioManager.Instance.Play("SlingshotPull");
                    anim.SetBool("Sling", true);
                    //maybe also add GetMouseButton() for the aim line
                    //draw the "aim" line
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    AudioManager.Instance.Play("SlingshotRelease");
                    anim.SetBool("Sling", false);
                    //FireSlingshotAttack();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !shieldBroken)
            {
                AudioManager.Instance.Play("ShieldReady");
                Block(true);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Block(false);
            }
        }
        else
		{
            SheathWeapons();
        }

        if (GameManager.Instance.GetCurrentAggro() > 0)
		{
            inCombat = true;
 
		}
        else
		{
            inCombat = false;
        }

    }

    public void TakeDamage(Transform damageSource, int damage)
	{
        if(blocking)
		{
            Vector3 damageDirection = damageSource.position - transform.position;

            float damageAngle = Vector3.Angle(transform.forward, damageDirection);
            if(damageAngle < 90)
			{
                damage = 10;
                Debug.Log("BLOCKED BITCH");
                AudioManager.Instance.Play("ShieldHit");
                shieldCurrentHealth -= damage;
                hud.UpdateShield(shieldCurrentHealth);
                return;
			}
		}
        print("OOF OUCH");
        AudioManager.Instance.Play("PCDamage");
        currentHealth -= damage;
        hud.LoseLife();
    }

    public void HealDamage(int heal)
	{
        hud.GainLife();
        if (currentHealth + heal > stats.health)
		{
            currentHealth = stats.health;
		}
        else
		{
            currentHealth += heal;
		}
	}

    public void FireSlingshotAttack()
	{
        if (slingCurrentAmmo > 0)
        {
            GameObject newSlingshotBullet = Instantiate(slingshotBullet, slingTrans.position, slingTrans.rotation);
            newSlingshotBullet.GetComponent<SlingshotPellet>().InitSlingshot(stats.slingDuration);

            newSlingshotBullet.GetComponent<Rigidbody>().velocity = transform.forward * stats.slingSpeed;
            slingCurrentAmmo--;
            hud.LoseSlingAmmo(); //just in case. remove UpdateSling if used
            hud.UpdateSlingAmmo(slingCurrentAmmo);
        }
    }

    public bool PickupSlingAmmo(int ammo)
	{
        //are we low on ammo?
        if(slingCurrentAmmo < stats.slingMaxAmmo)
		{
            //get ammo, and shave off any extra
            slingCurrentAmmo += ammo;
            if(slingCurrentAmmo > stats.slingMaxAmmo)
			{
                slingCurrentAmmo = stats.slingMaxAmmo;
			}
            hud.GainSlingAmmo(); //just in case. remove UpdateSling if used
            hud.UpdateSlingAmmo(slingCurrentAmmo);
            return true;
		}
        return false;
    }

    public void StartAttack()
    {
        hurtBox.SetActive(true);
        weapons[4].SetActive(true);

        if (currentAttackLevel < AttackLevel.MAX_LEVEL - 1)
        {
            currentAttackLevel++;
            print(currentAttackLevel);
        }
    }

    public void ResetAttack()
	{
        weapons[4].SetActive(false);

        currentAttackLevel = AttackLevel.LEVEL0;
        hurtBox.SetActive(false);
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

    //can move to Update if needed
    IEnumerator RegenShield()
	{
        while(true)
		{
            if (shieldCurrentHealth <= 0)
            {
                AudioManager.Instance.Play("ShieldBreak");
                hud.BreakShield();
                shieldBroken = true;
                Block(false);
            }

            if (shieldCurrentHealth < shieldMaxHealth)
            {
                shieldCurrentHealth += 1;
                hud.UpdateShield(shieldCurrentHealth);
                yield return new WaitForSeconds(shieldRechargeRate);
            }
            else
			{
                if(shieldCurrentHealth > shieldMaxHealth)
				{
                    shieldCurrentHealth = shieldMaxHealth;
				}
                hud.FixedShield();
                shieldBroken = false;
                yield return null;
			}
		}
	}

    public void DrawWeapons()
	{
        anim.SetBool("CanAttack", true);
        canAttack = true;
        weapons[0].SetActive(false);
        weapons[1].SetActive(false);
        weapons[2].SetActive(true);
        weapons[3].SetActive(true);
    }

    public void SheathWeapons()
	{
        anim.SetBool("CanAttack", false);
        canAttack = false;
        if (armed)
		{
            weapons[0].SetActive(true);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);
        }
        else
		{
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);
        }
    }

    public void LoadPlayer()
	{
        PlayerData data = SaveSystem.LoadPlayer();

        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        GetComponent<CharacterController>().enabled = true;

        currentHealth = data.health;
        slingCurrentAmmo = data.ammo;

    }

    void GoToCheckpoint(Event newFailstateEvent)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>().GetCheckpoint();
        GetComponent<CharacterController>().enabled = true;
        Debug.Log("Checkpoint is:" + GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>().GetCheckpoint());
        canMove = true;
    }
}

[System.Serializable]
public class PlayerData
{
    public PlayerData(Player player)
    {
        position = new float[3];
        Transform playerTrans = player.transform;

        position[0] = playerTrans.position.x;
        position[1] = playerTrans.position.y;
        position[2] = playerTrans.position.z;

        health = player.currentHealth;
        ammo = player.slingCurrentAmmo;
    }

    public float[] position;
    public int health;
    public int ammo;
}