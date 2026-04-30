using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TW.StateSignal
{

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class AnimatorStateSignalReceiver : MonoBehaviour
	{
		[SerializeField]
		EventKeyValue m_Events = new EventKeyValue();

		public event Action<AnimatorStateSignal> onNotify;

		private void Awake()
		{
			var anim = GetComponent<Animator>();
			if (anim)
			{
				SceneLinkedSMB<AnimatorStateSignalReceiver>.Initialize(anim, this);
			}
		}

		public void OnNotify(AnimatorStateSignal signal)
		{
			if (signal != null)
			{
				UnityEvent evt;
				if (m_Events.TryGetValue(signal, out evt) && evt != null)
				{
					evt.Invoke();
				}
				onNotify?.Invoke(signal);
			}
		}

		public void AddReaction(AnimatorStateSignal asset, UnityEvent reaction)
		{
			if (asset == null)
				throw new ArgumentNullException("asset");

			if (m_Events.signals.Contains(asset))
				throw new ArgumentException("SignalAsset already used.");
			m_Events.Append(asset, reaction);
		}

		public int AddEmptyReaction(UnityEvent reaction)
		{
			m_Events.Append(null, reaction);
			return m_Events.events.Count - 1;
		}

		public void Remove(AnimatorStateSignal asset)
		{
			if (!m_Events.signals.Contains(asset))
			{
				throw new ArgumentException("The Behavior Signal is not registered with this receiver.");
			}

			m_Events.Remove(asset);
		}

		public IEnumerable<AnimatorStateSignal> GetRegisteredSignals()
		{
			return m_Events.signals;
		}

		public int Count()
		{
			return m_Events.signals.Count;
		}

		public void ChangeSignalAtIndex(int idx, AnimatorStateSignal newKey)
		{
			if (idx < 0 || idx > m_Events.signals.Count - 1)
				throw new IndexOutOfRangeException();

			if (m_Events.signals[idx] == newKey)
				return;
			var alreadyUsed = m_Events.signals.Contains(newKey);
			if (newKey == null || m_Events.signals[idx] == null || !alreadyUsed)
				m_Events.signals[idx] = newKey;

			if (alreadyUsed)
				throw new ArgumentException("SignalAsset already used.");
		}

		public void RemoveAtIndex(int idx)
		{
			if (idx < 0 || idx > m_Events.signals.Count - 1)
				throw new IndexOutOfRangeException();
			m_Events.Remove(idx);
		}

		public void ChangeReactionAtIndex(int idx, UnityEvent reaction)
		{
			if (idx < 0 || idx > m_Events.events.Count - 1)
				throw new IndexOutOfRangeException();

			m_Events.events[idx] = reaction;
		}

		public UnityEvent GetReactionAtIndex(int idx)
		{
			if (idx < 0 || idx > m_Events.events.Count - 1)
				throw new IndexOutOfRangeException();
			return m_Events.events[idx];
		}

		public AnimatorStateSignal GetSignalAssetAtIndex(int idx)
		{
			if (idx < 0 || idx > m_Events.signals.Count - 1)
				throw new IndexOutOfRangeException();
			return m_Events.signals[idx];
		}

		public void Log(string msg)
		{
#if UNITY_EDITOR
			Debug.Log(msg);
#endif
		}

		private void OnEnable()
		{
		}

		[Serializable]
		class EventKeyValue
		{
			[SerializeField]
			List<AnimatorStateSignal> m_Signals = new List<AnimatorStateSignal>();

			[SerializeField, CustomAnimatorStateSignalDrawer]
			List<UnityEvent> m_Events = new List<UnityEvent>();

			public bool TryGetValue(AnimatorStateSignal key, out UnityEvent value)
			{
				var index = m_Signals.IndexOf(key);
				if (index != -1)
				{
					value = m_Events[index];
					return true;
				}

				value = null;
				return false;
			}

			public void Append(AnimatorStateSignal key, UnityEvent value)
			{
				m_Signals.Add(key);
				m_Events.Add(value);
			}

			public void Remove(int idx)
			{
				if (idx != -1)
				{
					m_Signals.RemoveAt(idx);
					m_Events.RemoveAt(idx);
				}
			}

			public void Remove(AnimatorStateSignal key)
			{
				var idx = m_Signals.IndexOf(key);
				if (idx != -1)
				{
					m_Signals.RemoveAt(idx);
					m_Events.RemoveAt(idx);
				}
			}

			public List<AnimatorStateSignal> signals
			{
				get { return m_Signals; }
			}

			public List<UnityEvent> events
			{
				get { return m_Events; }
			}
		}
	}
}

