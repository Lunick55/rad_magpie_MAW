using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("In Game References")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseSelection;

    [Header("Main Menu Popups")]
    [SerializeField] GameObject playerPicker;
    [SerializeField] GameObject saveFileDetected;
    [SerializeField] GameObject saveFileDeletion;

    [Header("Main Menu Selections")]
    [SerializeField] GameObject startSelection;
    [SerializeField] GameObject playerSelection;
    [SerializeField] GameObject overrideSelection;
    [SerializeField] GameObject deleteSaveSelection;

    private EventSystem evSys
	{
		get
		{
            return EventSystem.current;
		}
	}

    private bool music = true;
    int tempPlayerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();

        if (startSelection)
        {
            evSys.SetSelectedGameObject(startSelection);
        }
    }

    public void TogglePlayerSelect(bool toggle)
	{
        playerPicker.SetActive(toggle);
        if(toggle)
		{
            evSys.SetSelectedGameObject(playerSelection);
        }
        else
		{
            evSys.SetSelectedGameObject(startSelection);
        }
    }
    public void ToggleOverrideSelect(bool toggle)
	{
        saveFileDetected.SetActive(toggle);
        if (toggle)
        {
            evSys.SetSelectedGameObject(overrideSelection);
        }
        else
		{
            evSys.SetSelectedGameObject(playerSelection);
        }
    }    
    public void ToggleDeleteSelect(bool toggle)
	{
        saveFileDeletion.SetActive(toggle);
        if (toggle)
        {
            evSys.SetSelectedGameObject(deleteSaveSelection);
        }
        else
		{
            evSys.SetSelectedGameObject(startSelection);
        }
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
    public void LoadScene(string scene)
	{
        SceneLoader.Instance.LoadScene(scene);
    }
    public void TryDeleteSavedData()
	{
        SceneLoader.Instance.LoadProgress();

        if (SceneLoader.Instance.progressData.saveGame)
        {
            ToggleDeleteSelect(true);
        }
	}
    public void DeleteSavedData()
	{
        SaveSystem.DeleteAllSavedData();
        ToggleDeleteSelect(false);
    }

    public void Pause()
	{
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        for(int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
            evSys.SetSelectedGameObject(pauseSelection);

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
