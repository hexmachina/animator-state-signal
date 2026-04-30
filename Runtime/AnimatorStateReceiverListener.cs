using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TW.StateSignal
{
	public class AnimatorStateReceiverListener : MonoBehaviour
	{
		private AnimatorStateSignalReceiver receiver;

		[SerializeField] private List<AnimatorStateSignal> signals = new List<AnimatorStateSignal>();
		[SerializeField] private List<UnityEvent> events = new List<UnityEvent>();


		public void GetReceiverByComponent(Component component)
		{
			if (!component)
				return;

			if (component.TryGetComponent(out AnimatorStateSignalReceiver stateSignalReceiver))
			{
				if (receiver && stateSignalReceiver != receiver)
				{
					receiver.onNotify -= OnReceiverNotify;
				}
				receiver = stateSignalReceiver;
				receiver.onNotify += OnReceiverNotify;
			}
		}

		public void ClearReceiver()
		{
			if (receiver)
			{
				receiver.onNotify -= OnReceiverNotify;
			}
			receiver = null;
		}

		private void OnReceiverNotify(AnimatorStateSignal obj)
		{
			var index = signals.IndexOf(obj);
			if (index > -1 && index < events.Count)
			{
				events[index].Invoke();
			}
		}
	}
}

