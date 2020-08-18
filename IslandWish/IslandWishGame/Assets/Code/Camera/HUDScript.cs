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

    GameObject checkpointManager;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        //EventManager.instance.AddListener(IncomingDamage, EventTag.DAMAGE);
        //EventManager.instance.AddListener(HealDamage, EventTag.HEAL);
        EventManager.instance.AddListener(SetbackPlayer, EventTag.FAILSTATE);

        checkpointManager = GameObject.Find("CheckpointManager");
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

    public void InitLife()
	{
        int playerHealth = GameManager.Instance.player.currentHealth;
        int playerMaxHealth = GameManager.Instance.player.stats.health;

        if (playerHealth < playerMaxHealth)
		{
            for(int i = playerMaxHealth; i > playerHealth; i--)
			{
                uiLives[i-1].SetActive(false);
                uiLifeBackgrounds[i-1].SetActive(true);
            }
        }
    }

    public void LoseLife()
    {
        int playerHealth = GameManager.Instance.player.currentHealth;

        if (playerHealth > 0)
        {
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
        int playerHealth = GameManager.Instance.player.currentHealth;

        if (playerHealth < GameManager.Instance.player.stats.health)
        {
            uiLives[playerHealth].SetActive(true);
            uiLifeBackgrounds[playerHealth].SetActive(false);
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

        GameManager.Instance.player.currentHealth = GameManager.Instance.player.stats.health;
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