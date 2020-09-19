using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;
using TMPro;
using System;

public abstract class LevelManager : BaseSingleton<LevelManager>
{
	[SerializeField] protected PostProcessVolume postProcess;
	[SerializeField] protected List<NarrationObject> narrationObjects;

	[Header("The Movie Stuff")]
	[SerializeField] Animator anim;
	[SerializeField] float transitionTime = 1f;
	[SerializeField] public Canvas narrationUI;
	[SerializeField] public TextMeshProUGUI text;

	public abstract void LoadLevel();
	public abstract void SaveLevel();

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
}

//for later
[Serializable]
public abstract class LevelData
{
	public bool[] openDoors;
	public bool[] collectedKeys;
	public int[] collectedKeysID; // player ID that owns key. 0 = no one, 1 = P1, 2 = P2
	public bool[] completeNarrations;
	public bool[] enemiesDead;
	public float[] checkpointPosition;
	public bool[] coconutsSaved;
}