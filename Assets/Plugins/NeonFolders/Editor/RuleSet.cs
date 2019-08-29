using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeonFolders.GlobPattern;
using UnityEngine;

namespace NeonFolders.Editor {
	/// <summary>
	/// Defines a list of several rules.
	/// </summary>
	[CreateAssetMenu(menuName = "NeonFolders/Rule Set")]
	public class RuleSet : ScriptableObject, ISerializationCallbackReceiver {
		public Rule[] Rules;
		private Dictionary<string, Preset> pathLookup;
		private Dictionary<string, Preset> nameLookup;
		private List<KeyValuePair<Glob, Preset>> globPatterns;

		public bool IsDefaultSet { get { return name == "DefaultRules"; } }

		public RuleSet() {
			NeonFolders.InvalidateRulesetCache();
		}

		public void Add(Rule rule) {
			if(Rules == null) {
				Rules = new[] {rule};
				return;
			}
			var size = Rules.Length;
			Array.Resize(ref Rules, size + 1);
			Rules[size] = rule;
		}

		public void Remove(IEnumerable<Rule> rules) {
			if(Rules == null) return;
			Rules = Rules.Except(rules).ToArray();
		}

		public Preset GetByPath(string path) {
			Preset preset;
			if(pathLookup != null && pathLookup.TryGetValue(path, out preset)) return preset;
			return null;
		}

		public Preset GetByName(string path) {
			Preset preset;
			// ReSharper disable once AssignNullToNotNullAttribute
			if(nameLookup != null && nameLookup.TryGetValue(Path.GetFileName(path), out preset)) return preset;
			return null;
		}

		public Preset GetByGlob(string path) {
			if(globPatterns == null) return null;
			foreach(var patterns in globPatterns) {
				try {
					if(patterns.Key.IsMatch(path)) return patterns.Value;
				} catch(Exception ex) {
					Debug.LogException(ex);
				}
			}
			return null;
		}

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize() {
			if(Rules == null) {
				pathLookup = new Dictionary<string, Preset>();
				nameLookup = new Dictionary<string, Preset>();
				globPatterns = new List<KeyValuePair<Glob, Preset>>();
				return;
			}
			var rules = Rules.Where(x => x != null && x.Value != null && x.Preset != null).ToList();
			pathLookup = GetDictionary(rules.Where(x => x.Type == MatchType.Path));
			nameLookup = GetDictionary(rules.Where(x => x.Type == MatchType.Name));
			globPatterns = rules.Where(x => x.Type == MatchType.Glob).Select(TryParse).Where(x => x.Key != null).ToList();
		}

		private Dictionary<string, Preset> GetDictionary(IEnumerable<Rule> rules) {
			// group is necessary, because there might be duplicate keys and
			// therefore we can't directly do a ToDictionary() call
			return rules.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.First().Preset);
		}

		private KeyValuePair<Glob, Preset> TryParse(Rule rule) {
			Glob glob = TryParseGlob(rule.Value);
			return new KeyValuePair<Glob, Preset>(glob, rule.Preset);
		}

		public static Glob TryParseGlob(string pattern) {
			try {
				return new Glob(pattern, GlobOptions.Compiled);
			} catch(Exception) {
				return null;
			}
		}
	}
}
