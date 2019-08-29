using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Overlays {
	/// <summary>
	/// Simple overlay drawing a custom icon for RuleSet scriptable objects.
	/// </summary>
	public static class RuleSetOverlay {
		public static void Draw(string guid, Rect rect) {
			var path = AssetDatabase.GUIDToAssetPath(guid);
			if(AssetDatabase.IsValidFolder(path)) return; // ignore folders
			var type = AssetDatabase.GetMainAssetTypeAtPath(path);
			if(!typeof(RuleSet).IsAssignableFrom(type)) return; // asset is not a ruleset
			var preset = AssetDatabase.LoadAssetAtPath<RuleSet>(path);
			if(preset == null) return; // file was not a ruleset
			NeonFolders.AdjustIconRect(ref rect);
			NeonFolders.DrawRect(rect);
			GUI.DrawTexture(rect, Resources.RuleSetIcon);
		}
	}
}
