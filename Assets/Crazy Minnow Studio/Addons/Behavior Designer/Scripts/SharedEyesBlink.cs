using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	[System.Serializable]
	public class SharedEyesBlink : SharedVariable<EyesBlink>
	{
		public static implicit operator SharedEyesBlink(EyesBlink value) { return new SharedEyesBlink { mValue = value }; }
	}
}