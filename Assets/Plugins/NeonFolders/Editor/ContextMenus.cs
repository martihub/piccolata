using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor {
	public static class ContextMenus {
		[MenuItem("Assets/Create/NeonFolders/Create Preset From Texture", true)]
		[MenuItem("Assets/Create/NeonFolders/Create Multiple Presets From Texture", true)]
		public static bool CreatePresetValidation() {
			// all selected objects must be textures
			var guids = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToArray();
			return guids.Length > 0 && guids.All(path => AssetDatabase.LoadAssetAtPath<Texture2D>(path) != null);
		}

		[MenuItem("Assets/Create/NeonFolders/Create Preset From Texture")]
		public static void CreatePreset() {
			// load all textures
			var textures = Selection.assetGUIDs
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<Texture2D>)
				.ToArray();
			CreatePreset(textures);
		}

		[MenuItem("Assets/Create/NeonFolders/Create Multiple Presets From Texture")]
		public static void CreateMultiplePreset() {
			// load all textures
			var textureGroups = Selection.assetGUIDs
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<Texture2D>)
				.GroupBy(x => RemoveExt(x.name));
			foreach(var textures in textureGroups) {
				CreatePreset(textures.ToArray());
			}
		}

		private static void CreatePreset(Texture2D[] textures) {
			var preset = ScriptableObject.CreateInstance<Preset>();
			foreach(var tex in textures) {
				var icon = new IconLayer(tex);
				if(tex.name.EndsWith("_16")) preset.SmallIcon.Add(icon);
				else preset.LargeIcon.Add(icon);
			}
			var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(textures[0]));
			var name = RemoveExt(textures[0].name); // cleanup name
			if(string.IsNullOrEmpty(name)) name = "Preset";
			else if(name.Length == 1) name = name.ToUpper();
			else name = name.Substring(0, 1).ToUpper() + name.Substring(1); // CamelCase
			AssetDatabase.CreateAsset(preset, GetNextFileName(path, name));
		}

		private static string RemoveExt(string name) {
			return name.Replace("_64", "").Replace("_16", "");
		}

		private static string GetNextFileName(string path, string name) {
			if(!Exists(path, name)) return Path.Combine(path, name) + ".asset";
			int i = 0;
			while(Exists(path, name + "_" + i))
				i++;
			return Path.Combine(path, name + "_" + i + ".asset");
		}

		private static bool Exists(string path, string name) {
			return File.Exists(Path.Combine(path, name + ".asset"));
		}
	}
}
