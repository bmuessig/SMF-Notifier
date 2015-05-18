using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeWalriiNotify
{
	public class BBTag : IBBElement
	{
		public BBTag()
		{
			Name = "";
			Content = "";
			Index = -1;
			Length = -1;
			ForceEndTag = true;
			Arguments = new Dictionary<string, object>();
		}

		public BBTag(string Name, Dictionary<string, object> Arguments, int Index, int Length, string Content = "", bool ForceEndTag = false)
		{
			this.Name = Name;
			this.Arguments = Arguments;
			this.Index = Index;
			this.Length = Length;
			this.Content = Content;
			this.ForceEndTag = ForceEndTag;
		}

		public BBTag(string BBTagCode, bool ForceEndTag = false)
		{
			this.Arguments = new Dictionary<string, object>();
			this.ForceEndTag = ForceEndTag;

			var parserRegex = new Regex("\\[([A-Za-z0-9\\*#+-]+)(?:[ ]+?([^\\n\\r\\[\\]]*))?\\](?:([^]*)?(?:\\[\\/\\1\\]))?");
			var argumentRegex = new Regex("([A-Za-z0-9]*)[ ]?=[ ]?((?:([\"'])[^\"']*\\3)|(?:[^\\n\\r= \\[\\]]+))");
			var tagResult = parserRegex.Match(BBTagCode);

			if (tagResult.Success && tagResult.Groups.Count == 4) {
				this.Name = tagResult.Groups[1].Value;
				this.Content = tagResult.Groups[3].Value;
				this.Index = tagResult.Index;
				this.Length = tagResult.Length;

				MatchCollection argumentResults = argumentRegex.Matches(tagResult.Groups[2].Value);

				foreach (Match argumentResult in argumentResults) {
					if (argumentResult.Success && argumentResult.Groups.Count == 4) {
						string key = argumentResult.Groups[1].Value;
						string value = argumentResult.Groups[2].Value.Replace(argumentResult.Groups[3].Value, "");
						this.Arguments.Add(key, value);
					}
				}
			} else
				throw new ArgumentException("The BBCode is invalid!", "BBTagCode");
		}

		public string Name {
			get;
			set;
		}

		public string Content {
			get;
			set;
		}

		public bool ForceEndTag {
			get;
			set;
		}

		public Dictionary<string, object> Arguments {
			get;
			set;
		}

		public int Index {
			get;
			set;
		}

		public int Length {
			get;
			set;
		}

		public override string ToString()
		{
			if (Arguments.Count > 0) {
				var ArgumentString = new StringBuilder();
				foreach (KeyValuePair<string, object> Argument in Arguments) {
					ArgumentString.Append(" ");
					ArgumentString.AppendFormat("{0}={1}", Argument.Key, Argument.Value);
				}

				if (Content.Length > 0) {
					return String.Format("[{0}{1}]{2}[/{0}]", Name, ArgumentString, Content);
				} else {
					return ForceEndTag ? String.Format("[{0}{1}][/{0}]", Name, ArgumentString) : String.Format("[{0}{1}]", Name, ArgumentString);
				}
			} else {
				if (Content.Length > 0) {
					return String.Format("[{0}]{1}[/{0}]", Name, Content);
				} else {
					return ForceEndTag ? String.Format("[{0}][/{0}]", Name) : String.Format("[{0}]", Name);
				}
			}
		}
	}
}

