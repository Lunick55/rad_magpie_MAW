using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutPetHideState : SceneLinkedSMB<CoconutPetBehavior>
{
	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
		m_MonoBehaviour.Hide();
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
		m_MonoBehaviour.beamingParticle.Play();
	}
}
