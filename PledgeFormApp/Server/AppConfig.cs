using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public static class AppConfig
  {
	public static IConfigurationRoot Config => LazyConfig.Value;

	private static readonly Lazy<IConfigurationRoot> LazyConfig = new Lazy<IConfigurationRoot>(() => new ConfigurationBuilder()
		.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
		.AddJsonFile("appsettings.json")
		.AddJsonFile("config.json")
		.Build());
  }
}
