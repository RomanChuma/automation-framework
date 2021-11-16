using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utils.Log;

using HtmlAgilityPack;

using Microsoft.Exchange.WebServices.Data;

namespace AutomationFramework.Core.Utils
{
	public class EmailFetcher
	{
		private readonly ILogger _log = Log4NetLogger.Instance;

		private readonly ExchangeService _service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

		private readonly string _urlForExchangeWebService = GlobalSettings.Framework.ExchangeUri;

		private readonly WebCredentials _webCredentials = new WebCredentials(
			GlobalSettings.Framework.ExchangeEmailUserName,
			GlobalSettings.Framework.ExchangeEmailPassword);

		public EmailFetcher()
		{
			_service.Credentials = _webCredentials;
			_service.TraceEnabled = false;
			_service.Url = new Uri(_urlForExchangeWebService);
		}

		/// <summary>
		/// Deletes all emails with search filter provided
		/// </summary>
		/// <param name="searchFilter">Email search filter</param>
		public void DeleteEmailsWithSearchFilter(SearchFilter searchFilter)
		{
			try
			{
				int pageSize = 50;

				// Find items matching given filters
				FindItemsResults<Item> emailItemsToBeDeleted = _service.FindItems(
					WellKnownFolderName.Inbox,
					searchFilter,
					new ItemView(pageSize));

				if (emailItemsToBeDeleted.TotalCount != 0)
				{
					IEnumerable<ItemId> emailItemIds = from email in emailItemsToBeDeleted.Items select email.Id;
					_service.DeleteItems(emailItemIds, DeleteMode.MoveToDeletedItems, null, null);
				}

				_log.Info(
					$"Emails with search filter {searchFilter} have been moved to deleted items folder successfully");
			}
			catch (Exception e)
			{
				_log.Warn("Exception detected while deleting emails");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}
		}

		/// <summary>
		/// Extract all urls from email body
		/// </summary>
		/// <param name="emailBodyInHtml">Email body in HTML format</param>
		/// <returns>List of Urls</returns>
		public List<string> ExtractAllLinksFromEmailBody(string emailBodyInHtml)
		{
			string anchorNodesXpath = "//a[@href]";
			List<string> urls = new List<string>();

			try
			{
				HtmlDocument emailBody = new HtmlDocument();
				emailBody.LoadHtml(emailBodyInHtml);

				// Get all Anchor node from HTML (email body)
				foreach (HtmlNode link in emailBody.DocumentNode.SelectNodes(anchorNodesXpath))
				{
					// Decode Url
					string innerText = link.InnerText;
					string decodedHref = System.Web.HttpUtility.UrlDecode(innerText);
					urls.Add(decodedHref);
				}
			}
			catch (Exception e)
			{
				_log.Warn("Exception detected while extracting url out of email body (HTML)");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}

			return urls;
		}

		/// <summary>
		/// Get all Emails matching search filter
		/// </summary>
		/// <param name="searchFilter">Search Filter for getting Emails</param>
		/// <param name="wellKnownFolder">Folder in which Emails to be searched (Default is inbox)</param>
		/// <param name="subFolderName">Sub Folder in wellKnownFolder</param> 
		/// <param name="pageSize">Number of emails to fetch</param>
		/// <returns>List of All Email items matching search filters</returns>
		public FindItemsResults<Item> GetEmailsWithSearchFilter(
			SearchFilter searchFilter,
			WellKnownFolderName wellKnownFolder = WellKnownFolderName.Inbox,
			string subFolderName = null,
			int pageSize = 100)
		{
			FindItemsResults<Item> emailItems = null;

			try
			{
				ItemView emailItemView = new ItemView(pageSize)
					                         {
						                         PropertySet = new PropertySet(
							                         ItemSchema.Subject,
							                         EmailMessageSchema.IsRead)
					                         };

				if (subFolderName != null)
				{
					// Get sub folder id 
					FolderId subFolderId = GetSubFolderIdForFolderName(wellKnownFolder, subFolderName);

					// Find items matching given filters and Folder id
					emailItems = _service.FindItems(subFolderId, searchFilter, emailItemView);
				}
				else
				{
					// Find items matching given filters and wellKnownFolder
					emailItems = _service.FindItems(wellKnownFolder, searchFilter, emailItemView);
				}
			}
			catch (Exception e)
			{
				_log.Warn(
					$"Exception in getting emails matching search filter {searchFilter} in well known folder {wellKnownFolder}");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}

			return emailItems;
		}

		/// <summary>
		/// Get sub Folder id for sub folder name
		/// </summary>
		/// <param name="wellKnownFolder">Parent folder for sub folder</param>
		/// <param name="subFolderName">Sub Folder Name in wellKnownFolder</param>
		/// <returns>FolderId</returns>
		public FolderId GetSubFolderIdForFolderName(WellKnownFolderName wellKnownFolder = WellKnownFolderName.Inbox, string subFolderName = null)
		{
			int pageSize = 100;
			FolderId subFolderId = null;

			try
			{
				if (subFolderName != null)
				{
					// Set FolderView to find sub folder in well known folder
					FolderView subFolderView = new FolderView(pageSize)
						                           {
							                           PropertySet =
								                           new PropertySet(BasePropertySet.IdOnly)
									                           {
										                           FolderSchema.DisplayName
									                           },
							                           Traversal = FolderTraversal.Deep
						                           };

					FindFoldersResults subFolders = _service.FindFolders(wellKnownFolder, subFolderView);

					// Find specific folder
					foreach (Folder subFolder in subFolders)
					{
						// get folderId of the subFolderName
						if (subFolder.DisplayName == subFolderName)
						{
							subFolderId = subFolder.Id;
							break;
						}
					}
				}
			}
			catch (Exception e)
			{
				_log.Warn($"Exception detected while getting sub folder id from {subFolderName}");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}

			return subFolderId;
		}

