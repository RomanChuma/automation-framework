using System;

using RestSharp;

namespace AutomationFramework.Core.Utils.REST
{
	/// <summary>
	/// REST API Client
	/// </summary>
	public class RestClient
	{
		/// <summary>
		/// Wrapped RestSharp.RestClient object
		/// </summary>
		private readonly RestSharp.RestClient _client;

		public Uri Endpoint => _client.BaseUrl;

		public RestClient(string endpointUri)
		{
			_client = new RestSharp.RestClient(endpointUri);
		}

		/// <summary>
		/// Execute request and deserialize response data to given type
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="request">Request</param>
		/// <returns>Object of given type</returns>
		public RestResponse<T> Execute<T>(RestRequest request) where T : new()
		{
			IRestResponse<T> result = _client.Execute<T>(request.Request);
			RestResponse<T> response = new RestResponse<T>(result);
			return response;
		}

		/// <summary>
		/// Execute request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public RestResponse Execute(RestRequest request)
		{
			IRestResponse result = _client.Execute(request.Request);

			if (result.ErrorException != null)
			{
				throw result.ErrorException;
			}

			RestResponse response = new RestResponse(result);
			return response;
		}
	}
}
