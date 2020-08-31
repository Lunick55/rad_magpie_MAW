using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int locks = 1;
	public List<KeyScript> keys;
	private bool isLocked = true;

	private void Start()
	{
		foreach(KeyScript key in keys)
		{
			key.door = this;
		}
	}

	public void UnlockLock()
	{
		if(--locks <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	public void OpenPath()
	{
		gameObject.SetActive(false);
	}

	public bool IsLocked()
	{
		if (locks <= 0)
		{
			isLocked = false;
		}
		else
		{
			isLocked = true;
		}

		return isLocked;
	}

	public void SetLocked(bool locked)
	{
		isLocked = locked;
	}
}
