using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingDrawState : SceneLinkedSMB<Player>
{
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex);

		m_MonoBehaviour.StartSlingshotAttack();
	}
}