using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotPellet : MonoBehaviour
{
	[SerializeField] float duration = 0;

	private float timer = 0;

	public GameObject hitParticles;

	public void InitSlingshot(float newDuration, GameObject newHitParticle)
	{
		hitParticles = newHitParticle;
		duration = newDuration;
	}

	private void FixedUpdate()
	{
		timer += Time.deltaTime;
		if (timer > duration)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
		{
			if (other.tag == "Enemy")
			{
				GameObject newHitParticle = Instantiate(hitParticles);
				newHitParticle.transform.position = transform.position;
				newHitParticle.GetComponent<ParticleSystem>().Play();
			}
			Destroy(gameObject);
		}
	}
}
