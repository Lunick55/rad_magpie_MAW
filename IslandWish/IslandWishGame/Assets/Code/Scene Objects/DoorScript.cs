using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    public int locks = 1;
	public List<KeyScript> keys;
	private bool isLocked = true;

	public UnityEvent evnt;

	[SerializeField] GameObject doorMapUI;

	private void Start()
	{
		foreach(KeyScript key in keys)
		{
			key.door = this;
		}

		if(evnt.GetPersistentEventCount() <= 0)
		{
			evnt.AddListener(OpenPath);
		}

		if(!isLocked)
		{
			locks = 0;
			gameObject.SetActive(false);
			if (doorMapUI)
			{
				doorMapUI.SetActive(false);
			}
		}
	}

	public void UnlockLock()
	{
		if(--locks <= 0)
		{
			isLocked = false;
			if (doorMapUI)
			{
				doorMapUI.SetActive(false);
			}
		}
	}

	public void OpenPath()
	{
		gameObject.SetActive(false);
		if (doorMapUI)
		{
			doorMapUI.SetActive(false);
		}
		AudioManager.Instance.Play("Door");
	}

	public void ConsumeKeys()
	{
		for (int i = 0; i < keys.Count; i++)
		{
			for (int j = 0; j < GameManager.Instance.GetPlayerCount(); j++)
			{
				if(GameManager.Instance.GetPlayer(j).hud.ConsumeKey(keys[i]))
				{
					UnlockLock();
					if (!isLocked)
					{
						evnt.Invoke();
					}
				}
			}
		}
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

	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			ConsumeKeys();
		}
	}
}
