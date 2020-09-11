using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : MonoBehaviour
{
	[SerializeField] private Level1Manager lvl1Man;
	[SerializeField] public GameObject particles;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			lvl1Man.ActivateGhostScene();
		}
	}

}
