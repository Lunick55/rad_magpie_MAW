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
	[SerializeField] List<NarrationObject> narrationObjects;

	[Header("The Movie Stuff")]
	[SerializeField] Animator anim;
	[SerializeField] float transitionTime = 1f;
	[SerializeField] public Canvas playerUI, narrationUI;
	[SerializeField] public TextMeshProUGUI text;

	public abstract void LoadLevel();

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
	public LevelData(LevelManager level)
	{


	}

	public bool newGame;
	public bool[] openDoors;
	public bool[] completeNarrations;
}