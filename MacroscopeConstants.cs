﻿/*
	
	This file is part of SEOMacroscope.
	
	Copyright 2017 Jason Holland.
	
	The GitHub repository may be found at:
	
		https://github.com/nazuke/SEOMacroscope
	
	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.
	
	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.
	
	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;

namespace SEOMacroscope
{
	
	/// <summary>
	/// Description of MacroscopeOutlink.
	/// </summary>
	
	public static class MacroscopeConstants
	{

		/** BEGIN: Named Queues ***************************************************/

		public const string NamedQueueUrlList = "UrlQueue";

		public const string NamedQueueDisplayQueue = "DisplayQueue";
		public const string NamedQueueDisplayStructure = "DisplayStructure";
		public const string NamedQueueDisplayHierarchy = "DisplayHierarchy";
		public const string NamedQueueDisplayCanonicalAnalysis = "CanonicalAnalysis";
		public const string NamedQueueDisplayHrefLang = "DisplayHrefLang";
		public const string NamedQueueDisplayErrors = "Errors";
		public const string NamedQueueDisplayRedirectsAudit = "RedirectsAudit";
		public const string NamedQueueDisplayUriAnalysis = "UriAnalysis";
		public const string NamedQueueDisplayPageTitles = "PageTitles";
		public const string NamedQueueDisplayPageDescriptions = "PageDescriptions";
		public const string NamedQueueDisplayPageKeywords = "PageKeywords";
		public const string NamedQueueDisplayPageHeadings = "PageHeadings";
		public const string NamedQueueDisplayEmailAddresses = "EmailAddresses";
		public const string NamedQueueDisplayTelephoneNumbers = "TelephoneNumbers";
		public const string NamedQueueDisplayHostnames = "DisplayHostnames";

		public const string RecalculateDocCollection = "RecalculateDocCollection";

		/** END: Named Queues *****************************************************/

		/** BEGIN: Outlink Types **************************************************/

		public const string LINK_REDIRECT = "LINK";
		public const string LINK_LINK = "LINK";
		public const string LINK_CANONICAL = "CANONICAL";
		public const string LINK_HREFLANG = "HREFLANG";
		public const string LINK_AHREF = "AHREF";
		public const string LINK_IMAGE = "IMAGE";
		public const string LINK_SCRIPT = "SCRIPT";

		/** END: Outlink Types ****************************************************/

		/** BEGIN: ListView Column Names ******************************************/
		
		public const string Url = "URL";
		
		public const string Status = "Status";
		public const string IsRedirect = "Redirect";
				
		public const string Duration = "Duration (seconds)";

		public const string DateServer = "Date";
		public const string DateModified = "Last Modified";
		
		public const string ContentType = "Content Type";
		public const string Lang = "Lang";
		
		public const string Canonical = "Canonical";
		
		public const string Inhyperlinks = "Links In";
		public const string Outhyperlinks = "Links Out";
		
		public const string Title = "Title";
		public const string TitleLen = "Title Length";
		
		public const string Description = "Description";
		public const string DescriptionLen = "Description Length";
		
		public const string Keywords = "Keywords";
		public const string KeywordsLen = "Keywords Length";
		public const string KeywordsCount = "Keywords Count";
		
		public const string Hn = "First H{0}";
		
		public const string ErrorCondition = "Error Condition";

		/** END: ListView Column Names ********************************************/

	}
	
}