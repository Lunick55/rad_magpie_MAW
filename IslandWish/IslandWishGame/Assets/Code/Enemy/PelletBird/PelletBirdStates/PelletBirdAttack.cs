﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PelletBirdAttack : SceneLinkedSMB<PelletBirdBehavior>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.RangedAttack();
	}
}
