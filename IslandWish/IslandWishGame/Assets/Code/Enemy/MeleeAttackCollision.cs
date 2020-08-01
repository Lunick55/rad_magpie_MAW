using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollision : MonoBehaviour
{
	[SerializeField] int damage = 0;

	public void InitDamage(int newDamage)
	{
		damage = newDamage;
	}

	private void FixedUpdate()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			print("HIT");

			//DamageEvent damageEvent = new DamageEvent(damage);
			//
			//EventManager.instance.FireEvent(damageEvent);
		}
	}
}
