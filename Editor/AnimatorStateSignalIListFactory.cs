using TW.StateSignal;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor.AnimatorStateSignals
{
	static class AnimatorStateSignalListFactory
	{
		public static AnimatorStateSignalReceiverTreeView CreateSignalInspectorList(TreeViewState state, AnimatorStateSignalReceiverHeader header, AnimatorStateSignalReceiver target, SerializedObject so)
		{
			return new AnimatorStateSignalReceiverTreeView(state, header, target, so);
		}

		public static AnimatorStateSignalReceiverHeader CreateHeader(MultiColumnHeaderState state, int columnHeight)
		{
			var header = new AnimatorStateSignalReceiverHeader(state) { height = columnHeight };
			header.ResizeToFit();
			return header;
		}

		public static MultiColumnHeaderState CreateHeaderState()
		{
			return new MultiColumnHeaderState(AnimatorStateSignalReceiverTreeView.GetColumns());
		}

		public static TreeViewState CreateViewState()
		{
			return new TreeViewState();
		}
	}
}

