using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{

    public GameObject[] uiLives;
    public GameObject[] uiLifeBackgrounds;
    public GameObject spearImage;
    public GameObject slingshotImage;
    public int playerHealth;
    public int playerHealthMax = 5;

    GameObject checkpointManager;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(IncomingDamage, EventTag.DAMAGE);
        EventManager.instance.AddListener(HealDamage, EventTag.HEAL);
        EventManager.instance.AddListener(SetbackPlayer, EventTag.FAILSTATE);

        checkpointManager = GameObject.Find("CheckpointManager");
        playerHealth = playerHealthMax;
    }

    void IncomingDamage(Event newDamageEvent)
    {
        DamageEvent damageEvent = (DamageEvent)newDamageEvent;
        //Debug.Log("Damage taken: " + damageEvent.damage);
        //for(int i = 0; i < damageEvent.damage; i++)
        {
            LoseLife();
        }
        
    }

    void HealDamage(Event newHealEvent)
	{
        GainLife();
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            GainLife();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LoseLife();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleWeapon();
        }
    }

    public void LoseLife()
    {
        if (playerHealth > 0)
        {
            playerHealth--;
            uiLives[playerHealth].SetActive(false);
            uiLifeBackgrounds[playerHealth].SetActive(true);
        }        

        if(playerHealth <= 0)
        {
            GameManager.Instance.player.canMove = false;
            GameManager.Instance.audioManager.Play("PCDeath");
            anim.Play();
            Invoke("FailState", 2);

        }
    }

    public void GainLife()
    {
        if (playerHealth < playerHealthMax)
        {
            uiLives[playerHealth].SetActive(true);
            uiLifeBackgrounds[playerHealth].SetActive(false);
            playerHealth++;
        }
    }

    public void ToggleWeapon()
    {
        spearImage.SetActive(!spearImage.activeSelf);
        slingshotImage.SetActive(!slingshotImage.activeSelf);
    }

    void FailState()
    {
        FailstateEvent failstateEvent = new FailstateEvent();
        EventManager.instance.FireEvent(failstateEvent);
        foreach(GameObject lifeImage in uiLives)
        {
            lifeImage.SetActive(true);
        }

        foreach(GameObject lifeBackground in uiLifeBackgrounds)
        {
            lifeBackground.SetActive(false);

        }

        playerHealth = playerHealthMax;
    }

    void SetbackPlayer(Event newFailstateEvent)
    {

        Debug.Log("Player is dead!");
        FailstateEvent failstateEvent = (FailstateEvent)newFailstateEvent;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //GameManager.Instance.player.canMove = false;
        //GameManager.Instance.audioManager.Play("PCDeath");
        //Invoke("ResetScene", 2);
    }

    void ResetScene()
	{
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

}