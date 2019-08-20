using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{	
	[TaskCategory("SALSA")]
	[TaskDescription("Activate an Emoter emote.")]
	[global::BehaviorDesigner.Runtime.Tasks.HelpURL("https://crazyminnowstudio.com/posts/behavior-designer-actions-for-salsa-lipsync-suite/")]
	//[TaskIcon("{SkinColor}LogIcon.png")]
	public class Emoter_Emote : Action
	{
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("Emoter properties.")]
		public SharedEmoterEmote emote;

		private float durationOn;
		private float durationOff;
		private float timer;
        
		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (currentGameObject != null) 
				emote.Value.emoter = currentGameObject.GetComponent<Emoter>();

			if (emote.Value.emoter != null)
			{
				foreach (EmoteExpression exp in emote.Value.emoter.emotes)
				{
					for (int com = 0; com < exp.expData.components.Count; com++)
					{
						if (exp.expData.components[com].durationOn > durationOn)
							durationOn = exp.expData.components[com].durationOn;
						if (exp.expData.components[com].durationOff > durationOff)
							durationOff = exp.expData.components[com].durationOff;
					}
				}

				if (emote.Value.handler == ExpressionComponent.ExpressionHandler.RoundTrip)
				{
					timer = Time.time + emote.Value.duration;
				}
				else
				{
					if (emote.Value.animateOn)
						timer = Time.time + durationOn;
					else
						timer = Time.time + durationOff;
				}

				emote.Value.emoter.ManualEmote(
					emote.Value.emoteName,
					emote.Value.handler,
					emote.Value.duration,
					emote.Value.animateOn);
			}
		}
		
		public override TaskStatus OnUpdate()
		{
			if (emote.Value.emoter != null)
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
			emote = null;
		}
	}
}