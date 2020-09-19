using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCollision : MonoBehaviour
{
    public GameObject hitParticles;

	private void OnCollisionEnter(Collision collision)
	{
		print("SPEAR HIT");
		if (collision.gameObject.tag == "Enemy")
		{
			GameObject newHitParticle = Instantiate(hitParticles);
			newHitParticle.transform.position = collision.GetContact(0).point;
			newHitParticle.GetComponent<ParticleSystem>().Play();
		}
	}
}
