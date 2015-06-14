using System;
using System.Text.RegularExpressions;

namespace CodeWalriiNotify
{
	public struct IgnoredEntity
	{
		public uint ID;
		public string Name;
		public bool RegexMatch;
		
		private static readonly Regex ParseMatcher;

		static IgnoredEntity()
		{
			ParseMatcher = new Regex(
				@"^[ \t]*(?:(?:\(|\[)(\d+)(?:\)|\][ \t]*:?)|(\d+)[ \t]*:|#(\d+)[ \t]*:?|(\d+)[ \t]*\/)?[ \t]*(?:([\w ]+)|""([^\n\r]*)""|@\[(.*)\])?[ \t]*$",
				RegexOptions.Singleline
			);
		}

		public IgnoredEntity(uint ID)
		{
			this.ID = ID;
			Name = "";
			RegexMatch = false;
		}

		public IgnoredEntity(string Name, bool RegexMatch)
		{
			ID = 0;
			this.Name = Name;
			this.RegexMatch = RegexMatch;
		}

		public IgnoredEntity(uint ID, string Name, bool RegexMatch)
		{
			this.ID = ID;
			this.Name = Name;
			this.RegexMatch = RegexMatch;
		}

		public override string ToString()
		{
			return ((ID > 0) ? string.Format("#{0}", ID) : "") + ((!string.IsNullOrWhiteSpace(Name) && ID > 0) ? " " : "") + ((!string.IsNullOrWhiteSpace(Name)) ? (RegexMatch ? string.Format("@[{0}]", Name) : (Name.Contains(" ") ? string.Format("\"{0}\"", Name) : Name)) : "");
		}

		public static bool Parsable(string Text)
		{
			Match match = ParseMatcher.Match(Text);
			if (match.Success)
				return match.Groups.Count == 8;
			return false;
		}

		public static bool TryParse(string Text, out IgnoredEntity Entity)
		{
			if (string.IsNullOrWhiteSpace(Text)) {
				Entity = new IgnoredEntity();
				return false;
			}

			Match match = ParseMatcher.Match(Text);

			if (!match.Success) {
				Entity = new IgnoredEntity();
				return false;
			}
			if (match.Groups.Count != 8) {
				Entity = new IgnoredEntity();
				return false;
			}

			var entity = new IgnoredEntity();

			if (!string.IsNullOrWhiteSpace(match.Groups[5].Value)) {
				entity.Name = match.Groups[5].Value;
				entity.RegexMatch = false;
			} else if (!string.IsNullOrWhiteSpace(match.Groups[6].Value)) {
				entity.Name = match.Groups[6].Value;
				entity.RegexMatch = false;
			} else if (!string.IsNullOrWhiteSpace(match.Groups[7].Value)) {
				try {
					new Regex(match.Groups[7].Value).ToString();
				} catch (Exception) {
					Entity = new IgnoredEntity();
					return false;
				}
				entity.Name = match.Groups[7].Value;
				entity.RegexMatch = true;
			}

			if (!string.IsNullOrWhiteSpace(match.Groups[1].Value)) {
				if (!uint.TryParse(match.Groups[1].Value, out entity.ID))
					entity.ID = 0;
			} else if (!string.IsNullOrWhiteSpace(match.Groups[2].Value)) {
				if (!uint.TryParse(match.Groups[2].Value, out entity.ID))
					entity.ID = 0;
			} else if (!string.IsNullOrWhiteSpace(match.Groups[3].Value)) {
				if (!uint.TryParse(match.Groups[3].Value, out entity.ID))
					entity.ID = 0;
			} else if (!string.IsNullOrWhiteSpace(match.Groups[4].Value)) {
				if (!uint.TryParse(match.Groups[4].Value, out entity.ID))
					entity.ID = 0;
			}

			Entity = entity;
			return !(entity.ID == 0 && entity.Name == "");
		}
	}
}

