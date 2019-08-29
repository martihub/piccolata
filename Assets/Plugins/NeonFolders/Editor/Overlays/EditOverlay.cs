using System.Linq;
using NeonFolders.Editor.Gui.Windows;
using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Overlays {
	/// <summary>
	/// Provides the edit button overlay functionality.
	/// </summary>
	public static class EditOverlay {
		private const EventModifiers EditModifier = EventModifiers.Alt;

		public static void Draw(string guid, Rect rect) {
			// only active when alt-key is pressed
			if((Event.current.modifiers & EditModifier) == EventModifiers.None) return;

			var isSmall = NeonFolders.AdjustIconRect(ref rect);
			var isMouseOver = rect.Contains(Event.current.mousePosition);

			// if mouse is not over current folder icon or selected group
			if(!isMouseOver && !IsSelected(guid)) return;

			var path = AssetDatabase.GUIDToAssetPath(guid);
			if(!AssetDatabase.IsValidFolder(path)) return;

			var editIcon = isSmall ? Resources.EditFolder16 : Resources.EditFolder64;
			GUI.DrawTexture(rect, editIcon);

			if(GUI.Button(rect, GUIContent.none, GUIStyle.none)) {
				var pos = GUIUtility.GUIToScreenPoint(rect.position + new Vector2(0, rect.height + 2));
				EditFolderPopup.Show(pos, guid);
			}

			EditorApplication.RepaintProjectWindow();
		}

		private static bool IsSelected(string guid) {
			return Selection.assetGUIDs.Any(asset => asset == guid);
		}
	}
}
