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

    public Animation anim;

    [Header("Player UI")]
    //you can change the type to be whatever you want (text object, game object, image?), however you see fit to display the info. 
    //I can set the logic up to better match your vision once you've decided on how the final thing should look.act
    public TextMeshProUGUI uiSlingerAmmo;
    [SerializeField] public GameObject saveUI;

    [Header("Collectible Stuff")]
    public List<Image> keyImages;
    public List<KeyScript> keys;

    public ParticleSystem hooray;
    public GameObject boatCommand;

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
        GameManager.Instance.GetPlayer(playerIndex).GoToCheckpoint();

        foreach (GameObject lifeImage in uiLives)
        {
            lifeImage.SetActive(true);
        }

        foreach(GameObject lifeBackground in uiLifeBackgrounds)
        {
            lifeBackground.SetActive(false);

        }

        GameManager.Instance.GetPlayer(playerIndex).currentHealth = GameManager.Instance.GetPlayer(playerIndex).stats.health;
    }
}