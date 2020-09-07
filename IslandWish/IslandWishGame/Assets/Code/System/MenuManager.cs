using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject uiHud;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
    }

    public void StartGame(string scene)
	{
        SceneLoader.Instance.LoadScene(scene);
	}

    public void NextLevel()
	{
        SceneLoader.Instance.NextLevel();
    }

    public void LoadGame(string scene)
	{
        SceneLoader.Instance.loadData = true;
        SceneLoader.Instance.LoadScene(scene);
    }

    public void Pause()
	{
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        uiHud.SetActive(false);
        GameManager.Instance.player.canAttack = false;
        GameManager.Instance.player.canMove = false;
	}

    public void Resume()
	{
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        uiHud.SetActive(true);
        GameManager.Instance.player.canAttack = true;
        GameManager.Instance.player.canMove = true;
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
}
