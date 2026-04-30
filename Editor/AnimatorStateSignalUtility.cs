using System;
using System.Collections.Generic;
using System.Linq;
using TW.StateSignal;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatorStateSignals
{
	interface IBehaviorSignalProvider
	{
		AnimatorStateSignal signalAsset { get; set; }
		IEnumerable<AnimatorStateSignal> AvailableSignalAssets();
		void CreateNewSignalAsset(string path);
	}

	static class AnimatorStateSignalUtility
	{
		const string k_SignalExtension = "asset";

		public static void DrawSignalNames(IBehaviorSignalProvider assetProvider, Rect position, GUIContent label, bool multipleValues)
		{
			var assets = assetProvider.AvailableSignalAssets().ToList();
			var index = assets.IndexOf(assetProvider.signalAsset);

			var availableNames = new List<string>();
			availableNames.Add(Styles.EmptySignalList.text);

			availableNames.AddRange(assets.Select(x => x.name));
			availableNames.Add(Styles.CreateNewSignal.text);

			var curValue = index + 1;
			var selected = EditorGUI.Popup(position, label.text, curValue, availableNames.ToArray());
			//EditorGUI.Popup()
			if (selected != curValue)
			{
				var noneEntryIdx = 0;
				if (selected == noneEntryIdx) // None
					assetProvider.signalAsset = null;
				else if (selected == availableNames.Count - 1) // "Create New Asset"
				{
					var path = GetNewSignalPath();
					if (!string.IsNullOrEmpty(path))
						assetProvider.CreateNewSignalAsset(path);
					GUIUtility.ExitGUI();
				}
				else
					assetProvider.signalAsset = assets[selected - 1];
			}
			//using (new GUIMixedValueScope(multipleValues))
			//{
			//}
		}

		public static string GetNewSignalPath()
		{
			return EditorUtility.SaveFilePanelInProject(
				Styles.NewSignalWindowTitle.text,
				Styles.NewSignalDefaultName.text,
				k_SignalExtension,
				Styles.NewSignalWindowMessage.text);
		}

		public static bool IsSignalAssetHandled(this AnimatorStateSignalReceiver receiver, AnimatorStateSignal asset)
		{
			return receiver != null && asset != null && receiver.GetRegisteredSignals().Contains(asset);
		}

		public static void AddNewReaction(this AnimatorStateSignalReceiver receiver, AnimatorStateSignal signalAsset)
		{
			if (signalAsset != null && receiver != null)
			{
				Undo.RegisterCompleteObjectUndo(receiver, Styles.UndoAddReaction);

				var newEvent = new UnityEvent();
				UnityEventTools.AddPersistentListener(newEvent);
				//newEvent.AddPersistentListener();
				var evtIndex = newEvent.GetPersistentEventCount() - 1;
				UnityEventTools.RegisterVoidPersistentListener(newEvent, evtIndex, null);

				//newEvent.RegisterVoidPersistentListenerWithoutValidation(evtIndex, receiver.gameObject, string.Empty);
				receiver.AddReaction(signalAsset, newEvent);
			}
		}

		public static void DrawCenteredMessage(string message)
		{
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label(message);
				GUILayout.FlexibleSpace();
			}
		}

		public static bool DrawCenteredButton(GUIContent buttonLabel)
		{
			bool buttonClicked;
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				buttonClicked = GUILayout.Button(buttonLabel);
				GUILayout.FlexibleSpace();
			}
			return buttonClicked;
		}
	}

	static class SignalReceiverUtility
	{
		const int k_DefaultTreeviewHeaderHeight = 20;

		public static int headerHeight
		{
			get { return k_DefaultTreeviewHeaderHeight; }
		}

		public static SerializedProperty FindSignalsProperty(SerializedObject obj)
		{
			return obj.FindProperty("m_Events.m_Signals");
		}

		public static SerializedProperty FindEventsProperty(SerializedObject obj)
		{
			return obj.FindProperty("m_Events.m_Events");
		}
	}

}
