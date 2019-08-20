using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;
using CrazyMinnow.SALSA;
using CrazyMinnow.SALSA.BehaviorDesigner;

[CustomObjectDrawer(typeof(EyesRandom))]
public class EyesRandomDrawer : ObjectDrawer
{
	public override void OnGUI(GUIContent label)
	{
		var instance = value as EyesRandom;
		EditorGUILayout.BeginVertical();
		if (FieldInspector.DrawFoldout(instance.GetHashCode(), label)) 
		{
			EditorGUI.indentLevel++;

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				instance.setHead = EditorGUILayout.Toggle("Set Head", instance.setHead);
				if (instance.setHead)
					instance.randomHead = EditorGUILayout.Toggle("Random", instance.randomHead);
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				instance.setEye = EditorGUILayout.Toggle("Set Eye", instance.setEye);
				if (instance.setEye)
					instance.randomEye = EditorGUILayout.Toggle("Random", instance.randomEye);
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				instance.setBlink = EditorGUILayout.Toggle("Set Blink", instance.setBlink);
				if (instance.setBlink)
					instance.randomBlink = EditorGUILayout.Toggle("Random", instance.randomBlink);
			}
			EditorGUILayout.EndVertical();
			
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical();
	}
}