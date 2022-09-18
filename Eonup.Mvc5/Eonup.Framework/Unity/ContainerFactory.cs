using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
namespace Eonup.Framework.Unity
{
	public class ContainerFactory
	{
		private static IUnityContainer container = null;
		static ContainerFactory()
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "CfgFiles\\Unity.cfg.config");//ÕÒÅäÖÃÎÄ¼þµÄÂ·¾¶
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
			container = new UnityContainer();
			section.Configure(container, "eonpuMvc5Container");
		}
		public static IUnityContainer CreateContainer()
		{
			return container;
		}
	}
}
