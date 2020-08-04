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

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(IncomingDamage, EventTag.DAMAGE);
        EventManager.instance.AddListener(SetbackPlayer, EventTag.FAILSTATE);

        playerHealth = 5;
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
        playerHealth--;
        uiLives[playerHealth].SetActive(false);
        uiLifeBackgrounds[playerHealth].SetActive(true);
        

        if(playerHealth <= 0)
        {
            FailState();
        }
    }

    public void GainLife()
    {
        uiLives[playerHealth].SetActive(true);
        uiLifeBackgrounds[playerHealth].SetActive(false);
        playerHealth++;
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
    }

    void SetbackPlayer(Event newFailstateEvent)
    {

        Debug.Log("Player is dead!");
        FailstateEvent failstateEvent = (FailstateEvent)newFailstateEvent;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.player.canMove = false;
        GameManager.Instance.audioManager.Play("PCDeath");
        Invoke("ResetScene", 2);
    }

    void ResetScene()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}