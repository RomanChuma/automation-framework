using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AutomationFramework.Core.Utils
{
	public class NetworkHelper
	{
		protected NetworkHelper()
		{
		}

		/// <summary>
		/// Get free TCP port number
		/// </summary>
		/// <returns>Free TCP port</returns>
		public static int GetFreeTcpPort()
		{
			Thread.Sleep(100);
			var tcpListener = new TcpListener(IPAddress.Loopback, 0);
			tcpListener.Start();
			int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
			tcpListener.Stop();
			return port;
		}
	}
}