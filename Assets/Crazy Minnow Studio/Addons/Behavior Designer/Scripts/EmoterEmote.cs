using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	public class EmoterEmote
	{
		public Emoter emoter;
		public string emoteName;
		public ExpressionComponent.ExpressionHandler handler;
		public float duration;
		public bool animateOn;
		
		public EmoterEmote()
		{
			this.emoter = null;
			this.emoteName = "";
			this.handler = ExpressionComponent.ExpressionHandler.RoundTrip;
			this.duration = 2.0f;
			this.animateOn = true;
		}
	}
}