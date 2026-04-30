using UnityEditor.IMGUI.Controls;

namespace UnityEditor.AnimatorStateSignals
{
	public class AnimatorStateSignalReceiverHeader : MultiColumnHeader
	{
		public AnimatorStateSignalReceiverHeader(MultiColumnHeaderState state) : base(state) { }

		protected override void AddColumnHeaderContextMenuItems(GenericMenu menu)
		{
			menu.AddItem(EditorGUIUtility.TrTextContent("Resize to Fit"), false, ResizeToFit);
		}
	}

}

