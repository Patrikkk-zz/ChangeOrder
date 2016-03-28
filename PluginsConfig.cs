using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeOrder
{
	public class PluginsConfig
	{

		public Dictionary<string, int> PluginOrders = new Dictionary<string, int> { };
		public List<string> Exceptions = new List<string>() { };


		public static PluginsConfig Read(string path)
		{
			if (!File.Exists(path))
				return new PluginsConfig();
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Read(fs);
			}
		}

		public static PluginsConfig Read(Stream stream)
		{
			using (var sr = new StreamReader(stream))
			{
				var cf = JsonConvert.DeserializeObject<PluginsConfig>(sr.ReadToEnd());
				if (ConfigRead != null)
					ConfigRead(cf);
				return cf;
			}
		}

		public void Write(string path)
		{
			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				Write(fs);
			}
		}
		public void Write(Stream stream)
		{
			var str = JsonConvert.SerializeObject(this, Formatting.Indented);
			using (var sw = new StreamWriter(stream))
			{
				sw.Write(str);
			}
		}

		public static Action<PluginsConfig> ConfigRead;

	}
}
