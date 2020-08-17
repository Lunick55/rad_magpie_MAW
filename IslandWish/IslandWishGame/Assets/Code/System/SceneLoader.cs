using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : BasePersistentSingleton<SceneLoader>
{
	List<CoconutData> coconuts;
	bool isInit = false;

	public void Init()
	{
		if(!isInit)
		{
			coconuts = new List<CoconutData>();
			isInit = true;
		}
	}

	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public void AddSavedCoconuts(List<CoconutPetBehavior> savedCoconuts)
	{
		foreach (CoconutPetBehavior coconut in savedCoconuts)
		{
			CoconutData cocoData = new CoconutData(coconut.name, coconut.accessory);
			coconuts.Add(cocoData);
		}
	}

	public List<CoconutData> GetSavedCoconuts()
	{
		return coconuts;
	}

	public void Reset()
	{
		coconuts.Clear();
		coconuts.Capacity = 0;

	}
}
