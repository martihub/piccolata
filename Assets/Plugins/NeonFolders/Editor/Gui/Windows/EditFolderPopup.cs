using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Gui.Windows {
	/// <summary>
	/// Popup window shown when rightclicking a folder.
	/// Used to create a new rule for this folder.
	/// </summary>
	public class EditFolderPopup : DraggablePopupWindow<EditFolderPopup> {
		protected const int Padding = 4;
		private Rule rule;
		private RuleSet ruleset;
		private string guid; // guid of the asset which was clicked
		/// <summary>
		/// Path of the clicked asset.
		/// </summary>
		private string Path { get { return AssetDatabase.GUIDToAssetPath(guid); } }
		/// <summary>
		/// Name of the clicked asset.
		/// </summary>
		private string Name { get { return System.IO.Path.GetFileName(Path); } }

		private static Vector2 WindowSize { get { return new Vector2(400, 120); } }
		protected float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }

		public static void Show(Vector2 pos, string guid) {
			var win = Get();
			win.guid = guid;
			win.Show(new Rect(pos, WindowSize), "Create new rule");
			win.rule.Type = MatchType.Path;
			win.rule.Value = win.Path;
		}

		private void OnEnable() {
			rule = new Rule();
			ruleset = NeonFolders.GetDefaultRules() ?? NeonFolders.GetAllRules().FirstOrDefault();
		}

		protected override void DrawGui(Rect position) {
			const int typeWidth = 80;
			var length = NeonFolders.LargeIconSize;
			var valueWidth = position.width - typeWidth - 4 * Padding - length;
			var buttonSize = new Vector2(80, LineHeight);
			var buttonPos = new Vector2(position.width - 2 * (buttonSize.x + Padding), position.height - LineHeight - Padding);
			
			var type = (MatchType)EditorGUI.EnumPopup(new Rect(0, 0, typeWidth, LineHeight), rule.Type);
			if(type != rule.Type) { // type has changed, modify value
				rule.Value = ChangeValue(rule.Value, rule.Type, type);
				rule.Type = type;
			}
			rule.Value = EditorGUI.TextField(new Rect(typeWidth + Padding, 0, valueWidth, LineHeight), rule.Value);
			
			EditorGUI.LabelField(new Rect(0, LineHeight + Padding, typeWidth, LineHeight), "Preset");
			rule.Preset = (Preset)EditorGUI.ObjectField(new Rect(typeWidth + Padding, LineHeight + Padding, valueWidth, LineHeight), rule.Preset, typeof(Preset), false);
			var rect = new Rect(position.width - length - Padding, Padding, length, length);
			NeonFolders.DrawRect(rect);
			if(rule.Preset != null) {
				rule.Preset.DrawPreview(rect);
			}

			ruleset = (RuleSet)EditorGUI.ObjectField(
				new Rect(0, position.height - LineHeight - Padding, buttonPos.x - Padding, buttonSize.y),
				ruleset, typeof(RuleSet), false);
			
			EditorGUI.BeginDisabledGroup(ruleset == null);
			if(GUI.Button(new Rect(buttonPos, buttonSize), "Apply")) {
				Apply();
				Close();
			}
			EditorGUI.EndDisabledGroup();
			buttonPos.x += buttonSize.x + Padding;
			if(GUI.Button(new Rect(buttonPos, buttonSize), "Cancel")) {
				Close();
			}
		}

		private string ChangeValue(string value, MatchType oldType, MatchType newType) {
			if(value == Path || oldType == MatchType.Name && value == Name)
				return GetValueForType(value, newType);
			return value;
		}

		private string GetValueForType(string value, MatchType type) {
			switch(type) {
				case MatchType.Path: return Path;
				case MatchType.Name: return Name;
				default: return value;
			}
		}

		private void Apply() {
			Undo.RecordObject(ruleset, "Added rule");
			ruleset.Add(rule);
			ruleset.OnAfterDeserialize();
			EditorUtility.SetDirty(ruleset);
		}
	}
}
