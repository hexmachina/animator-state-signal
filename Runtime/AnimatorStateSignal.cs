using System;
using UnityEngine;

namespace TW.StateSignal
{

	[CreateAssetMenu(menuName = "Data/Animator/State Signal")]
	public class AnimatorStateSignal : ScriptableObject
	{
		public static event Action<AnimatorStateSignal> OnEnableCallback;

		void OnEnable()
		{
			if (OnEnableCallback != null)
				OnEnableCallback(this);

		}
	}
}

