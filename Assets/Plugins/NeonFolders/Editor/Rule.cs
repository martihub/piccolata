using System;

namespace NeonFolders.Editor {
	public enum MatchType {
		Path,
		Name,
		Glob
	}

	/// <summary>
	/// Describes a single rule used to match a path to a preset.
	/// </summary>
	[Serializable]
	public class Rule {
		/// <summary>
		/// Depending on the <see cref="Type"/> this value is either a name or a path.
		/// </summary>
		public string Value = string.Empty;
		public MatchType Type;
		public Preset Preset;
	}
}
