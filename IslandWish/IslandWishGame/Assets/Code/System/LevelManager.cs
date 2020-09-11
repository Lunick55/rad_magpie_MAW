using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public abstract class LevelManager : BaseSingleton<LevelManager>
{
	[SerializeField] protected PostProcessVolume postProcess;

	public abstract void LoadLevel();

	public abstract void ExitLevel();
}
