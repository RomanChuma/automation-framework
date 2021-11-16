using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AutomationFramework.Core.Utils.REST
{
	/// <summary>
	/// Generic class that holds deserialized response object.
	/// </summary>
	/// <typeparam name="T">Type of response</typeparam>
	public class RestResponse<T>
	{
		private readonly IRestResponse<T> _response;

		/// <summary>
		/// Used internally to inject and wrap RestSharp.IRestResponse object
		/// </summary>
		/// <param name="response">RestSharp.IRestResponse[T] object to wrap</param>
		internal RestResponse(IRestResponse<T> response)
		{
			_response = response;
		}

		public string Content => _response.Content;

		public IList<RestResponseCookie> Cookies => _response.Cookies;

		/// <summary>
		/// Returns de-serialized response object
		/// </summary>
		public T Data => _response.Data;

		public string ErrorException => _response.ErrorMessage;

		public bool IsSuccessful => _response.IsSuccessful;

		public string Status => _response.StatusDescription;
	}

	/// <summary>
	/// Represent basic Rest HTTP response
	/// </summary>
	public class RestResponse
	{
		private readonly IRestResponse _response;

		/// <summary>
		/// Used internally to inject  and wrap RestSharp.IRestResponse object
		/// </summary>
		/// <param name="response">RestSharp.IRestResponse object to wrap</param>
		internal RestResponse(IRestResponse response)
		{
			_response = response;
		}

		/// <summary>
		/// Returns Response content as plain string
		/// </summary>
		public string Content => _response.Content;

		public IList<RestResponseCookie> Cookies => _response.Cookies;

		public string Date => _response.Headers.First(x => x.Name == "Date").Value.ToString();

		public string ErrorException => _response.ErrorMessage;

		public bool IsSuccessful => _response.IsSuccessful;

		public HttpStatusCode StatusCode => _response.StatusCode;
	}
}