		/// <summary>
		/// Returns true/false on if email is received with email subject
		/// </summary>
		/// <param name="emailSubject">Email subject</param>
		/// <param name="wellKnownFolder">wellKnownFolder such as inbox, draft,deleted items</param>
		/// <param name="subFolderName">Sub Folder in WellKnownFolderName such as Inbox -> Firm</param>
		/// <param name="isRead">Is email read flag</param>
		/// <returns>bool</returns>
		public bool IsEmailReceivedWithSubject(
			string emailSubject,
			string subFolderName,
			WellKnownFolderName wellKnownFolder = WellKnownFolderName.Inbox,
			bool isRead = false)
		{
			// Create the search filter.
			var searchFilterCollection = new List<SearchFilter>
				                             {
					                             new SearchFilter.ContainsSubstring(ItemSchema.Subject, emailSubject),
					                             new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)
				                             };

			var searchFilterWithSubject = new SearchFilter.SearchFilterCollection(
				LogicalOperator.And,
				searchFilterCollection.ToArray());
			FindItemsResults<Item> emailsReceived = GetEmailsWithSearchFilter(
				searchFilterWithSubject,
				wellKnownFolder,
				subFolderName);
			bool isEmailsReceived = false;

			// Check if emails received
			if (emailsReceived != null)
			{
				// Received email count
				int totalCountForEmailsReceived = emailsReceived.TotalCount;
				_log.Info($"Total count of emails matching search filter is {totalCountForEmailsReceived}");
				isEmailsReceived = true;
			}

			return isEmailsReceived;
		}

		/// <summary>
		/// Marks all email items as read
		/// </summary>
		/// <param name="wellKnownFolder">Email items to be marked as read</param>
		/// <param name="subFolderName">Folder Id in which emails to be marked as read</param>
		/// <returns>bool</returns>
		public bool MarkAllEmailsAsReadFromFolder(
			WellKnownFolderName wellKnownFolder = WellKnownFolderName.Inbox,
			string subFolderName = null)
		{
			bool successFlag = false;
			Collection<EmailMessage> emailMessages = new Collection<EmailMessage>();

			// Create the search filter.
			List<SearchFilter> searchFilterCollection = new List<SearchFilter>
				                                            {
					                                            new SearchFilter.IsEqualTo(
						                                            EmailMessageSchema.IsRead,
						                                            false)
				                                            };

			// Get Emails For Register To Client Portal
			SearchFilter searchFilterWithSubject = new SearchFilter.SearchFilterCollection(
				LogicalOperator.And,
				searchFilterCollection.ToArray());

			// Wait added as it takes one minute(max) to receive an email after it is sent from sender
			Wait.Until(
				() => GetEmailsWithSearchFilter(searchFilterWithSubject, subFolderName: subFolderName).TotalCount > 0,
				TimeSpan.FromMinutes(1));
			FindItemsResults<Item> emailsReceived = GetEmailsWithSearchFilter(
				searchFilterWithSubject,
				subFolderName: subFolderName);

			// Get Sub FolderId
			FolderId subFolderId = GetSubFolderIdForFolderName(subFolderName: subFolderName);

			try
			{
				// Convert emailItems to EmailMessages
				foreach (var emailItem in emailsReceived)
				{
					EmailMessage emailMessage = (EmailMessage)emailItem;
					emailMessages.Add(emailMessage);
					emailMessage.IsRead = true;
				}

				successFlag = UpdateAllEmailItems(emailMessages, subFolderId);
			}
			catch (Exception e)
			{
				_log.Warn("Exception detected while marking emails as read");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}

			return successFlag;
		}

		/// <summary>
		/// Update all emails from provided folder
		/// </summary>
		/// <param name="messageItems">Updated emails with different attributes</param>
		/// <param name="folderId">Folder Id in which emails to be updated</param>
		/// <returns>bool</returns>
		public bool UpdateAllEmailItems(Collection<EmailMessage> messageItems, FolderId folderId)
		{
			bool successFlag = false;
			try
			{
				// Update emails from provided folder
				ServiceResponseCollection<UpdateItemResponse> response = _service.UpdateItems(
					messageItems,
					folderId,
					ConflictResolutionMode.AutoResolve,
					MessageDisposition.SaveOnly,
					null);

				// Check for success of the UpdateItems method call.
				if (response.OverallResult == ServiceResult.Success)
				{
					Console.WriteLine("All email messages updated successfully.");
					successFlag = true;
				}
				else
				{
					_log.Error("All emails could not update successfully.");
				}
			}
			catch (Exception e)
			{
				_log.Warn("Exception detected while updating email items");
				_log.Error(e.Message, e);
				_log.Error(e.StackTrace, e);
			}

			return successFlag;
		}

		// Decode Url
	}
}