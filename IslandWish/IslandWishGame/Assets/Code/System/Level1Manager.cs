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

	[Header("The Ghost Stuff")]
	[SerializeField] GameObject ghost;
	private bool runGhost;
	[SerializeField] List<string> ghostTalk;
	[SerializeField] List<AudioClip> ghostAudio;
	[SerializeField] AudioClip ghostMusic;
	[SerializeField] AudioClip normalMusic;
	private int ghostTalkIndex = 0;

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

		throw new NotImplementedException();
	}

	public override void SaveLevel()
	{


		throw new NotImplementedException();
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
			ghost.SetActive(true);
			ghost.transform.position = GameManager.Instance.GetPlayerTrans(0).position;
			ghost.transform.position += GameManager.Instance.GetPlayerTrans(0).forward.normalized * 2;
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
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
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

			text.text = ghostTalk[ghostTalkIndex];
			AudioManager.Instance.SetClip("Narration", ghostAudio[ghostTalkIndex]);
			AudioManager.Instance.Play("Narration");
		}
	}

	public void EndGhostScene()
	{
		ghost.SetActive(false);

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

		}

		public bool newGame;
	}
}


