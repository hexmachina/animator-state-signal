using System;
using System.Collections.Generic;
using System.IO;
using TW.StateSignal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.AnimatorStateSignals
{
	public class AnimatorStateSignalManager : IDisposable
	{
		static AnimatorStateSignalManager m_Instance;
		readonly List<AnimatorStateSignal> m_assets = new List<AnimatorStateSignal>();

		internal static AnimatorStateSignalManager instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = new AnimatorStateSignalManager();
					m_Instance.Refresh();
				}

				return m_Instance;
			}

			set { m_Instance = value; }
		}

		internal AnimatorStateSignalManager()
		{
			AnimatorStateSignal.OnEnableCallback += Register;
		}

		public static IEnumerable<AnimatorStateSignal> assets
		{
			get
			{
				foreach (var asset in instance.m_assets)
				{
					if (asset != null)
						yield return asset;
				}
			}
		}

		public static AnimatorStateSignal CreateSignalAssetInstance(string path)
		{
			var newSignal = ScriptableObject.CreateInstance<AnimatorStateSignal>();
			newSignal.name = Path.GetFileNameWithoutExtension(path);

			var asset = AssetDatabase.LoadMainAssetAtPath(path) as AnimatorStateSignal;
			if (asset != null)
			{
				//TimelineUndo.PushUndo(asset, Styles.UndoCreateSignalAsset);
				EditorUtility.CopySerialized(newSignal, asset);
				Object.DestroyImmediate(newSignal);
				return asset;
			}

			AssetDatabase.CreateAsset(newSignal, path);
			return newSignal;
		}

		public void Dispose()
		{
			AnimatorStateSignal.OnEnableCallback -= Register;
		}

		void Register(AnimatorStateSignal a)
		{
			m_assets.Add(a);
		}

		void Refresh()
		{
			var guids = AssetDatabase.FindAssets("t:AnimatorStateSignal");
			foreach (var g in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(g);
				var asset = AssetDatabase.LoadAssetAtPath<AnimatorStateSignal>(path);
				m_assets.Add(asset);
			}
		}
	}

}

