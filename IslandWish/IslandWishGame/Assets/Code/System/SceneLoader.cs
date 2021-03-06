﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class SceneLoader : BasePersistentSingleton<SceneLoader>
{
	List<CoconutData> coconuts;
	bool isInit = false;
	public bool loadSingleData = false;
	public bool loadMultiData = false;
	private string nextLevel;
	public ProgressData progressData;
	private PostProcessVolume postProcess;
	public int playerCount;
	public void Init()
	{
		if(!isInit)
		{
			coconuts = new List<CoconutData>();

			progressData = new ProgressData(SceneManager.GetActiveScene().name, playerCount, false);

			isInit = true;
		}
	}

	public void StartGame(string scene, int newPlayerCount)
	{
		playerCount = newPlayerCount;
		LoadScene(scene);
	}
	public void LoadScene(string scene)
	{
		//TODO: turn off player movement n'stuff
		StartCoroutine(TransitionManager.Instance.TransitionToScene(scene));
	}
	public void FinishLevel(string nextLevelName, PostProcessVolume newPost)
	{
		nextLevel = nextLevelName;
		postProcess = newPost;
	}
	public void NextLevel()
	{
		LoadScene(nextLevel);
	}
	public PostProcessVolume GetPost()
	{
		return postProcess;
	}


	public void AddSavedCoconuts(List<CoconutPetBehavior> savedCoconuts)
	{
		foreach (CoconutPetBehavior coconut in savedCoconuts)
		{
			CoconutData cocoData = new CoconutData(coconut.name, coconut.accessoryIndex, coconut.bodyIndex);
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

	//the between scenes coconuts
	public void LoadPersistentCoconuts()
	{
		Reset();

		CoconutSaveData data = SaveSystem.LoadCoconuts();

		for (int i = 0; i < data.name.Length; i++)
		{
			CoconutData coconut = new CoconutData(data.name[i], data.accessoryID[i], data.bodyID[i]);

			coconuts.Add(coconut);
		}
	}

	public void LoadProgress()
	{
		if (SaveSystem.LoadProgress() != null)
		{
			progressData = SaveSystem.LoadProgress();
			playerCount = progressData.playerCount;
		}
	}
	public void SaveProgress()
	{
		progressData.currentLevelName = SceneManager.GetActiveScene().name;
		progressData.playerCount = playerCount;
		progressData.saveGame = true;
		SaveSystem.SaveProgress(progressData);
	}
}

[Serializable]
public class ProgressData
{
	public ProgressData(string newLevelName, int newPlayerCount, bool saveStatus)
	{
		currentLevelName = newLevelName;
		playerCount = newPlayerCount;
		saveGame = saveStatus;
	}

	public string currentLevelName;
	public int playerCount;
	public bool saveGame;
}