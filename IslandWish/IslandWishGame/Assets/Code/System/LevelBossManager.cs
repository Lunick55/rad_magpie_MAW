using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelBossManager : LevelManager
{
	[Header("World Stuff")]
	[SerializeField] List<DoorScript> doors;
	[SerializeField] EnemyBehavior boss;

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

	private void Update()
	{

	}

	public override void ExitLevel()
	{
		if (CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count && boss.isDead)
		{
			//go to next level
			SceneLoader.Instance.AddSavedCoconuts(CoconutManager.Instance.coconutsFreed);
			SceneLoader.Instance.FinishLevel("EpilogueTest", postProcess);
			SceneLoader.Instance.LoadScene("Boat Scene");
		}

	}
}