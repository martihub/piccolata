using UnityEditor;
using UnityEngine;

namespace NeonFolders.Editor.Gui {
	/// <summary>
	/// Custom editor base class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BaseEditor<T> : UnityEditor.Editor where T : Object {
		protected const int Padding = 4;

		protected T Target { get { return (T)target; } }
		protected float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
	}
}
