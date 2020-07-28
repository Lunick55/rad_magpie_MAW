using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: maybe turn this into a scriptable object or extendable class
public class RangedAttackCollision : MonoBehaviour
{
	[SerializeField] int damage = 0;
	[SerializeField] float duration = 0;

	private float timer = 0;

	public void InitDamage(int newDamage, float newDuration)
	{
		damage = newDamage;
		duration = newDuration;
	}

	private void FixedUpdate()
	{
		timer += Time.deltaTime;
		if(timer > duration)
		{
			Destroy(gameObject);
		}
	}

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
