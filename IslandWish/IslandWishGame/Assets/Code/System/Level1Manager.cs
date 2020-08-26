using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1Manager : BaseSingleton<Level1Manager>
{
	[SerializeField] GameObject ghost, playerDoll;
	[SerializeField] Camera playerCam, cutsceneCam;
	[SerializeField] Transform cutscenePos;
	[SerializeField] GameObject fire;
	public bool ghostSceneComplete = false;
	private bool runGhost;
	[SerializeField] Canvas playerUI, talkUI;
	[SerializeField] TextMeshProUGUI text;
	[SerializeField] List<string> ghostTalk;
	private int ghostTalkIndex = 0;

	void Awake()
	{
		Init();
	}

	private void Init()
	{
		if(!ghostSceneComplete)
		{
			GameManager.Instance.player.canAttack = false;
			GameManager.Instance.player.armed = false;
			fire.SetActive(false);
		}
		else
		{
			GameManager.Instance.player.canAttack = true;
			GameManager.Instance.player.armed = true;

			playerCam.enabled = true;
			cutsceneCam.enabled = false;

			fire.SetActive(true);
		}
	}

	private void Update()
	{
		if(runGhost)
		{
			RunGhostScene();
		}
	}

	public void ActivateGhostScene()
	{
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
	}

	public void RunGhostScene()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{			
			if(++ghostTalkIndex >= ghostTalk.Count)
			{
				runGhost = false;
				EndGhostScene();
				return;
			}

			text.text = ghostTalk[ghostTalkIndex];
		}
	}

	public void EndGhostScene()
	{
		ghost.SetActive(false);
		GameManager.Instance.player.canAttack = true;
		GameManager.Instance.player.armed = true;

		playerCam.enabled = true;
		cutsceneCam.enabled = false;

		//teleport player and maybe deactivate my cutscene clone
		GameManager.Instance.player.canMove = true;
		playerUI.gameObject.SetActive(true);
		talkUI.gameObject.SetActive(false);

		BeachEvent beach = new BeachEvent();
		EventManager.instance.FireEvent(beach);
	}
}

//for later
[Serializable]
public class Level1Data
{
	public Level1Data()
	{

	}

	public bool ghostComplete;
	public bool[] openDoors;


}
