using System;
using System.IO;

using AutomationFramework.Core.Enums;

using RestSharp;

namespace AutomationFramework.Core.Utils.REST
{
	/// <summary>
	/// Represents basic Rest Request that will be sent
	/// </summary>
	public class RestRequest
	{
		public RestRequest(string resource, HttpMethod methodType)
		{
			Request = new RestSharp.RestRequest(resource, (Method)Enum.Parse(typeof(Method), methodType.ToString()))
			{
				RequestFormat = DataFormat.Json
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RestRequest"/> class based on HttpMethod type
		/// </summary>
		/// <param name="methodType">Http Method type</param>
		public RestRequest(HttpMethod methodType)
		{
			Request = new RestSharp.RestRequest((Method)Enum.Parse(typeof(Method), methodType.ToString()))
			{
				RequestFormat = DataFormat.Json
			};
		}

		/// <summary>
		/// HTTP Method
		/// </summary>
		public string Method => Request.Method.ToString();

		/// <summary>
		/// Request resource
		/// </summary>
		public string Resource => Request.Resource;

		public HttpDataFormat RequestFormat
		{
			get => (HttpDataFormat)Enum.Parse(typeof(HttpDataFormat), Request.RequestFormat.ToString());
			set => Request.RequestFormat = (DataFormat)Enum.Parse(typeof(DataFormat), value.ToString());
		}

		/// <summary>
		/// Internal RestSharp.RestRequest object
		/// </summary>
		internal RestSharp.RestRequest Request { get; }

		/// <summary>
		/// Add parameter to POST or URL query string based on HTTP Method
		/// </summary>
		/// <param name="parameterName">Name of parameter</param>
		/// <param name="parameterValue">Value of parameter</param>
		public void AddParameter(string parameterName, string parameterValue) => Request.AddParameter(parameterName, parameterValue);

		/// <summary>
		/// Add parameter to POST or URL query string based on HTTP Method
		/// </summary>
		/// <param name="parameterName">Name of parameter</param>
		/// <param name="parameterValue">Value of parameter</param>
		/// <param name="parameterType">Type of parameter</param>
		public void AddParameter(string parameterName, string parameterValue, ParameterType parameterType) => Request.AddParameter(parameterName, parameterValue, parameterType);

		/// <summary>
		/// Add new HTTP Header to request
		/// </summary>
		/// <param name="headerName">Header name</param>
		/// <param name="headerValue">Header value</param>
		public void AddHeader(string headerName, string headerValue) => Request.AddHeader(headerName, headerValue);

		/// <summary>
		/// Add file to POST request
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="filePath">File path</param>
		public void AddFile(string fileName, string filePath) => Request.AddFile(Path.GetFileNameWithoutExtension(fileName), filePath);

		/// <summary>
		/// Replace matching token in request
		/// </summary>
		/// <param name="segmentName">Segment name to replace</param>
		/// <param name="segmentValue">Value to insert in segment</param>
		public void AddUrlSegment(string segmentName, string segmentValue) => Request.AddUrlSegment(segmentName, segmentValue);

		/// <summary>
		/// Add body payload to current Request by serializing the passed object
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		public void AddJsonBody(object obj) => Request.AddJsonBody(obj);

		/// <summary>
		/// Add cookie to the request
		/// </summary>
		/// <param name="name">Cookie name</param>
		/// <param name="value">Cookie value</param>
		public void AddCookie(string name, string value) => Request.AddCookie(name, value);

		public void OnBeforeDeserialization(Action<IRestResponse> response)
		{
			Request.OnBeforeDeserialization = response;
		}
	}
}
