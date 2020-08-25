﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : BasePersistentSingleton<SceneLoader>
{
	List<CoconutData> coconuts;
	bool isInit = false;
	public bool loadData = false;

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

	public string GetCurrentLevelName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public void Reset()
	{
		coconuts.Clear();
		coconuts.Capacity = 0;

	}

	public void LoadCoconuts()
	{
		CoconutSaveData data = SaveSystem.LoadCoconuts();

		for (int i = 0; i < data.name.Length; i++)
		{
			coconuts[i].name = data.name[i];
			coconuts[i].isSaved = data.isSaved[i];
			coconuts[i].accessory = CoconutManager.Instance.cocoAttach.GetAccessoryFromID(data.accessoryID[i]);
		}
	}
}
