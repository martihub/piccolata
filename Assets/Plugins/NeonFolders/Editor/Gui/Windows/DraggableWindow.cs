using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Gui.Windows {
	/// <summary>
	/// Base class for draggable windows.
	/// </summary>
	/// <typeparam name="T">Should be the subclass itself</typeparam>
	public abstract class DraggablePopupWindow<T> : EditorWindow where T : DraggablePopupWindow<T> {
		private const int HeaderHeight = 24;
		private Vector2? offset; // if not null -> dragging in progress
		private Rect HeaderRect { get { return new Rect(position.x, position.y, position.width, HeaderHeight);} }
		
		protected static T Get() {
			var array = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
			var win = array == null || array.Length == 0 ? null : array[0];
			return win ?? CreateInstance<T>();
		}
		
		protected void Show(Rect pos, string title) {
			titleContent = new GUIContent(title);
			minSize = pos.size;
			position = pos;
			Focus();
			ShowPopup();
		}
		
		public void OnGUI() {
			// draw windows with draggable title bar
			NeonFolders.DrawRect(new Rect(0, 0, position.width, position.height));
			NeonFolders.DrawRect(new Rect(0, 0, position.width, HeaderHeight));
			EditorGUI.LabelField(new Rect(4, 4, position.width - 8, EditorGUIUtility.singleLineHeight), titleContent, EditorStyles.boldLabel);
			var e = Event.current;
			var screenPoint = GUIUtility.GUIToScreenPoint(e.mousePosition);

			if(e.button == 0) {
				// calculate offset for the mouse cursor when start dragging
				if(e.type == EventType.MouseDown && HeaderRect.Contains(screenPoint)) {
					offset = position.position - screenPoint;
				} else if(e.type == EventType.MouseDrag && offset.HasValue) {
					var mousePos = screenPoint;
					position = new Rect(mousePos + offset.Value, position.size);
				} else if(e.type == EventType.MouseUp) {
					offset = null;
				}
			}

			var content = new Rect(4, HeaderHeight + 4, position.width - 8, position.height - HeaderHeight - 4);
			GUI.BeginGroup(content);
			DrawGui(new Rect(Vector2.zero, content.size));
			GUI.EndGroup();
		}

		protected abstract void DrawGui(Rect position);
	}
}
