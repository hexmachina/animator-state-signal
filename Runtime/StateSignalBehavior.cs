using UnityEngine;
using UnityEngine.Events;

namespace TW.StateSignal
{
	[System.Serializable]
	public class StateSignalEvent : UnityEvent<AnimatorStateSignal> { }

	[RequireComponent(typeof(Animator))]
	public class StateSignalBehavior : MonoBehaviour
	{
		public StateSignalEvent onStateSignalChanged = new StateSignalEvent();

		private void Awake()
		{
			var anim = GetComponent<Animator>();
			if (anim)
			{
				SceneLinkedSMB<StateSignalBehavior>.Initialize(anim, this);
			}
		}

		public void SignalChanged(AnimatorStateSignal signal)
		{
			onStateSignalChanged.Invoke(signal);
		}

	}
}

