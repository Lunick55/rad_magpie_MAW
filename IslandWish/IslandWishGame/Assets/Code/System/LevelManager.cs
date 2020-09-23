using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;
using TMPro;
using System;

public abstract class LevelManager : BaseSingleton<LevelManager>
{
	[Header("General Level Info")]
	[SerializeField] protected PostProcessVolume postProcess;
	[SerializeField] protected List<NarrationObject> narrationObjects;
	[SerializeField] protected List<DoorScript> doors;
	[SerializeField] protected List<KeyScript> keys;
	[SerializeField] protected NarrationObject startNarrObj;
	protected bool playerArmed = false;

	[Header("The Movie Stuff")]
	[SerializeField] Animator anim;
	[SerializeField] float transitionTime = 1f;
	[SerializeField] public Canvas narrationUI;
	[SerializeField] public TextMeshProUGUI text;

	public void LoadLevel()
	{
		LevelData data = SaveSystem.LoadLevel();

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

		//load the coconuts to match their saved versions
		CoconutSaveData cocoData = data.cocoData;
		for (int i = 0; i < cocoData.name.Length; i++)
		{
			CoconutData coconutLook = new CoconutData(cocoData.name[i], cocoData.accessoryID[i], cocoData.bodyID[i]);
			CoconutManager.Instance.coconuts[i].LoadCoconutLook(coconutLook);
			CoconutManager.Instance.coconuts[i].isSaved = data.coconutsSaved[i];
		}
	}
	public void SaveLevel()
	{
		SaveSystem.SaveLevel(this);
	}

	public abstract void ExitLevel();

	public IEnumerator MovieTransitionStart(UnityAction evnt)
	{
		anim.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		evnt.Invoke();
	}
	public IEnumerator MovieTransitionEnd(UnityAction evnt)
	{
		anim.SetTrigger("End");

		yield return new WaitForSeconds(transitionTime);

		evnt.Invoke();
	}

	//for later
	[Serializable]
	public class LevelData
	{
		public LevelData(LevelManager level)
		{
			//add the door lock/unlock status
			openDoors = new bool[level.doors.Count];
			for (int i = 0; i < openDoors.Length; i++)
			{
				openDoors[i] = level.doors[i].IsLocked();
			}

			//keys collected, and who owns it if it isn't used
			collectedKeys = new bool[level.keys.Count];
			collectedKeysID = new int[level.keys.Count];
			for (int i = 0; i < collectedKeys.Length; i++)
			{
				collectedKeys[i] = level.keys[i].isCollected;
				for (int j = 0; j < GameManager.Instance.GetPlayerCount(); j++)
				{
					//is it in an inventory? if not, the key is free
					if (GameManager.Instance.GetPlayer(j).hud.keys.Contains(level.keys[i]))
					{
						collectedKeysID[i] = j + 1;
					}
					else collectedKeysID[i] = 0;
				}
			}

			//TODO: door for now, then we do other stuff

			//get our narration statuses
			completeNarrations = new bool[level.narrationObjects.Count];
			for (int i = 0; i < completeNarrations.Length; i++)
			{
				completeNarrations[i] = level.narrationObjects[i].complete;
			}

			//get our enemy status
			enemiesDead = new bool[GameManager.Instance.enemies.Count];
			for (int i = 0; i < enemiesDead.Length; i++)
			{
				enemiesDead[i] = GameManager.Instance.enemies[i].isDead;
			}

			//check if the coconuts are saved or not
			coconutsSaved = new bool[CoconutManager.Instance.coconuts.Count];
			for (int i = 0; i < coconutsSaved.Length; i++)
			{
				coconutsSaved[i] = CoconutManager.Instance.coconuts[i].isSaved;
			}

			//if not work, I can just create the base arrays here and populate them
			List<CoconutData> coconutData = new List<CoconutData>();
			foreach (CoconutPetBehavior coconut in CoconutManager.Instance.coconuts)
			{
				CoconutData cocoData = new CoconutData(coconut.name, coconut.accessoryIndex, coconut.bodyIndex);
				coconutData.Add(cocoData);
			}

			cocoData = new CoconutSaveData(coconutData);
		}

		//Door data
		public bool[] openDoors;
		//Key Data
		public bool[] collectedKeys;
		public int[] collectedKeysID; // player ID that owns key. 0 = no one, 1 = P1, 2 = P2
									  //Narration Data
		public bool[] completeNarrations;
		//Enemy Data
		public bool[] enemiesDead;
		//Checkpoint Data
		public float[] checkpointPosition;
		//Coconut Data
		public bool[] coconutsSaved;
		public CoconutSaveData cocoData; //will this work???
	}
}

