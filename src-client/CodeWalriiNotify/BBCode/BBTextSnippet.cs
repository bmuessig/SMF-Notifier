using System;
using System.Collections.Generic;

namespace CodeWalriiNotify
{
	public class BBTextSnippet : IBBElement
	{
		public BBTextSnippet()
		{
			Text = "";
			Index = -1;
			Length = -1;
		}

		public BBTextSnippet(string Text, int Index, int Length)
		{
			this.Text = Text;
			this.Index = Index;
			this.Length = Length;
		}

		public BBTextSnippet(string Text, int Index)
		{
			this.Text = Text;
			this.Index = Index;
			this.Length = Text.Length;
		}

		public String Text {
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

		public static List<BBTextSnippet> FromBBCodeTags(string BBCode, List<BBTag> Tags)
		{
			var fragments = new List<BBTextSnippet>();

			int charIndex = 0;
			foreach (BBTag tag in Tags) {
				if (charIndex < tag.Index) {
					fragments.Add(new BBTextSnippet(BBCode.Substring(charIndex, tag.Index - charIndex), charIndex));
					charIndex = tag.Index + tag.Length;
				} else {
					charIndex = tag.Index + tag.Length;
				}
			}

			if (charIndex < BBCode.Length)
				fragments.Add(new BBTextSnippet(BBCode.Substring(charIndex), charIndex));

			return fragments;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}

