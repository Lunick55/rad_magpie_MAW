using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBirdShootReference : MonoBehaviour
{
	/*This script exists so that I can call an animation event 
	 *on the animator that references a script attached to the 
	 *parent object.
	*/
	[SerializeField] PelletBirdBehavior pelletBird;

	public void Shoot()
	{
		pelletBird.RangedAttack();
	}
}
