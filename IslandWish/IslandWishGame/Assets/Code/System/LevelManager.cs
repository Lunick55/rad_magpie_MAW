using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManager : BaseSingleton<LevelManager>
{
	public abstract void LoadLevel();
}
