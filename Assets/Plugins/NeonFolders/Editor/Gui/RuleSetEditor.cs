using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NeonFolders.Editor.Gui {
	/// <summary>
	/// Custom editor for rulesets.
	/// </summary>
	[CustomEditor(typeof(RuleSet))]
	public class RuleSetEditor : BaseEditor<RuleSet> {
		private ReorderableList list;

		private void OnEnable() {
			list = new ReorderableList(serializedObject, serializedObject.FindProperty("Rules"),
				true, true, true, true) { elementHeight = NeonFolders.LargeIconSize + 2 * Padding };
			list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Rules");
			list.drawElementCallback += DrawRule;
			list.onAddCallback += AddItem;
		}

		private void DrawRule(Rect rect, int i, bool isActive, bool isFocused) {
			const int rightPadding = 18;
			const int labelWidth = 48;
			var rule = list.serializedProperty.GetArrayElementAtIndex(i);
			var length = NeonFolders.LargeIconSize;
			var labelRect = new Rect(rect.x, rect.y, labelWidth, LineHeight);
			var valueRect = new Rect(labelRect.xMax, rect.y, rect.width - labelWidth - length - rightPadding, LineHeight);
			var lineRect = new Rect(rect.x, rect.y + Padding + LineHeight, rect.width - length - rightPadding, LineHeight);
			EditorGUI.LabelField(labelRect, "Type");
			EditorGUI.PropertyField(valueRect, rule.FindPropertyRelative("Type"), GUIContent.none);
			var oldColor = GUI.color;
			if(rule.FindPropertyRelative("Type").enumValueIndex == (int)MatchType.Glob &&
			   RuleSet.TryParseGlob(rule.FindPropertyRelative("Value").stringValue) == null)
				GUI.color = Color.red;
			EditorGUI.PropertyField(lineRect, rule.FindPropertyRelative("Value"), GUIContent.none);
			GUI.color = oldColor;
			lineRect.y += Padding + LineHeight;
			lineRect.width += rightPadding; // circle beneath field should be outside of the 'regular' field width
			EditorGUI.PropertyField(lineRect, rule.FindPropertyRelative("Preset"), GUIContent.none);
			var preset = Target.Rules[i].Preset;
			if(preset != null) preset.DrawPreview(new Rect(rect.xMax - length, rect.y, length, length));
		}

		private void AddItem(ReorderableList list) {
			var index = list.serializedProperty.arraySize;
			list.serializedProperty.arraySize++;
			list.index = index;
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("Value").stringValue = "";
			element.FindPropertyRelative("Type").enumValueIndex = 0;
			element.FindPropertyRelative("Preset").objectReferenceValue = null;
		}

		public override void OnInspectorGUI() {
			if(Target.IsDefaultSet) {
				EditorGUILayout.HelpBox("All rules added via the config menu will be added to this rule set.", MessageType.Info);
			}
			serializedObject.Update();
			list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
			if(GUILayout.Button("Remove Empty Entries")) Remove(GetEmptyRules());
			if(GUILayout.Button("Remove non existing paths")) Remove(GetNonExistingPaths());
		}

		private void Remove(List<Rule> rules) {
			if(rules.Count == 0) return;
			Undo.RecordObject(Target, "Removed empty rules");
			Target.Remove(rules);
		}

		private List<Rule> GetEmptyRules() {
			return Target.Rules.Where(x => string.IsNullOrEmpty(x.Value) || x.Preset == null).ToList();
		}

		private List<Rule> GetNonExistingPaths() {
			return Target.Rules
				.Where(x => x.Type == MatchType.Path)
				.Where(x => string.IsNullOrEmpty(x.Value) || !Directory.Exists(x.Value))
				.ToList();
		}
	}
}
