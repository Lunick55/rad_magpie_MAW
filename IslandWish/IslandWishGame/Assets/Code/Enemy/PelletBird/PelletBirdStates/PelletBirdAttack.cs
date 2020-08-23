using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PelletBirdAttack : SceneLinkedSMB<PelletBirdBehavior>
{
	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

		m_MonoBehaviour.AttackPlayer();
	}
}
