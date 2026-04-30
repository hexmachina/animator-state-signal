using TW.StateSignal;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatorStateSignals
{
	[CustomEditor(typeof(AnimatorStateSignalReceiver))]
	public class InteractionAnimatorDataInspector : Editor
	{
		AnimatorStateSignalReceiver m_Target;

		[SerializeField] TreeViewState<int> m_TreeState;
		[SerializeField] MultiColumnHeaderState m_MultiColumnHeaderState;
		internal AnimatorStateSignalReceiverTreeView m_TreeView;

		//SignalEmitter signalEmitterContext
		//{
		//	get { return m_Context as SignalEmitter; }
		//}

		void OnEnable()
		{
			m_Target = target as AnimatorStateSignalReceiver;
			InitTreeView(serializedObject);

			Undo.undoRedoPerformed += OnUndoRedo;
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedo;
		}

		void OnUndoRedo()
		{
			m_TreeView.dirty = true;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			using (var changeCheck = new EditorGUI.ChangeCheckScope())
			{
				m_TreeView.RefreshIfDirty();
				//DrawEmitterControls(); // Draws buttons coming from the Context (SignalEmitter)

				EditorGUILayout.Space();
				m_TreeView.Draw();

				DrawAddRemoveButtons();
				//if (signalEmitterContext == null)

				if (changeCheck.changed)
				{
					serializedObject.ApplyModifiedProperties();
					m_TreeView.dirty = true;
				}
			}
		}

		//void DrawEmitterControls()
		//{
		//	var context = signalEmitterContext;
		//	if (context != null)
		//	{
		//		var currentSignal = context.asset;
		//		if (currentSignal != null && !m_Target.IsSignalAssetHandled(currentSignal))
		//		{
		//			EditorGUILayout.Separator();
		//			var message = string.Format(Styles.NoReaction, currentSignal.name);
		//			BehaviorSignalUtility.DrawCenteredMessage(message);
		//			if (BehaviorSignalUtility.DrawCenteredButton(Styles.AddReactionButton))
		//				m_Target.AddNewReaction(currentSignal); // Add reaction on the first
		//			EditorGUILayout.Separator();
		//		}
		//	}
		//}

		internal void SetAssetContext(AnimatorStateSignal asset)
		{
			m_TreeView.SetSignalContext(asset);
		}

		void DrawAddRemoveButtons()
		{
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(Styles.AddReactionButton))
				{
					Undo.RegisterCompleteObjectUndo(m_Target, Styles.UndoAddReaction);
					m_Target.AddEmptyReaction(new UnityEvent());
				}
				GUILayout.Space(18.0f);
			}
		}

		void InitTreeView(SerializedObject so)
		{
			m_TreeState = AnimatorStateSignalListFactory.CreateViewState();
			m_MultiColumnHeaderState = AnimatorStateSignalListFactory.CreateHeaderState();
			var header = AnimatorStateSignalListFactory.CreateHeader(m_MultiColumnHeaderState, SignalReceiverUtility.headerHeight);

			//var context = signalEmitterContext;
			m_TreeView = AnimatorStateSignalListFactory.CreateSignalInspectorList(m_TreeState, header, m_Target, so);
			//m_TreeView.readonlySignals = context != null;

			//if (context != null)
			//m_TreeView.SetSignalContext(context.asset);
		}
	}

}
