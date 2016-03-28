using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace ChangeOrder
{
	[ApiVersion(1, 22)]
	public class ChangeOrder : TerrariaPlugin
    {

		public override string Author
		{
			get { return "Patrikk"; }
		}
		public override string Description
		{
			get { return "Gives ability to change loading order of plugins!"; }
		}

		public override string Name
		{
			get { return "ChangeOrder"; }
		}

		public override Version Version
		{
			get { return new Version(1, 0); }
		}

		public ChangeOrder(Main game)
			: base(game)
		{
			Order = 10000;
		}

		public static PluginsConfig Config = new PluginsConfig();

		public string path = Path.Combine(TShock.SavePath, "ChangeOrder.json");

		public override void Initialize()
		{
			var thisPlugin = ServerApi.Plugins.Where(x => x.Plugin.Name == Name).Select(x => x.Plugin).First();
            if (File.Exists(path))
			{
				Config = PluginsConfig.Read(path);
				if (Config.PluginOrders != null || Config.PluginOrders.Count > 0)
				{
					Console.WriteLine("----------------");
					ServerApi.LogWriter.PluginWriteLine(ServerApi.Plugins.Where(x => x.Plugin.Name == Name).Select(x => x.Plugin).First(), ": Reordering Plugins!", System.Diagnostics.TraceLevel.Info);
					foreach (var plugin in ServerApi.Plugins)
					{
						if (plugin.Plugin.Name == Name || plugin.Plugin.Name == "TShock" || Config.Exceptions.Contains(plugin.Plugin.Name)) continue;
						if (!Config.PluginOrders.ContainsKey(plugin.Plugin.Name))
						{
							Config.PluginOrders.Add(plugin.Plugin.Name, plugin.Plugin.Order);
						}
						else
						{
							try
							{
								plugin.Plugin.Dispose();
								plugin.Plugin.Order = Config.PluginOrders[plugin.Plugin.Name];
								plugin.Plugin.Initialize();
							}
							catch
							{
								ServerApi.LogWriter.PluginWriteLine(thisPlugin, ": Failed to re-order {0}!", System.Diagnostics.TraceLevel.Error);
								ServerApi.LogWriter.PluginWriteLine(thisPlugin, ": Please report this error to Patrikk and meanwhile add the plugin to Exceptions list!", System.Diagnostics.TraceLevel.Info);
							}
							
						}
					}
					foreach (var plugin in ServerApi.Plugins.OrderBy(x => x.Plugin.Order))
					{
						if (plugin.Plugin.Name == Name || plugin.Plugin.Name == "TShock" || Config.Exceptions.Contains(plugin.Plugin.Name)) continue;
						ServerApi.LogWriter.PluginWriteLine(thisPlugin, string.Format("Plugin {0} v{1} (by {2}) initiated.", plugin.Plugin.Name, plugin.Plugin.Version, plugin.Plugin.Author), System.Diagnostics.TraceLevel.Info);

					}
				}
			}
			Config.Write(path);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			
			}
			base.Dispose(disposing);
		}
	}
}
