using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: maybe turn this into a scriptable object or extendable class
public class EnemyAttackCollision : MonoBehaviour
{
	[SerializeField] int damage = 0;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			print("HIT");

			DamageEvent damageEvent = new DamageEvent(damage);

			EventManager.instance.FireEvent(damageEvent);
		}
	}
}
