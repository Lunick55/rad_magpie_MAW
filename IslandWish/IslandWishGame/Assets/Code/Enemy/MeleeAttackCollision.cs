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
			other.gameObject.GetComponent<Player>().TakeDamage(transform, 1);
		}
	}
}
