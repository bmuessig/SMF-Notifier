using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeWalriiNotify
{
	public static class BBParser
	{
		public static List<IBBElement> ParseBBCode(string BBCode)
		{
			List<BBTag> bbtags = BBTag.FromBBCode(BBCode);
			List<BBTextSnippet> bbtexts = BBTextSnippet.FromBBCodeTags(BBCode, bbtags);

			return UnionLists(bbtags.ConvertAll<IBBElement>(
				new Converter<BBTag, IBBElement>((BBTag input) => input)), bbtexts.ConvertAll<IBBElement>(
				new Converter<BBTextSnippet, IBBElement>((BBTextSnippet input) => input)));
		}

		public static List<IBBElement> UnionTagTextElements(string BBCode, List<BBTag> Tags)
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

		public static List<IBBElement> UnionLists(params List<IBBElement>[] ElementLists)
		{
			var unitedList = new List<IBBElement>();

			foreach (List<IBBElement> list in ElementLists) {
				unitedList.AddRange(list);
			}

			SortElementList(ref unitedList);

			return unitedList;
		}

		public static void SortElementList(ref List<IBBElement> UnsortedElements)
		{
			UnsortedElements.Sort(
				(IBBElement p1, IBBElement p2) => p1.Index.CompareTo(p2.Index)
			);
		}
	}
}
