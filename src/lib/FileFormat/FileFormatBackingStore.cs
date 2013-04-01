using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Messaging;
using Frameworks.Plugin;


namespace Libraries.FileFormat 
{
	public static class FileFormatLoaderBackingStore
	{
		private static Dictionary<Guid, FileFormatLoader> pluginEnvironments;
		static FileFormatLoaderBackingStore()
		{
			pluginEnvironments = new Dictionary<Guid, FileFormatLoader>();
		}
		public static Message Invoke(Guid targetPluginGroup, Message input)
		{
			return pluginEnvironments[targetPluginGroup].Invoke(input);
		}
		public static Tuple<string, string, string, Guid, Tuple<bool, bool>> GetPlugin(Guid targetGuid, Guid pluginGuid)
		{
			FileFormatConverter target = (FileFormatConverter)pluginEnvironments[targetGuid][pluginGuid];
			return new Tuple<string, string, string, Guid, Tuple<bool,bool>>(target.Name, target.FilterString, 
					target.FormCode, target.ObjectID, new Tuple<bool, bool>(target.SupportsSaving, target.SupportsLoading));
		}
		public static Tuple<Guid, Guid[]> LoadPlugins(string path)
		{
			FileFormatLoader pl = new FileFormatLoader(path);
			pluginEnvironments.Add(pl.ObjectID, pl); 
			return new Tuple<Guid,Guid[]>(pl.ObjectID, pl.Names.ToArray());
		}
	}
}
