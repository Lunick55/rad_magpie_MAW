using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject uiHud;

    [SerializeField] GameObject playerSelection;

    private bool music = true;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
    }

    public void OpenPlayerSelect()
	{
        playerSelection.SetActive(true);
	}
    public void ClosePlayerSelect()
	{
        playerSelection.SetActive(false);
    }
    public void StartSingleGame(string scene)
	{
        SceneLoader.Instance.StartGame(scene, 1);
	}
    public void StartMultiGame(string scene)
	{
        SceneLoader.Instance.StartGame(scene, 2);
	}

    public void NextLevel()
	{
        SceneLoader.Instance.NextLevel();
    }

    public void LoadSinglePlayerGame(string scene)
	{
        SceneLoader.Instance.loadSingleData = true;
        SceneLoader.Instance.LoadScene(scene);
    }
    public void LoadMultiPlayerGame(string scene)
    {
        SceneLoader.Instance.loadMultiData = true;
        SceneLoader.Instance.LoadScene(scene);
    }


    public void Pause()
	{
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        uiHud.SetActive(false);

        for(int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
            GameManager.Instance.GetPlayer(0).SheathWeapons();
            GameManager.Instance.GetPlayer(0).canMove = false;
        }
    }

    public void Resume()
	{
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        uiHud.SetActive(true);
        for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
        {
            GameManager.Instance.GetPlayer(0).DrawWeapons();
            GameManager.Instance.GetPlayer(0).canMove = true;
        }
    }

    public void SaveAndQuit()
	{
        //save
        SceneLoader.Instance.LoadScene("Menu");
    }

    public void QuitGame()
	{
        Application.Quit();
	}

    public void ResetGame()
	{
        SceneLoader.Instance.Reset();
        SceneLoader.Instance.LoadScene("Menu");
	}

    public void ToggleMusic()
	{
        music = !music;
        if (music)
        {
            AudioManager.Instance.Play("MenuMusic");
        }
        else
        {
            AudioManager.Instance.Stop("MenuMusic");
        }
	}
}
