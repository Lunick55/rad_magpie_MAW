using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableEnvironment : MonoBehaviour
{
	[SerializeField] GameObject breakable;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MeleeAttack" || other.tag == "Slingshot")
		{
			if (breakable != null)
			{
				AudioManager.Instance.Play("BreakObject");
				Destroy(breakable);
			}
		}
	}
}
