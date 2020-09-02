using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBoat : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			LevelManager.Instance.ExitLevel();
		}
	}
}
