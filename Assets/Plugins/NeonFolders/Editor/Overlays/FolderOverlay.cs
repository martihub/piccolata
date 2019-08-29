using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Overlays {
	/// <summary>
	/// Provides the core overlay functionality for folder icons.
	/// </summary>
	public static class FolderOverlay {
		public static void Draw(string guid, Rect rect) {
			var path = AssetDatabase.GUIDToAssetPath(guid);
			if(!AssetDatabase.IsValidFolder(path)) return; // ignore files
			try {
				NeonFolders.AdjustIconRect(ref rect);
				var rules = NeonFolders.GetAllRules();
				// find order: exact path -> exact name -> glob pattern
				var preset = (rules.Select(x => x.GetByPath(path)).FirstOrDefault(NotNull)
				             ?? rules.Select(x => x.GetByName(path)).FirstOrDefault(NotNull))
				             ?? rules.Select(x => x.GetByGlob(path)).FirstOrDefault(NotNull);
				if(preset == null) return; // no preset found
				if(preset.CanDraw())
					EditorGUI.DrawRect(rect, NeonFolders.BackgroundColor);
				preset.Draw(rect);
			} catch(Exception e) {
				Debug.LogException(e);
			}
		}

		private static bool NotNull<T>(T obj) where T : class {
			return obj != null;
		}
	}
}
