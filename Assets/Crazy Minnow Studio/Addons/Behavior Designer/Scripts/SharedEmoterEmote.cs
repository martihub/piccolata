using BehaviorDesigner.Runtime;

namespace CrazyMinnow.SALSA.BehaviorDesigner
{
	[System.Serializable]
	public class SharedEmoterEmote : SharedVariable<EmoterEmote>
	{
		public static implicit operator SharedEmoterEmote(EmoterEmote value) { return new SharedEmoterEmote { mValue = value }; }
	}
}