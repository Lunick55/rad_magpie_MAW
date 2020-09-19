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
			other.gameObject.GetComponent<Player>().PickupSlingAmmo(ammo);
			AudioManager.Instance.Play("AmmoPickup");
			//maybe put on a hidden timer?
			Destroy(gameObject);
		}
	}
}
