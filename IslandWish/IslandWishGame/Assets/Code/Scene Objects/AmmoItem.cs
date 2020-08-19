using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    public int ammo = 1;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			GameManager.Instance.player.PickupSlingAmmo(ammo);
			//maybe put on a hidden timer?
			Destroy(gameObject);
		}
	}
}
