using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NeonFolders.Editor.Gui {
	/// <summary>
	/// Custom editor for presets.
	/// </summary>
	[CustomEditor(typeof(Preset))]
	public class PresetEditor : BaseEditor<Preset> {
		private ReorderableList largeLayers;
		private ReorderableList smallLayers;
		private bool showSmallIcon;

		private void OnEnable() {
			largeLayers = InitList(false);
			smallLayers = InitList(true);
		}

		private ReorderableList InitList(bool isSmall) {
			var propName = isSmall ? "SmallIcon" : "LargeIcon";
			var icon = serializedObject.FindProperty(propName);
			var layers = new ReorderableList(serializedObject, icon.FindPropertyRelative("Layers"),
				true, true, true, true) {elementHeight = NeonFolders.LargeIconSize};
			layers.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Edit " + propName);
			layers.drawElementCallback += (rect, index, active, focused) => DrawLayer(rect, layers, index, isSmall);
			layers.onAddCallback += AddItem;
			return layers;
		}

		private void AddItem(ReorderableList list) {
			var index = list.serializedProperty.arraySize;
			list.serializedProperty.arraySize++;
			list.index = index;
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("Icon").objectReferenceValue = null;
			element.FindPropertyRelative("BlendColor").colorValue = Color.white;
			element.FindPropertyRelative("Offset").vector2Value = Vector2.zero;
			element.FindPropertyRelative("Scale").vector2Value = Vector2.one;
		}

		private void DrawLayer(Rect rect, ReorderableList list, int index, bool isSmall) {
			var length = isSmall ? NeonFolders.SmallIconSize : NeonFolders.LargeIconSize;
			var layeredIcon = isSmall ? Target.SmallIcon : Target.LargeIcon;
			var size = new Vector2(length, length);
			var layer = layeredIcon.Layers[index];
			layer.Draw(new Rect(rect.xMax - size.x, rect.y, size.x, size.y));
			var prop = list.serializedProperty.GetArrayElementAtIndex(index);
			DrawIconAndColor(rect.position, prop);
			DrawVector(new Vector2(rect.x, rect.y + LineHeight + Padding), prop, "Offset");
			DrawVector(new Vector2(rect.x, rect.y + 2 * (LineHeight + Padding)), prop, "Scale");
		}

		private void DrawIconAndColor(Vector2 pos, SerializedProperty layer) {
			const int iconWidth = 114;
			const int colorWidth = 48;
			EditorGUI.PropertyField(new Rect(pos.x, pos.y, iconWidth, LineHeight),
				layer.FindPropertyRelative("Icon"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(pos.x + iconWidth, pos.y, colorWidth, LineHeight),
				layer.FindPropertyRelative("BlendColor"), GUIContent.none);

		}

		private void DrawVector(Vector2 pos, SerializedProperty layer, string name) {
			const int labelWidth = 48;
			const int vectorWidth = 96;
			EditorGUI.LabelField(new Rect(pos.x, pos.y, labelWidth, LineHeight), name);
			EditorGUI.PropertyField(new Rect(pos.x + labelWidth, pos.y, vectorWidth, LineHeight),
				layer.FindPropertyRelative(name), GUIContent.none);
		}

		public override void OnInspectorGUI() {
			var rect = EditorGUILayout.GetControlRect(false, NeonFolders.LargeIconSize);
			var size = NeonFolders.LargeIconSize;
			Target.DrawPreview(new Rect(rect.xMax - size - 6, rect.y, size, size));
			EditorGUILayout.BeginHorizontal();
			showSmallIcon = !GUILayout.Toggle(!showSmallIcon, "Large Icon", "Button");
			showSmallIcon = GUILayout.Toggle(showSmallIcon, "Small Icon", "Button");
			EditorGUILayout.EndHorizontal();

			var list = showSmallIcon ? smallLayers : largeLayers;
			serializedObject.Update();
			list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
			if(GUILayout.Button(string.Format("Copy from {0} icon", showSmallIcon ? "large" : "small"))) {
				Undo.RecordObject(target, "Copy icon");
				if(showSmallIcon)
					Target.SmallIcon = Target.LargeIcon.Clone();
				else
					Target.LargeIcon = Target.SmallIcon.Clone();
			}

			if(showSmallIcon) {
				EditorGUILayout.HelpBox(
					"If you leave the small icon set empty, the large icon set will automatically be used in a scaled down version.",
					MessageType.Info);
			}
		}
	}
}
