using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconapperMelee : SceneLinkedSMB<CoconapperBehavior>
{
	public bool leftMelee = false;
	public bool rightMelee = false;

	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);
		m_MonoBehaviour.MeleeAttack(leftMelee, rightMelee);
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
		m_MonoBehaviour.FinishMeleeAttack();
	}
}
