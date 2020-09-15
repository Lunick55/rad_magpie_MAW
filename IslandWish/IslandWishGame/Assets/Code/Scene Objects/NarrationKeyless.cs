using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationKeyless : NarrationObject
{
	[SerializeField] DoorScript door;

	protected override void OnTriggerEnter(Collider other)
	{
		if (door.IsLocked())
		{
			base.OnTriggerEnter(other);
		}
	}
}
