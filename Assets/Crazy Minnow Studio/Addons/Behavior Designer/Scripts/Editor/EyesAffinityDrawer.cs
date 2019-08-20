using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;
using CrazyMinnow.SALSA;
using CrazyMinnow.SALSA.BehaviorDesigner;

[CustomObjectDrawer(typeof(EyesAffinity))]
public class EyesAffinityDrawer : ObjectDrawer
{
	public override void OnGUI(GUIContent label)
	{
		var instance = value as EyesAffinity;
		EditorGUILayout.BeginVertical();
		if (FieldInspector.DrawFoldout(instance.GetHashCode(), label)) 
		{
			EditorGUI.indentLevel++;

			instance.useAffinity = EditorGUILayout.Toggle("Affinity", instance.useAffinity);
			if (instance.useAffinity)
			{
				instance.percentage = EditorGUILayout.Slider("Percentage", instance.percentage, 0f, 1f);
				instance.timerMin = EditorGUILayout.FloatField("Timer Min", instance.timerMin);
				instance.timerMax = EditorGUILayout.FloatField("Timer Max", instance.timerMax);
			}
			
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical();
	}
}