using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	public class EyesBlink
	{
		public Eyes eyes;
		public bool customTiming;
		public float durationOn;
		public float durationHold;
		public float durationOff;

		public EyesBlink()
		{
			this.eyes = null;
			this.customTiming = false;
			this.durationOn = 0.1f;
			this.durationHold = 0.1f;
			this.durationOff = 0.1f;
		}
	}
}