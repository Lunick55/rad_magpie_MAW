using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(string scene)
	{
        SceneLoader.Instance.LoadScene(scene);
	}

    public void LoadGame(string scene)
	{
        SceneLoader.Instance.loadData = true;
        SceneLoader.Instance.LoadScene(scene);
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
