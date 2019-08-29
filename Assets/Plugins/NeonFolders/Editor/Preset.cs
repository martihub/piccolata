using System.Linq;
using UnityEngine;

namespace NeonFolders.Editor {
	/// <summary>
	/// Defines a preset for a multi layered icon, consisting of a large and a small icon.
	/// </summary>
	[CreateAssetMenu(menuName = "NeonFolders/Preset")]
	public class Preset : ScriptableObject {
		public MultiLayerIcon LargeIcon = new MultiLayerIcon();
		public MultiLayerIcon SmallIcon = new MultiLayerIcon();

		public bool CanDraw() {
			if(LargeIcon == null || LargeIcon.Layers == null) return false;
			return LargeIcon.Layers.Any(x => x.BlendColor.a > 0 && x.Icon != null && x.Scale.magnitude > 0);
		}

		/// <summary>
		/// Render this icon in the specified region.
		/// Depending on the size of the region, the large or small icon is rendered.
		/// </summary>
		/// <param name="rect"></param>
		public void Draw(Rect rect) {
			if(rect.height > NeonFolders.SmallIconSize || SmallIcon.IsEmpty)
				LargeIcon.Draw(rect);
			else
				SmallIcon.Draw(rect);
		}

		/// <summary>
		/// Draws a preview of this icon, showing the large and small icon.
		/// </summary>
		/// <param name="rect"></param>
		public void DrawPreview(Rect rect) {
			NeonFolders.DrawRect(rect);
			LargeIcon.Draw(rect);
			var small = NeonFolders.SmallIconSize;
			var icon = SmallIcon.IsEmpty ? LargeIcon : SmallIcon;
			icon.Draw(new Rect(rect.x + 2, rect.y + rect.height - small - 2, small, small));
		}
	}
}
