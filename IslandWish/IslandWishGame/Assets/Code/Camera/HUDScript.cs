using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour
{

    public GameObject[] uiLives;
    public GameObject[] uiLifeBackgrounds;
    public GameObject spearImage;
    public GameObject slingshotImage;
    int playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 4;
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
        uiLives[playerHealth].SetActive(false);
        uiLifeBackgrounds[playerHealth].SetActive(true);
        playerHealth--;
    }

    public void GainLife()
    {
        
        playerHealth++;
        uiLives[playerHealth].SetActive(true);
        uiLifeBackgrounds[playerHealth].SetActive(false);
    }

    public void ToggleWeapon()
    {
        spearImage.SetActive(!spearImage.activeSelf);
        slingshotImage.SetActive(!slingshotImage.activeSelf);
    }
}