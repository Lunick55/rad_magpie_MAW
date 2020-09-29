using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseSelection;

    [SerializeField] GameObject playerSelection;
    [SerializeField] GameObject saveFileDetected;

    private bool music = true;
    int tempPlayerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
    }

    public void TogglePlayerSelect(bool toggle)
	{
        playerSelection.SetActive(toggle);
	}
    public void ToggleOverrideSelect(bool toggle)
	{
        saveFileDetected.SetActive(toggle);
	}

    public void TryStartGame(int playerCount)
    {
        SceneLoader.Instance.LoadProgress();
        tempPlayerCount = playerCount;
        if (SceneLoader.Instance.progressData.saveGame)
        {
            ToggleOverrideSelect(true);
        }
        else
        {
            StartGame(playerCount);
        }
    }
    public void StartGame(int playerCount)
	{
        if (playerCount == 0)
        {
            SceneLoader.Instance.StartGame("PrologueTest", tempPlayerCount);
        }
        else
		{
            SceneLoader.Instance.StartGame("PrologueTest", playerCount);
        }
    }

    public void NextLevel()
	{
        SceneLoader.Instance.NextLevel();
    }

    public void LoadSinglePlayerGame()
	{
        SceneLoader.Instance.LoadProgress();

        if(SceneLoader.Instance.progressData.saveGame)
		{
            SceneLoader.Instance.loadSingleData = true;
            SceneLoader.Instance.LoadScene(SceneLoader.Instance.progressData.currentLevelName);
        }
    }

    public void Pause()
	{
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        for(int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
            EventSystem.current.SetSelectedGameObject(pauseSelection);

            GameManager.Instance.GetPlayer(i).hud.gameObject.SetActive(false);
            GameManager.Instance.GetPlayer(i).SheathWeapons();
            GameManager.Instance.GetPlayer(i).canMove = false;
        }
    }
    public bool isPause()
	{
        return pauseMenu.activeSelf;
	}
    public void Resume()
	{
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
        {
            GameManager.Instance.GetPlayer(i).hud.gameObject.SetActive(true);
            if (GameManager.Instance.GetPlayer(i).armed)
            {
                GameManager.Instance.GetPlayer(i).DrawWeapons();
            }
            GameManager.Instance.GetPlayer(i).canMove = true;
        }
    }

    public void SaveAndQuit()
	{
        //save
        Time.timeScale = 1;
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
