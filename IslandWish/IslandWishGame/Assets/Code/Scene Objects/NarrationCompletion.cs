using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationCompletion : NarrationObject
{
	public void CocosFound()
	{
		if (!complete)
		{
			StartMovie();
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		//does nothing
	}
}
