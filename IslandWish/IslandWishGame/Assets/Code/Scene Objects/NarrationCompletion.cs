using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationCompletion : NarrationObject
{
	protected override void OnTriggerEnter(Collider other)
	{
		if (CoconutManager.Instance.coconutsFreed.Count >= CoconutManager.Instance.coconuts.Count)
		{
			base.OnTriggerEnter(other);
		}
	}
}
