using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelBossManager : LevelManager
{
	[Header("World Stuff")]
	[SerializeField] List<DoorScript> doors;

	void Awake()
	{
		Init();
	}

	private void Init()
	{
		for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
			GameManager.Instance.GetPlayer(i).armed = true;
			GameManager.Instance.GetPlayer(i).DrawWeapons();
		}
	}

	public override void LoadLevel()
	{
		//TODO: load the level

		throw new NotImplementedException();
	}

	public override void SaveLevel()
	{


		throw new NotImplementedException();
	}

	private void Update()
	{

	}

	public override void ExitLevel()
	{
		if (CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count)
		{
			//go to next level
			SceneLoader.Instance.AddSavedCoconuts(CoconutManager.Instance.coconutsFreed);
			SceneLoader.Instance.FinishLevel("EpilogueTest", postProcess);
			SceneLoader.Instance.LoadScene("Boat Scene");
		}

	}
}

//for later
[Serializable]
public class LevelBossData
{
	public LevelBossData(LevelBossManager level)
	{

	}

	public bool newGame;
	public bool[] openDoors;
}
