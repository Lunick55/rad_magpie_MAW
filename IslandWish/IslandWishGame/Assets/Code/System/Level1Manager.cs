using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Level1Manager : LevelManager
{
	[Header("World Stuff")]
	[SerializeField] GameObject fire;
	[SerializeField] GameObject fireParticles;
	public bool newGame = false;
	[SerializeField] List<DoorScript> doors;

	[Header("The Ghost Stuff")]
	[SerializeField] GameObject ghost;
	private bool runGhost;
	[SerializeField] List<string> ghostTalk;
	[SerializeField] List<string> ghostAudioNames;
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

		playerUI.gameObject.SetActive(false);

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
			AudioManager.Instance.Play(ghostAudioNames[ghostTalkIndex]);

			newGame = false;
		}
	}

	public void RunGhostScene()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			AudioManager.Instance.Stop(ghostAudioNames[ghostTalkIndex]);
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
			AudioManager.Instance.Play(ghostAudioNames[ghostTalkIndex]);
		}
	}

	public void EndGhostScene()
	{
		ghost.SetActive(false);


		playerUI.gameObject.SetActive(true);

	}

	public override void ExitLevel()
	{
		if (CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count)
		{
			//go to next level
			SceneLoader.Instance.AddSavedCoconuts(CoconutManager.Instance.coconutsFreed);
			SceneLoader.Instance.FinishLevel("Level 2", postProcess);
			SceneLoader.Instance.LoadScene("Boat Scene");
		}

	}
}

//for later
[Serializable]
public class Level1Data
{
	public Level1Data(Level1Manager level)
	{
		newGame = level.newGame;

		
	}

	public bool newGame;
	public bool[] openDoors;
}
