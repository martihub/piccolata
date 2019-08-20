using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{	
	[TaskCategory("SALSA")]
	[TaskDescription("Perform a new blink.")]
	[global::BehaviorDesigner.Runtime.Tasks.HelpURL("https://crazyminnowstudio.com/posts/behavior-designer-actions-for-salsa-lipsync-suite/")]
	//[TaskIcon("{SkinColor}LogIcon.png")]
	public class Eyes_Blink : Action
	{
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("EyesBlink properties.")]
		public SharedEyesBlink eyesBlink;

		private float durOn;
		private float durHold;
		private float durOff;
		private float timer;

		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (currentGameObject != null) 
				eyesBlink.Value.eyes = currentGameObject.GetComponent<Eyes>();

			if (eyesBlink.Value.eyes != null)
			{
				if (eyesBlink.Value.customTiming)
				{
					eyesBlink.Value.eyes.NewBlink(
						eyesBlink.Value.durationOn,
						eyesBlink.Value.durationHold,
						eyesBlink.Value.durationOff);
					timer = Time.time +
					        eyesBlink.Value.durationOn +
					        eyesBlink.Value.durationHold +
					        eyesBlink.Value.durationOff;
				}
				else
				{
					eyesBlink.Value.eyes.NewBlink();
					foreach (EyesExpression exp in eyesBlink.Value.eyes.eyelids)
					{
						for (int com = 0; com < exp.expData.components.Count; com++)
						{
							if (exp.expData.components[com].durationOn > durOn)
								durOn = exp.expData.components[com].durationOn;
							if (exp.expData.components[com].durationHold > durHold)
								durHold = exp.expData.components[com].durationHold;
							if (exp.expData.components[com].durationOff > durOff)
								durOff = exp.expData.components[com].durationOff;
						}
					}

					timer = Time.time + durOn + durHold + durOff;
				}
			}
		}
		
		public override TaskStatus OnUpdate()
		{
			if (eyesBlink.Value.eyes != null)
			{
				if (Time.time > timer)
					return TaskStatus.Success;
				else
					return TaskStatus.Running;
			}
			else
			{
				return TaskStatus.Failure;
			}
		}
		
		public override void OnReset()
		{
			eyesBlink = null;
		}
	}
}