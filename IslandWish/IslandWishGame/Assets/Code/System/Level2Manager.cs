using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2Manager : LevelManager
{
	[Header("World Stuff")]
	[SerializeField] List<DoorScript> doors;
	[SerializeField] Canvas playerUI, talkUI;

	void Awake()
	{
		Init();
	}

	private void Init()
	{
		GameManager.Instance.player.armed = true;
		GameManager.Instance.player.DrawWeapons();
	}

	public override void LoadLevel()
	{
		//TODO: load the level

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
			SceneLoader.Instance.FinishLevel("Level 3");
			SceneLoader.Instance.LoadScene("Boat Scene");
		}

	}
}

//for later
[Serializable]
public class Level2Data
{
	public Level2Data(Level2Manager level)
	{

	}

	public bool newGame;
	public bool[] openDoors;
}
