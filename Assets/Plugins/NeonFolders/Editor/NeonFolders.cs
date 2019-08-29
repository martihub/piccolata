using System;
using System.Collections.Generic;
using System.Linq;
using NeonFolders.Editor.Overlays;
using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor {
	/// <summary>
	/// Central class for unity editor integration.
	/// </summary>
	[InitializeOnLoad]
	public static class NeonFolders {
		public const int SmallIconSize = 16;
		public const int LargeIconSize = 64;

		private static List<RuleSet> rulesetCache;
		private static bool isRulesetCacheDirty;

		public static Color BackgroundColor {
			get {
				return EditorGUIUtility.isProSkin
					? new Color32(56, 56, 56, 255)
					: new Color32(194, 194, 194, 255);
			}
		}

		public static Color InverseBackgroundColor {
			get {
				var color = Color.white - BackgroundColor;
				color.a = 1f;
				return color;
			}
		}

		/// <summary>
		/// Register overlay handlers.
		/// </summary>
		static NeonFolders() {
			EditorApplication.projectWindowItemOnGUI += FolderOverlay.Draw;
			EditorApplication.projectWindowItemOnGUI += PreviewOverlay.Draw;
			EditorApplication.projectWindowItemOnGUI += RuleSetOverlay.Draw;
			EditorApplication.projectWindowItemOnGUI += EditOverlay.Draw;
		}

		/// <summary>
		/// Adjusts the rect's size to be a square, fixes offsets and returns whether 
		/// the icon is a small icon or not.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool AdjustIconRect(ref Rect rect) {
			var isSmall = rect.width > rect.height;
			rect.width = rect.height = Math.Min(rect.width, rect.height);
			if(rect.width > LargeIconSize) {
				// center icon if region is larger than the large icon
				var size = new Vector2(LargeIconSize, LargeIconSize);
				rect = new Rect(rect.center - size / 2f, size);
			} else if(isSmall && !IsTreeView(rect)) {
				// there is a small offset in the project view for small icons
				rect.x += 3;
			}
			return isSmall;
		}

		private static bool IsTreeView(Rect rect) {
			return ((int)rect.x - 16) % 14 == 0;
		}

		private static IEnumerable<string> FindAll<T>() where T : UnityEngine.Object {
			return AssetDatabase.FindAssets("t:" + typeof(T).FullName).Select(AssetDatabase.GUIDToAssetPath);
		}

		public static void InvalidateRulesetCache() {
			isRulesetCacheDirty = true;
		}

		/// <summary>
		/// Returns all instances.
		/// </summary>
		/// <returns></returns>
		public static IList<RuleSet> GetAllRules() {
			if(rulesetCache == null || isRulesetCacheDirty) {
				rulesetCache = FindAll<RuleSet>().Select(AssetDatabase.LoadAssetAtPath<RuleSet>).ToList();
				isRulesetCacheDirty = false;
			} else {
				rulesetCache.RemoveAll(x => !x);
			}
			return rulesetCache;
		}

		/// <summary>
		/// Returns the default instance.
		/// </summary>
		/// <returns></returns>
		public static RuleSet GetDefaultRules() {
			return GetAllRules().FirstOrDefault(x => x.IsDefaultSet);
		}

		public static void DrawRect(Rect rect) {
			EditorGUI.DrawRect(rect, InverseBackgroundColor);
			rect = new Rect(rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2);
			EditorGUI.DrawRect(rect, BackgroundColor);
		}
	}
}