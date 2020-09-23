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
		newGame = !GameManager.Instance.GetPlayer(0).armed;
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
}