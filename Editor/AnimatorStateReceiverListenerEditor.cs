using TW.StateSignal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AnimatorStateReceiverListener))]
public class AnimatorStateReceiverListenerEditor : Editor
{
	ReorderableList evaluateList;
	SerializedProperty signals;
	SerializedProperty events;

	private void OnEnable()
	{
		signals = serializedObject.FindProperty("signals");
		events = serializedObject.FindProperty("events");

		evaluateList = new ReorderableList(serializedObject, signals, false, false, true, true);
		evaluateList.drawElementCallback = OnDrawEvalElement;
		evaluateList.onAddCallback = AddEvalCallback;
		evaluateList.onRemoveCallback = RemoveEvalCallback;
		evaluateList.elementHeightCallback = ElementHeightEvalCallback;
		evaluateList.drawHeaderCallback = OnDrawEvalHeader;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		evaluateList.DoLayoutList();
		if (serializedObject.hasModifiedProperties)
		{
			serializedObject.ApplyModifiedProperties();
		}
	}

	private void OnDrawEvalHeader(Rect rect)
	{
		EditorGUI.LabelField(rect, "Signal Events");
	}

	private void OnDrawEvalElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		rect.height = EditorGUIUtility.singleLineHeight;

		var bit = signals.GetArrayElementAtIndex(index);
		EditorGUI.PropertyField(rect, bit, new GUIContent("Signal"));

		rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		var ev = events.GetArrayElementAtIndex(index);
		rect.height = EditorGUI.GetPropertyHeight(ev);
		EditorGUI.PropertyField(rect, ev, new GUIContent("On Notify"), false);
	}

	private void RemoveEvalCallback(ReorderableList list)
	{
		var index = list.index;
		signals.DeleteArrayElementAtIndex(index);
		events.DeleteArrayElementAtIndex(index);
	}

	private void AddEvalCallback(ReorderableList list)
	{

		signals.arraySize++;
		events.arraySize++;
	}

	private float ElementHeightEvalCallback(int index)
	{
		var height = EditorGUIUtility.standardVerticalSpacing * 3;
		if (index < events.arraySize)
		{
			var ev = events.GetArrayElementAtIndex(index);
			height += EditorGUI.GetPropertyHeight(ev);
		}
		if (index < signals.arraySize)
		{
			var bit = signals.GetArrayElementAtIndex(index);
			height += EditorGUI.GetPropertyHeight(bit);
		}
		return height;
	}
}