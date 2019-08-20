using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;
using CrazyMinnow.SALSA;
using CrazyMinnow.SALSA.BehaviorDesigner;

[CustomObjectDrawer(typeof(EyesBlink))]
public class EyesBlinkDrawer : ObjectDrawer
{
	public override void OnGUI(GUIContent label)
	{
		var instance = value as EyesBlink;
		EditorGUILayout.BeginVertical();
		if (FieldInspector.DrawFoldout(instance.GetHashCode(), label)) 
		{
			EditorGUI.indentLevel++;

			instance.customTiming = EditorGUILayout.Toggle("Custom Timing", instance.customTiming);
			if (instance.customTiming)
			{
				instance.durationOn = EditorGUILayout.FloatField("On", instance.durationOn);
				instance.durationHold = EditorGUILayout.FloatField("Hold", instance.durationHold);
				instance.durationOff = EditorGUILayout.FloatField("Off", instance.durationOff);
			}
			
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical();
	}
}