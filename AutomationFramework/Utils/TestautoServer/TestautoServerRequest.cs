using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

using AutomationFramework.Core.Utils.Log;
using AutomationFramework.Core.Utils.TestautoServer.Models.RequestParameters;

namespace AutomationFramework.Core.Utils.TestautoServer
{
	public static class TestautoServerRequest
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		public static string TestautoServerUrl => "http://testautoserver.cch.ca/testauto_webservices";

		/// <summary>
		/// Send a POST request to testautoserver.cch.ca
		/// </summary>
		/// <param name="relativeUrl">Relative URL from the TestautoServerUrl staring with a foward slash (/) </param>
		/// <param name="requestParameters">List of parameters for the URL</param>
		public static void Post(string relativeUrl, List<RequestParamater> requestParameters)
		{
			string urlParams = string.Empty;

			var stringBuilder = new StringBuilder();

			foreach (RequestParamater param in requestParameters)
			{
				stringBuilder.Append(param.ToKeyValue() + "&");
			}

			urlParams = stringBuilder.ToString();
			urlParams = urlParams.Remove(urlParams.Length - 1);

			if (!relativeUrl.StartsWith("/"))
			{
				relativeUrl = "/" + relativeUrl;
			}

			string url = $"{TestautoServerUrl}{relativeUrl}?{urlParams}";

			var request = (HttpWebRequest)WebRequest.Create(url);

			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			SendRequest(request);
		}

		private static HttpWebResponse SendRequest(HttpWebRequest request)
		{
			HttpWebResponse response = null;

			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				using (var streamReader = new StreamReader(e.Response.GetResponseStream()))
				{
					string result = streamReader.ReadToEnd();
					Log.Error("There was an error with the request", e);
					Log.Error(result);
					throw;
				}
			}

			return response;
		}
	}
}