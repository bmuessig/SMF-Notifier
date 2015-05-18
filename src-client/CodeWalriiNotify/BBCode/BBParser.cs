using System;
using System.Collections.Generic;

namespace CodeWalriiNotify
{
	public class BBParser
	{
		public BBParser()
		{
		}

		public static List<IBBElement> ParseBBCode(string BBCode)
		{
			List<BBTag> bbtags = BBTag.FromBBCode(BBCode);
			List<IBBElement> elements = BBTextSnippet.Union(BBCode, bbtags);

			return elements;
		}
	}
}
