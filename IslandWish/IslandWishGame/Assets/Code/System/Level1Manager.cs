using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Level1Manager : LevelManager
{
	public bool levelSkip = false;

	[Header("World Stuff")]
	[SerializeField] GameObject fire;
	[SerializeField] GameObject fireParticles;
	public bool newGame = false;
	[SerializeField] List<DoorScript> doors;
	[SerializeField] List<KeyScript> keys;
	[SerializeField] NarrationObject startNarrObj;

	[Header("The Ghost Stuff")]
	[SerializeField] GameObject ghost;
	private bool runGhost;
	[SerializeField] List<string> ghostTalk;
	[SerializeField] List<AudioClip> ghostAudio;
	[SerializeField] AudioClip ghostMusic;
	[SerializeField] AudioClip normalMusic;
	private int ghostTalkIndex = 0;
	public int ghostAppear, ghostDisappear;

	void Awake()
	{
		Init();
	}

	private void Init()
	{
		if(newGame)
		{
			for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
			{
				GameManager.Instance.GetPlayer(i).armed = false;
				GameManager.Instance.GetPlayer(i).SheathWeapons();
			}

			startNarrObj.complete = true;
			fire.SetActive(false);
		}
		else
		{
			for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
			{
				GameManager.Instance.GetPlayer(i).armed = true;
				GameManager.Instance.GetPlayer(i).DrawWeapons();
			}

			fire.SetActive(true);
		}
		fireParticles.SetActive(false);
	}

	public override void LoadLevel()
	{
		//TODO: load the level
		Level1Data data = SaveSystem.LoadLevel1();

		newGame = data.newGame;

		//unlock the right doors
		for (int i = 0; i < data.openDoors.Length; i++)
		{
			doors[i].SetLocked(data.openDoors[i]);
		}

		//collect the right keys
		for (int i = 0; i < data.collectedKeys.Length; i++)
		{
			keys[i].isCollected = data.collectedKeys[i];

			if (data.collectedKeysID[i] == 1)
			{
				keys[i].CollectKey(GameManager.Instance.GetPlayer(data.collectedKeysID[i] - 1));
			}
			else if (data.collectedKeysID[i] == 2)
			{
				keys[i].CollectKey(GameManager.Instance.GetPlayer(data.collectedKeysID[i] - 1));
			}
		}

		for (int i = 0; i < data.completeNarrations.Length; i++)
		{
			narrationObjects[i].complete = data.completeNarrations[i];
		}

		for (int i = 0; i < data.enemiesDead.Length; i++)
		{
			GameManager.Instance.enemies[i].isDead = data.enemiesDead[i];
		}

		for (int i = 0; i < data.coconutsSaved.Length; i++)
		{
			CoconutManager.Instance.coconuts[i].isSaved = data.coconutsSaved[i];
		}

	}

	public override void SaveLevel()
	{
		SaveSystem.SaveLevel1(this);
	}

	private void Update()
	{
		if(runGhost)
		{
			RunGhostScene();
		}
		if (newGame)
		{
			fireParticles.SetActive(true);
		}
	}

	public void StartMovie()
	{
		fireParticles.SetActive(true);

		for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
			GameManager.Instance.GetPlayer(i).canMove = false;
		}

		GameManager.Instance.DisableHud();

		StartCoroutine(MovieTransitionStart(ActivateGhostScene));
	}

	public void ActivateGhostScene()
	{
		if (newGame)
		{
			runGhost = true;

			fire.SetActive(true);

			narrationUI.gameObject.SetActive(true);
			text.text = ghostTalk[ghostTalkIndex];
			AudioManager.Instance.Stop("MenuMusic");
			AudioManager.Instance.SetClip("MenuMusic", ghostMusic);
			AudioManager.Instance.Play("MenuMusic");
			AudioManager.Instance.SetClip("Narration", ghostAudio[ghostTalkIndex]);
			AudioManager.Instance.Play("Narration");

			newGame = false;
		}
	}

	public void RunGhostScene()
	{
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
		{
			AudioManager.Instance.Stop("Narration");
			if (++ghostTalkIndex >= ghostTalk.Count)
			{
				runGhost = false;
				narrationUI.gameObject.SetActive(false);

				for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
				{
					GameManager.Instance.GetPlayer(i).canMove = true;
					GameManager.Instance.GetPlayer(i).DrawWeapons();
					GameManager.Instance.GetPlayer(i).armed = true;
				}

				StartCoroutine(MovieTransitionEnd(EndGhostScene));
				//EndGhostScene();
				return;
			}

			if(ghostTalkIndex == ghostAppear)
			{
				ghost.SetActive(true);
				ghost.transform.position = GameManager.Instance.GetPlayerTrans(0).position;
				ghost.transform.position += GameManager.Instance.GetPlayerTrans(0).forward.normalized * 3;

				ghost.transform.LookAt(GameManager.Instance.GetPlayerTrans(0));
			}
			else if(ghostTalkIndex == ghostDisappear)
			{
				ghost.SetActive(false);
			}

			text.text = ghostTalk[ghostTalkIndex];
			AudioManager.Instance.SetClip("Narration", ghostAudio[ghostTalkIndex]);
			AudioManager.Instance.Play("Narration");
		}
	}

	public void EndGhostScene()
	{
		GameManager.Instance.EnableHud();

		AudioManager.Instance.Stop("MenuMusic");
		AudioManager.Instance.SetClip("MenuMusic", normalMusic);
		AudioManager.Instance.Play("MenuMusic");
	}

	public override void ExitLevel()
	{
		if ((CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count) || levelSkip)
		{
			//go to next level
			SceneLoader.Instance.AddSavedCoconuts(CoconutManager.Instance.coconutsFreed);
			SceneLoader.Instance.FinishLevel("Level 2", postProcess);
			SceneLoader.Instance.LoadScene("Boat Scene");
		}

	}

	//for later
	[Serializable]
	public class Level1Data : LevelData
	{
		public Level1Data(Level1Manager level)
		{
			newGame = level.newGame;

			openDoors = new bool[level.doors.Count];
			for (int i = 0; i < openDoors.Length; i++)
			{
				openDoors[i] = level.doors[i].IsLocked();
			}

			collectedKeys = new bool[level.keys.Count];
			collectedKeysID = new int[level.keys.Count];
			for (int i = 0; i < collectedKeys.Length; i++)
			{
				collectedKeys[i] = level.keys[i].isCollected;
				for(int j = 0; j < GameManager.Instance.GetPlayerCount(); j++)
				{
					if(GameManager.Instance.GetPlayer(j).hud.keys.Contains(level.keys[i]))
					{
						collectedKeysID[i] = j + 1;
					}
				}
			}

			completeNarrations = new bool[level.narrationObjects.Count];
			for (int i = 0; i < completeNarrations.Length; i++)
			{
				completeNarrations[i] = level.narrationObjects[i].complete;
			}

			enemiesDead = new bool[GameManager.Instance.enemies.Count];
			for (int i = 0; i < enemiesDead.Length; i++)
			{
				enemiesDead[i] = GameManager.Instance.enemies[i].isDead;
			}

			coconutsSaved = new bool[CoconutManager.Instance.coconuts.Count];
			for (int i = 0; i < coconutsSaved.Length; i++)
			{
				coconutsSaved[i] = CoconutManager.Instance.coconuts[i].isSaved;
			}

		}

		public bool newGame;
	}
}
//public bool[] openDoors;
//public bool[] collectedKeys;
//public int[] collectedKeysID; // for the inventory, if you've got any keys on you
//public bool[] completeNarrations;
//public bool[] enemiesDead;
//public float[] checkpointPosition;
//public bool[] coconutsSaved;

