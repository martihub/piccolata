using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	[System.Serializable]
	public class SharedEyesRandom : SharedVariable<EyesRandom>
	{
		public static implicit operator SharedEyesRandom(EyesRandom value) { return new SharedEyesRandom { mValue = value }; }
	}
}