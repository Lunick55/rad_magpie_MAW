using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotPellet : MonoBehaviour
{
	[SerializeField] float duration = 0;

	private float timer = 0;

	public void InitSlingshot(float newDuration)
	{
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
			Destroy(gameObject);
		}
	}
}
