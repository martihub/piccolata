using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{	
	[TaskCategory("SALSA")]
	[TaskDescription("Set or clear the look target.")]
	[global::BehaviorDesigner.Runtime.Tasks.HelpURL("https://crazyminnowstudio.com/posts/behavior-designer-actions-for-salsa-lipsync-suite/")]
	//[TaskIcon("{SkinColor}LogIcon.png")]
	public class Eyes_Look : Action
	{
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
		[global::BehaviorDesigner.Runtime.Tasks.Tooltip("EyesAffinity properties.")]
		public SharedTransform lookTarget;
		
		private Eyes eyes;

		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (currentGameObject != null) 
				eyes = currentGameObject.GetComponent<Eyes>();
		}
		
		public override TaskStatus OnUpdate()
		{
			if (eyes)
			{
				eyes.lookTarget = lookTarget.Value;
				return TaskStatus.Success;
			}
			else
			{
				return TaskStatus.Failure;
			}
		}
		
		public override void OnReset()
		{
			eyes = null;
			lookTarget = null;
		}
	}
}