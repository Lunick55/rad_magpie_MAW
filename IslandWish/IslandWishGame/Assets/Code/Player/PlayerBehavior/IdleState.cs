﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : SceneLinkedSMB<Player>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);
		m_MonoBehaviour.ResetAttack();
	}

	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
		m_MonoBehaviour.Idle();
	}

}