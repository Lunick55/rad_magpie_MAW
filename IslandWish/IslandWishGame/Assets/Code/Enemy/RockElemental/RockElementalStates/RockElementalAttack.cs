using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockElementalAttack : SceneLinkedSMB<RockElementalBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.RangedAttack();
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);

		m_MonoBehaviour.EndAttack();
	}
}
