using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockElementalChase : SceneLinkedSMB<RockElementalBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

		m_MonoBehaviour.ChasePlayer();
	}
}
