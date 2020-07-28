using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBirdAttack : SceneLinkedSMB<BunnyBirdBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);
		m_MonoBehaviour.MeleeAttack();
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
		m_MonoBehaviour.FinishMeleeAttack();
	}
}

