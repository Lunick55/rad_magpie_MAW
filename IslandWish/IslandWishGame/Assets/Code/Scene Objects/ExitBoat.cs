using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBoat : MonoBehaviour
{
	[SerializeField] NarrationCompletion narrCompObj;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (narrCompObj)
			{
				if (CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count)
				{
					narrCompObj.CocosFound();
				}
			}
			else
			{
				LevelManager.Instance.ExitLevel();
			}
		}
	}
}
