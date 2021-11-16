using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utils.Jenkins.Models.BuildParameters;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.Jenkins
{
	public class JenkinsApiClient
	{
		private readonly string _apiToken;

		private readonly string _baseUrl;

		private readonly ILogger _log = Log4NetLogger.Instance;

		private readonly string _username;

		public JenkinsApiClient()
		{
			_baseUrl = GlobalSettings.Jenkins.JenkinsUrl;
			_apiToken = GlobalSettings.Jenkins.ApiToken;
			_username = GlobalSettings.Jenkins.Username;
		}

		public void SendBuildRequest(string jobEndpoint, string token)
		{
			string url = $"{_baseUrl}{jobEndpoint}build";

			var request = (HttpWebRequest)WebRequest.Create(url);

			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			var credentialBuffer = new UTF8Encoding().GetBytes($"{_username}:{_apiToken}");

			request.Headers["Authorization"] = $"Basic {Convert.ToBase64String(credentialBuffer)}";

			request.PreAuthenticate = true;

			var parameters = new List<BuildParameters> { new BuildParameters { Name = "token", Value = token } };

			string json = new JavaScriptSerializer().Serialize(new { parameter = parameters.ToArray() });
			json = HttpUtility.UrlEncode(json);

			using (var streamWrite = new StreamWriter(request.GetRequestStream()))
			{
				streamWrite.Write($"json={json}");
			}

			SendRequest(request);
		}

		public void SendBuildRequest(string jobEndpoint, string token, List<BuildParameters> parameters)
		{
			var tokenParameter = new BuildParameters { Name = "token", Value = token };
			parameters.Insert(index: 0, tokenParameter);

			string postData = string.Empty;

			var stringBuilder = new StringBuilder();
			foreach (BuildParameters buildParameter in parameters)
			{
				stringBuilder.Append(buildParameter.ToKeyValue() + "&");
			}

			postData = stringBuilder.ToString();
			postData = postData.Remove(postData.Length - 1);

			string url = $"{_baseUrl}{jobEndpoint}buildWithParameters";
			var request = (HttpWebRequest)WebRequest.Create(url);

			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			var credentialBuffer = new UTF8Encoding().GetBytes($"{_username}:{_apiToken}");
			request.Headers["Authorization"] = $"Basic {Convert.ToBase64String(credentialBuffer)}";
			request.PreAuthenticate = true;

			var data = Encoding.ASCII.GetBytes(postData);
			request.ContentLength = data.Length;

			Stream requestStream = request.GetRequestStream();
			requestStream.Write(data, offset: 0, data.Length);
			requestStream.Close();

			SendRequest(request);
		}

		public void SendRequest(HttpWebRequest request)
		{
			try
			{
				var response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				using (var streamReader = new StreamReader(e.Response.GetResponseStream()))
				{
					string result = streamReader.ReadToEnd();
					_log.Error("There was an error with the request", e);
					_log.Error(result);
				}
			}

			// We wait for 10 seconds since there is a quiet period of about 5-10 seconds on a job
			Wait.For(TimeSpan.FromSeconds(value: 10));
		}
	}
}