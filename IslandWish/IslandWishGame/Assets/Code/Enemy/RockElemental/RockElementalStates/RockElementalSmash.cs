using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockElementalSmash : SceneLinkedSMB<RockElementalBehavior>
{
	public bool leftSmash = false;
	public bool rightSmash = false;

	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.StartSmash(leftSmash, rightSmash);
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.EndSmash();
	}
}
