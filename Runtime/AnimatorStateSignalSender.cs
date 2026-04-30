using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace TW.StateSignal
{

	[DisallowMultipleComponent]
	public class AnimatorStateSignalSender : SceneLinkedSMB<AnimatorStateSignalReceiver>
	{
		public enum AnimatorStateType
		{
			None,
			Enter,
			Update,
			Exit,
			EnterPostTransition,
			ExitPreTransition
		}

		[System.Serializable]
		public struct AnimatorSignalInfo
		{
			public AnimatorStateSignal signal;
			public AnimatorStateType stateType;
			[Range(0, 0.99f)]
			public float ratio;
			public UnityEvent<Animator, AnimatorStateInfo, int> onTriggered;
		}

		[SerializeField] private List<AnimatorSignalInfo> infos = new List<AnimatorSignalInfo>();

		private Dictionary<AnimatorStateType, List<AnimatorSignalInfo>> infoDict;
		private bool[] reached;
		protected override void InternalInitialise(Animator animator, AnimatorStateSignalReceiver monoBehaviour)
		{
			base.InternalInitialise(animator, monoBehaviour);

			infoDict = new Dictionary<AnimatorStateType, List<AnimatorSignalInfo>>();
			var enums = Enum.GetValues(typeof(AnimatorStateType)).Cast<AnimatorStateType>();
			foreach (var item in enums)
			{
				infoDict.Add(item, new List<AnimatorSignalInfo>());
			}
			int reachCount = 0;
			for (int i = 0; i < infos.Count; i++)
			{
				if (infos[i].signal)
				{
					infoDict[infos[i].stateType].Add(infos[i]);
					if (infos[i].stateType == AnimatorStateType.Update)
					{
						++reachCount;
					}
				}
			}
			reached = new bool[reachCount];
		}

		public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!m_MonoBehaviour)
				return;

			for (int i = 0; i < infoDict[AnimatorStateType.Enter].Count; i++)
			{
				m_MonoBehaviour.OnNotify(infoDict[AnimatorStateType.Enter][i].signal);
				infoDict[AnimatorStateType.Enter][i].onTriggered?.Invoke(animator, stateInfo, layerIndex);

			}
			for (int i = 0; i < reached.Length; i++)
			{
				reached[i] = false;
			}
		}

		public override void OnSLStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!m_MonoBehaviour)
				return;

			for (int i = 0; i < infoDict[AnimatorStateType.Update].Count; i++)
			{
				if (!reached[i] && stateInfo.normalizedTime >= infoDict[AnimatorStateType.Update][i].ratio)
				{
					m_MonoBehaviour.OnNotify(infoDict[AnimatorStateType.Update][i].signal);
					infoDict[AnimatorStateType.Update][i].onTriggered?.Invoke(animator, stateInfo, layerIndex);
					reached[i] = true;
				}
			}
		}

		public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!m_MonoBehaviour)
				return;

			for (int i = 0; i < infoDict[AnimatorStateType.Exit].Count; i++)
			{
				m_MonoBehaviour.OnNotify(infoDict[AnimatorStateType.Exit][i].signal);
				infoDict[AnimatorStateType.Exit][i].onTriggered?.Invoke(animator, stateInfo, layerIndex);

			}
		}

		public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!m_MonoBehaviour)
				return;

			for (int i = 0; i < infoDict[AnimatorStateType.EnterPostTransition].Count; i++)
			{
				m_MonoBehaviour.OnNotify(infoDict[AnimatorStateType.EnterPostTransition][i].signal);
				infoDict[AnimatorStateType.EnterPostTransition][i].onTriggered?.Invoke(animator, stateInfo, layerIndex);

			}
		}

		public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!m_MonoBehaviour)
				return;

			for (int i = 0; i < infoDict[AnimatorStateType.ExitPreTransition].Count; i++)
			{
				m_MonoBehaviour.OnNotify(infoDict[AnimatorStateType.ExitPreTransition][i].signal);
				infoDict[AnimatorStateType.ExitPreTransition][i].onTriggered?.Invoke(animator, stateInfo, layerIndex);

			}
		}

	}
}

