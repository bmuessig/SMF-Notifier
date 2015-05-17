using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace CodeWalriiNotify
{
	public struct PostMeta
	{
		public String Title { get; set; }

		public String Poster { get; set; }

		public String Body { get; set; }

		public DateTime Time { get; set; }

		public String FormatTime(String Format)
		{
			return Time.ToString();
		}

		public static List<PostMeta> FromJSON(String json)
		{
			var posts = new List<PostMeta>();
			var serializer = new JavaScriptSerializer();

			serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
			object jsonObj = serializer.Deserialize(json, typeof(object));
			bool success = ((bool)((dynamic)jsonObj).success);
			MessageBox.Show(success.ToString());

			return posts;
		}

	}
}

