using System;
using System.Reflection;

using log4net;
using log4net.Core;

namespace AutomationFramework.Core.Extensions
{
	public static class ILogExtentions
	{
		public static void Trace(this ILog log, string message, Exception exception)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType,
				Level.Trace, message, exception);
		}

		public static void Verbose(this ILog log, string message, Exception exception)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType,
				Level.Verbose, message, exception);
		}
	}
}
