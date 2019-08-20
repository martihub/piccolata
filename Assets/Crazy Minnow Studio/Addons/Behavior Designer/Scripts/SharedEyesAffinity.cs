using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	[System.Serializable]
	public class SharedEyesAffinity : SharedVariable<EyesAffinity>
	{
		public static implicit operator SharedEyesAffinity(EyesAffinity value) { return new SharedEyesAffinity { mValue = value }; }
	}
}