using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            other.gameObject.GetComponent<Player>().HealDamage(1);
            Destroy(gameObject);
		}
	}
}
