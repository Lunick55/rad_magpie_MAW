using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : BasePersistentSingleton<SceneLoader>
{
	List<string> coconutNames; //for now. gameobjects to instantiate later
	bool isInit = false;

	public void Init()
	{
		if(!isInit)
		{
			coconutNames = new List<string>();
			isInit = true;
		}
	}

	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public void AddSavedCoconuts(List<GameObject> coconuts)
	{
		foreach (GameObject coconut in coconuts)
		{
			coconutNames.Add(coconut.name);
		}
	}

	public List<string> GetSavedCoconuts()
	{
		return coconutNames;
	}

	public void Reset()
	{
		coconutNames.Clear();
		coconutNames.Capacity = 0;

	}
}
