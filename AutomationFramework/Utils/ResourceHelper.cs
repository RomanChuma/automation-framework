using System.Collections;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace AutomationFramework.Core.Utils
{
	public class ResourceHelper
	{
		/// <summary>
		/// ResourceHelper helps to deal with resource files
		/// </summary>
		/// <param name="resourceName">i.e. "Namespace.ResourceFileName"</param>
		/// <param name="assembly">i.e. GetType().Assembly if working on the local assembly</param>
		public ResourceHelper(string resourceName, Assembly assembly)
		{
			ResourceManager = new ResourceManager(resourceName, assembly);
		}

		private ResourceManager ResourceManager { get; }

		/// <summary>
		/// GetResourceValue returns "Name" value from the resource file, having "Value"
		/// </summary>
		/// <param name="value">Value of the resource "name-value" pair</param>
		public string GetResourceName(string value)
		{
			DictionaryEntry entry = ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true)
				.OfType<DictionaryEntry>().FirstOrDefault(dictionaryEntry => dictionaryEntry.Value.ToString() == value);
			return entry.Key.ToString();
		}
	}
}