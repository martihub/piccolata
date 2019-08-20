using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;
using CrazyMinnow.SALSA;
using CrazyMinnow.SALSA.BehaviorDesigner;

[CustomObjectDrawer(typeof(EmoterEmote))]
public class EmoterEmoteDrawer : ObjectDrawer
{
	public override void OnGUI(GUIContent label)
	{
		var instance = value as EmoterEmote;
		EditorGUILayout.BeginVertical();
		if (FieldInspector.DrawFoldout(instance.GetHashCode(), label)) 
		{
			EditorGUI.indentLevel++;

			instance.emoteName = EditorGUILayout.TextField("Emote Name", instance.emoteName);
			instance.handler = (ExpressionComponent.ExpressionHandler)EditorGUILayout.EnumPopup("Handler", instance.handler);

			if (instance.handler == ExpressionComponent.ExpressionHandler.RoundTrip)
				instance.duration = EditorGUILayout.FloatField("Duration", instance.duration);

			if (instance.handler == ExpressionComponent.ExpressionHandler.OneWay)
				instance.animateOn = EditorGUILayout.Toggle("Animate On", instance.animateOn);
			
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical();
	}
}