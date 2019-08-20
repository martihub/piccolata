using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{	
	[TaskCategory("SALSA")]
	[TaskDescription("Set affinity parameters.")]
	[global::BehaviorDesigner.Runtime.Tasks.HelpURL("https://crazyminnowstudio.com/posts/behavior-designer-actions-for-salsa-lipsync-suite/")]
	//[TaskIcon("{SkinColor}LogIcon.png")]
	public class Eyes_SetAffinity : Action
	{
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("EyesAffinity properties.")]
		public SharedEyesAffinity eyesAffinity;

		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (currentGameObject != null) 
				eyesAffinity.Value.eyes = currentGameObject.GetComponent<Eyes>();

			if (eyesAffinity.Value.eyes != null)
			{
				eyesAffinity.Value.eyes.useAffinity = eyesAffinity.Value.useAffinity;
				eyesAffinity.Value.eyes.affinityPercentage = eyesAffinity.Value.percentage;
				eyesAffinity.Value.eyes.affinityTimerRange = new Vector2(eyesAffinity.Value.timerMin, eyesAffinity.Value.timerMax);
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (eyesAffinity.Value.eyes != null)
				return TaskStatus.Success;
			else
				return TaskStatus.Failure;
		}
	}
}