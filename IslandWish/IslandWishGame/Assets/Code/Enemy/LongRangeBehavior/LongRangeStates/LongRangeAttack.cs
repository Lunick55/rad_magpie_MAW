using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LongRangeAttack : SceneLinkedSMB<LongRangeBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex, controller);

		m_MonoBehaviour.RangedAttack();
	}
}
