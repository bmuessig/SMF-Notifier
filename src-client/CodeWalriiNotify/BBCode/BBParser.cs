using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeWalriiNotify
{
	public static class BBParser
	{
		public static List<IBBElement> ParseBBCode(string BBCode)
		{
			List<BBTag> bbtags = ParseTags(BBCode);
			List<IBBElement> elements = UnionElements(BBCode, bbtags);

			return elements;
		}

		public static List<BBTag> ParseTags(string BBCode)
		{
			var tags = new List<BBTag>();

			var parserRegex = new Regex("\\[([A-Za-z0-9\\*\\#\\+\\-]+)(?:[ ]+?([^\\n\\r\\x5d\\x5b]*))?\\](?:([^\\0]*)?(?:\\[\\/\\1\\]))?");
			var argumentRegex = new Regex("([A-Za-z0-9]*)[ ]?=[ ]?((?:([\"'])[^\"']*\\3)|(?:[^\\n\\r= \\[\\]]+))");
			MatchCollection tagResults = parserRegex.Matches(BBCode);

			foreach (Match tagResult in tagResults) {
				if (tagResult.Success && tagResult.Groups.Count == 4) {
					string name = tagResult.Groups[1].Value;
					string content = tagResult.Groups[3].Value;
					var arguments = new Dictionary<string, object>();
					int index = tagResult.Index;
					int length = tagResult.Length;

					MatchCollection argumentResults = argumentRegex.Matches(tagResult.Groups[2].Value);

					foreach (Match argumentResult in argumentResults) {
						if (argumentResult.Success && argumentResult.Groups.Count == 4) {
							string key = argumentResult.Groups[1].Value;
							string value = argumentResult.Groups[2].Value;
							if (argumentResult.Groups[3].Length > 0)
								value = value.Replace(argumentResult.Groups[3].Value, "");
							arguments.Add(key, value);
						}
					}

					tags.Add(new BBTag(name, arguments, index, length, content));
				}
			}

			return tags;
		}

		public static List<IBBElement> UnionElements(string BBCode, List<BBTag> Tags)
		{
			var fragments = new List<IBBElement>();

			int charIndex = 0;
			foreach (BBTag tag in Tags) {
				if (charIndex < tag.Index) {
					fragments.Add(new BBTextSnippet(BBCode.Substring(charIndex, tag.Index - charIndex), charIndex));
					fragments.Add(tag);
					charIndex = tag.Index + tag.Length;
				} else {
					fragments.Add(tag);
					charIndex = tag.Index + tag.Length;
				}
			}

			if (charIndex < BBCode.Length)
				fragments.Add(new BBTextSnippet(BBCode.Substring(charIndex), charIndex));

			return fragments;
		}
	}
}
