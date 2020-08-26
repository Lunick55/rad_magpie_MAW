using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int locks = 1;
	public bool beachDoor = false; //temp thing, might restructure later

	private void Start()
	{
		EventManager.instance.AddListener(OpenPath, EventTag.BEACH_LOG);
	}

	public void UnlockLock()
	{
		if(--locks <= 0)
		{
			if(beachDoor)
			{
				Level1Manager.Instance.ActivateGhostScene();
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void OpenPath(Event evnt)
	{
		gameObject.SetActive(false);
	}
}
