using System;
using System.Linq;
using UnityEngine;

namespace NeonFolders.Editor {
	/// <summary>
	/// Defines an icon consisting of several layers.
	/// </summary>
	[Serializable]
	public class MultiLayerIcon {
		public IconLayer[] Layers = new IconLayer[0];

		public bool IsEmpty { get { return Layers == null || Layers.Length == 0; } }

		public void Add(IconLayer layer) {
			if(Layers == null) {
				Layers = new[] {layer};
				return;
			}
			var size = Layers.Length;
			Array.Resize(ref Layers, size + 1);
			Layers[size] = layer;
		}

		/// <summary>
		/// Render this icon in the specified region.
		/// </summary>
		/// <param name="rect"></param>
		public void Draw(Rect rect) {
			if(Layers == null) return;
			foreach(var layer in Layers) {
				if(layer != null) layer.Draw(rect);
			}
		}

		public MultiLayerIcon Clone() {
			return new MultiLayerIcon {
				Layers = Layers.Select(x => x.Clone()).ToArray()
			};
		}
	}
}
