using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockElementalAttack2 : SceneLinkedSMB<RockElementalBehavior>
{
	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.SmashAttack();
	}
}
