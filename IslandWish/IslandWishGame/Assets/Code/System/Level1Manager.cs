using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1Manager : LevelManager
{
	[Header("World Stuff")]
	[SerializeField] Camera playerCam, cutsceneCam;
	[SerializeField] Transform cutscenePos;
	[SerializeField] GameObject fire;
	public bool newGame = false;
	[SerializeField] List<DoorScript> doors;
	[SerializeField] DoorScript beachDoor;
	[SerializeField] Canvas playerUI, talkUI;

	[Header("The Ghost Stuff")]
	[SerializeField] Firepit firepit;
	[SerializeField] GameObject ghost;
	[SerializeField] GameObject playerDoll;
	private bool runGhost;
	[SerializeField] TextMeshProUGUI text;
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
			GameManager.Instance.player.armed = false;
			GameManager.Instance.player.SheathWeapons();

			fire.SetActive(false);
		}
		else
		{
			GameManager.Instance.player.armed = true;
			GameManager.Instance.player.DrawWeapons();

			playerCam.enabled = true;
			cutsceneCam.enabled = false;

			fire.SetActive(true);
			beachDoor.OpenPath();
		}
		firepit.particles.SetActive(false);
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
		if (!beachDoor.IsLocked() && newGame)
		{
			firepit.particles.SetActive(true);
		}
	}

	public void ActivateGhostScene()
	{
		if (!beachDoor.IsLocked() && newGame)
		{
			firepit.particles.SetActive(true);

			ghost.SetActive(true);
			runGhost = true;

			playerCam.enabled = false;
			cutsceneCam.enabled = true;

			//teleport player and maybe activate my cutscene clone
			GameManager.Instance.playerMove.Teleport(cutscenePos.position);
			GameManager.Instance.player.canMove = false;
			GameManager.Instance.playerTrans.rotation = playerDoll.transform.rotation;
			fire.SetActive(true);
			playerUI.gameObject.SetActive(false);
			talkUI.gameObject.SetActive(true);
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
				EndGhostScene();
				return;
			}

			text.text = ghostTalk[ghostTalkIndex];
			AudioManager.Instance.Play(ghostAudioNames[ghostTalkIndex]);
		}
	}

	public void EndGhostScene()
	{
		ghost.SetActive(false);

		GameManager.Instance.player.canMove = true;
		GameManager.Instance.player.DrawWeapons();
		GameManager.Instance.player.armed = true;

		playerCam.enabled = true;
		cutsceneCam.enabled = false;

		//teleport player and maybe deactivate my cutscene clone
		playerUI.gameObject.SetActive(true);
		talkUI.gameObject.SetActive(false);

		beachDoor.OpenPath();
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
