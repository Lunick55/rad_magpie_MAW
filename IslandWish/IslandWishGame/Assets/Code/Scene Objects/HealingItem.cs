using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            if(other.gameObject.GetComponent<Player>().currentHealth < other.gameObject.GetComponent<Player>().stats.health)
			{
				other.gameObject.GetComponent<Player>().HealDamage(1);
				AudioManager.Instance.Play("Pickup");
				Destroy(gameObject);
			}
		}
	}
}
