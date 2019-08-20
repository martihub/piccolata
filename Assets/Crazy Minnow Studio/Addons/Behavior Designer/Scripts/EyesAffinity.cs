using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	public class EyesAffinity
	{
		public Eyes eyes;
		public bool useAffinity;
		public float percentage;
		public float timerMin;
		public float timerMax;

		public EyesAffinity()
		{
			this.eyes = null;
			this.useAffinity = false;
			this.percentage = 0.75f;
			this.timerMin = 2f;
			this.timerMax = 5f;
		}
	}
}