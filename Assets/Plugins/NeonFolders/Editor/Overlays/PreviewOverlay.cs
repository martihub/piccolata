using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Overlays {
	/// <summary>
	/// Simple overlay showing a preview for Preset-Scriptable objects
	/// </summary>
	public static class PreviewOverlay {
		public static void Draw(string guid, Rect rect) {
			var path = AssetDatabase.GUIDToAssetPath(guid);
			if(AssetDatabase.IsValidFolder(path)) return; // ignore folders
			var type = AssetDatabase.GetMainAssetTypeAtPath(path);
			if(!typeof(Preset).IsAssignableFrom(type)) return; // asset is not a preset
			var preset = AssetDatabase.LoadAssetAtPath<Preset>(path);
			if(preset == null) return; // file was not a preset
			NeonFolders.AdjustIconRect(ref rect);
			preset.DrawPreview(rect);
		}
	}
}
