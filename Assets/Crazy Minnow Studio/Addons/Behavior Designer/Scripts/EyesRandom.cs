using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	public class EyesRandom
	{
		public Eyes eyes;
		public bool setHead;
		public bool randomHead;
		public bool setEye;
		public bool randomEye;
		public bool setBlink;
		public bool randomBlink;

		public EyesRandom()
		{
			this.eyes = null;
			this.setHead = false;
			this.randomHead = true;
			this.setEye = false;
			this.randomEye = true;
			this.setBlink = false;
			this.randomBlink = true;
		}
	}
}