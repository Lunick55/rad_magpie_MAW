using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocoBossRanged : SceneLinkedSMB<CoconapperBossBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);
		m_MonoBehaviour.RangedAttackPlayer();
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
		m_MonoBehaviour.EndRangedAttack();
	}
}
