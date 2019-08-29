using System;
using UnityEngine;

namespace NeonFolders.Editor {
	/// <summary>
	/// Represents a single layer of a multi layered icon.
	/// </summary>
	[Serializable]
	public class IconLayer {
		/// <summary>
		/// The icon to be rendered.
		/// </summary>
		public Texture2D Icon;
		/// <summary>
		/// The blending color of the icon.
		/// </summary>
		public Color BlendColor = Color.white;
		/// <summary>
		/// The offset relative to the size of the rendered region. 
		/// A value of (0, 0) means that this icon is rendered in the top left corner of the tagret region.
		/// A value of (1, 1) means that this icon is rendered at the bottom right corner of the target region, 
		/// which means that it will actually be outside of the target region.
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// The scaling factor of this icon.
		/// </summary>
		public Vector2 Scale = Vector2.one;

		public IconLayer() { }

		public IconLayer(Texture2D texture) {
			Icon = texture;
		}

		/// <summary>
		/// Render this icon in the specified region.
		/// </summary>
		/// <param name="rect"></param>
		public void Draw(Rect rect) {
			if(Icon == null) return;
			var oldColor = GUI.color;
			GUI.color = BlendColor;
			var pos = rect.position + Offset * rect.width; // width == height
			var size = new Vector2(rect.width * Scale.x, rect.height * Scale.y);
			GUI.DrawTexture(new Rect(pos, size), Icon);
			GUI.color = oldColor;
		}

		public IconLayer Clone() {
			return new IconLayer {
				Icon = Icon,
				BlendColor = BlendColor,
				Offset = Offset,
				Scale = Scale
			};
		}
	}
}
