using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{	
	[TaskCategory("SALSA")]
	[TaskDescription("Set random for head, eye, and blink.")]
	[global::BehaviorDesigner.Runtime.Tasks.HelpURL("https://crazyminnowstudio.com/posts/behavior-designer-actions-for-salsa-lipsync-suite/")]
	//[TaskIcon("{SkinColor}LogIcon.png")]
	public class Eyes_SetRandom : Action
	{
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("EyesRandom properties.")]
		public SharedEyesRandom eyesRandom;

		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (currentGameObject != null) 
				eyesRandom.Value.eyes = currentGameObject.GetComponent<Eyes>();

			if (eyesRandom.Value.eyes != null)
			{
				if (eyesRandom.Value.setHead)
					eyesRandom.Value.eyes.headRandom = eyesRandom.Value.randomHead;
				if (eyesRandom.Value.setEye)
					eyesRandom.Value.eyes.eyeRandom = eyesRandom.Value.randomEye;
				if (eyesRandom.Value.setBlink)
					eyesRandom.Value.eyes.blinkRandom = eyesRandom.Value.randomBlink;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (eyesRandom.Value.eyes != null)
				return TaskStatus.Success;
			else
				return TaskStatus.Failure;
		}
	}
}