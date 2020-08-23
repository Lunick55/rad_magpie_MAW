﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBirdChase : SceneLinkedSMB<BunnyBirdBehavior>
{
	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
		m_MonoBehaviour.ChasePlayer();
	}
}
