using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    [SerializeField] int playerIndex = 0;
    public GameObject[] uiLives;
    public GameObject[] uiLifeBackgrounds;

    GameObject checkpointManager;
    public Animation anim;

    [Header("Replace These Petra")]
    //you can change the type to be whatever you want (text object, game object, image?), however you see fit to display the info. 
    //I can set the logic up to better match your vision once you've decided on how the final thing should look.act
    public List<Image> uiCoconutSad;
    public List<Image> uiCoconutHappy;
    public List<Image> uiCoconutAll;
    public TextMeshProUGUI uiSlingerAmmo;
    public TextMeshProUGUI uiShieldHealth;

    [Header("Collectible Stuff")]
    public List<Image> keyImages;
    public List<KeyScript> keys;

    // Start is called before the first frame update
    void Start()
    {
        //EventManager.instance.AddListener(IncomingDamage, EventTag.DAMAGE);
        //EventManager.instance.AddListener(HealDamage, EventTag.HEAL);
        EventManager.instance.AddListener(SetbackPlayer, EventTag.FAILSTATE);

        checkpointManager = GameObject.Find("CheckpointManager");
    }

    //can become a full InitUI function as things get added and need to be saved
    public void InitLife()
	{
        int playerHealth = GameManager.Instance.GetPlayer(playerIndex).currentHealth;
        int playerMaxHealth = GameManager.Instance.GetPlayer(playerIndex).stats.health;

        if (playerHealth < playerMaxHealth)
		{
            for(int i = playerMaxHealth; i > playerHealth; i--)
			{
                uiLives[i-1].SetActive(false);
                uiLifeBackgrounds[i-1].SetActive(true);
            }
        }
    }

    //health functions
    public void LoseLife()
    {
        int playerHealth = GameManager.Instance.GetPlayer(playerIndex).currentHealth;

        if (playerHealth > 0)
        {
            uiLives[playerHealth].SetActive(false);
            uiLifeBackgrounds[playerHealth].SetActive(true);
        }        

        if(playerHealth <= 0)
        {
            GameManager.Instance.GetPlayer(playerIndex).canMove = false;
            AudioManager.Instance.Play("PCDeath");
            anim.Play();
            Invoke("FailState", 2);

        }
    }

    public void GainLife()
    {
        int playerHealth = GameManager.Instance.GetPlayer(playerIndex).currentHealth;

        if (playerHealth < GameManager.Instance.GetPlayer(playerIndex).stats.health)
        {
            uiLives[playerHealth].SetActive(true);
            uiLifeBackgrounds[playerHealth].SetActive(false);
        }
    }

    //Coconut functions
    public void GainCoconut()
	{
        //if something more complex happens than just changing a text field
	}

    public void LoseCoconut()
	{
        //if something more complex happens than just changing a text field
    }

    public void UpdateCoconut(int updatedCoconutsRescued)
	{
        if (updatedCoconutsRescued < uiCoconutAll.Count)
        {

            int i = 0;
            for (; i < updatedCoconutsRescued; i++)
            {
                uiCoconutHappy[i].enabled = true;
                uiCoconutSad[i].enabled = false;
                uiCoconutAll[i].enabled = false;
            }
            for (; i < uiCoconutSad.Count; i++)
            {
                uiCoconutHappy[i].enabled = false;
                uiCoconutSad[i].enabled = true;
                uiCoconutAll[i].enabled = false;
            }
        }
        else
		{
            for (int i = 0; i < updatedCoconutsRescued; i++)
            {
                uiCoconutHappy[i].enabled = false;
                uiCoconutSad[i].enabled = false;
                uiCoconutAll[i].enabled = true;
            }
        }
    }

    //Slinger functions
    public void GainSlingAmmo()
	{
        //if something more complex happens than just changing a text field
    }

    public void LoseSlingAmmo()
	{
        //if something more complex happens than just changing a text field
    }

    public void UpdateSlingAmmo(int playerAmmo)
	{
        uiSlingerAmmo.text = "" + playerAmmo;
	}

    //Shield Functions(s)? //might be both an add/remove shield or just an updateShield
    public void UpdateShield(int playerShield)
	{
        uiShieldHealth.text = "Shield: " + playerShield;
	}
    public void FixedShield()
	{
        uiShieldHealth.text = "Shield: " + 100;

        //indicate fixed shield somehow
    }
    public void BreakShield()
	{
        //indicate broken shield somehow
	}

    public void AddKey(KeyScript key)
	{
        if(!keys.Contains(key))
		{
            keys.Add(key);
            int index = keys.IndexOf(key);
            keyImages[index].sprite = key.sprite;
            keyImages[index].enabled = true;
        }

    }

    //CONSUME PRYLOSEC
    public bool ConsumeKey(KeyScript key)
	{
        if (keys.Contains(key))
        {
            int index = keys.IndexOf(key);
            keyImages[index].sprite = null;
            keyImages[index].enabled = false;
            keys.Remove(key);

            for(int i = index + 1; i < keyImages.Count; i++)
			{
                keyImages[i-1].sprite = keyImages[i].sprite;
                keyImages[i].sprite = null;
                keyImages[i].enabled = false;
            }

            return true;
        }

        return false;
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

        GameManager.Instance.GetPlayer(playerIndex).currentHealth = GameManager.Instance.GetPlayer(playerIndex).stats.health;
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