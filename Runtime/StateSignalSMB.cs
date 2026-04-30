using UnityEngine;

namespace TW.StateSignal
{
	public class StateSignalSMB : SceneLinkedSMB<StateSignalBehavior>
	{
		public AnimatorStateSignal exitSignal;

		public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//base.OnSLStateExit(animator, stateInfo, layerIndex);
			if (!m_MonoBehaviour || !exitSignal)
				return;

			m_MonoBehaviour.SignalChanged(exitSignal);
		}
	}
}
