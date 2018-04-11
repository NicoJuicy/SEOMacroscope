﻿/*

	This file is part of SEOMacroscope.

	Copyright 2018 Jason Holland.

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
using System.Collections.Generic;
using System.Text;

namespace SEOMacroscope
{

  /// <summary>
  /// Description of MacroscopeDocument.
  /// </summary>

  public partial class MacroscopeDocument : Macroscope
  {

    /**************************************************************************/

    public List<KeyValuePair<string, string>> DetailDocumentDetails ()
    {

      List<KeyValuePair<string, string>> DetailsList = new List<KeyValuePair<string, string>>();

      DetailsList.Add( new KeyValuePair<string, string>( "URL", this.GetUrl() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Status Code", ( (int) this.GetStatusCode() ).ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Status", this.GetStatusCode().ToString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Robots", this.GetAllowedByRobotsAsString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Crawled Date", this.GetCrawledDate() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Error Condition", this.GetErrorCondition() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Duration (seconds)", this.GetDurationInSecondsFormatted() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "HTST Policy Enabled", this.HypertextStrictTransportPolicy.ToString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Content Type", this.GetMimeType() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Content Length", this.ContentLength.ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Encoding", this.ContentEncoding ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Compressed", this.GetIsCompressed().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Compression Method", this.GetCompressionMethod() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Date", this.GetDateServer() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Date Modified", this.GetDateModified() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Expires", this.GetDateExpires() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Locale", this.GetLocale() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Language", this.GetIsoLanguageCode() ) );

      {

        Encoding TextEncoding = this.GetCharacterEncoding();
        string TextEncodingValue = "";

        if( TextEncoding != null )
        {
          TextEncodingValue = TextEncoding.EncodingName;
        }

        DetailsList.Add( new KeyValuePair<string, string>( "Character Encoding", TextEncodingValue ) );

      }

      DetailsList.Add( new KeyValuePair<string, string>( "Character Set", this.GetCharacterSet() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Canonical", this.GetCanonical() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Link: Shortlink", this.GetLinkShortLink() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Link: First", this.GetLinkFirst() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Link: Prev", this.GetLinkPrev() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Link: Next", this.GetLinkNext() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Link: Last", this.GetLinkLast() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Redirect", this.GetIsRedirect().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Redirected From", this.UrlRedirectFrom ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Referrer Meta Tag", this.GetMetaTag( "referrer" ) ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Hyperlinks In Count", this.CountHyperlinksIn().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Hyperlinks Out Count", this.CountHyperlinksOut().ToString() ) );

      {
        List<decimal> HyperlinkRatio = this.DocCollection.GetDocumentHyperlinksRatio( Url: this.GetUrl() );
        DetailsList.Add( new KeyValuePair<string, string>( "Hyperlinks In Ratio", string.Format( "{0:0.00}%", HyperlinkRatio[ 0 ] ) ) );
        DetailsList.Add( new KeyValuePair<string, string>( "Hyperlinks Out Ratio", string.Format( "{0:0.00}%", HyperlinkRatio[ 1 ] ) ) );
      }

      DetailsList.Add( new KeyValuePair<string, string>( "HrefLang Count", this.GetHrefLangs().Count.ToString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Title", this.GetTitle() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Title Length", this.GetTitleLength().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Title Pixel Width", this.GetTitlePixelWidth().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Probable Title Language", this.GetTitleLanguage() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Description", this.GetDescription() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Description Length", this.GetDescriptionLength().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Probable Description Language", this.GetDescriptionLanguage() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Keywords", this.GetKeywords() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Keywords Length", this.GetKeywordsLength().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Keywords Count", this.GetKeywordsCount().ToString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Probable Document Text Language", this.GetDocumentTextLanguage() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Levenshtein Fingerpring", this.GetLevenshteinFingerprint() ) );

      {

        string ReadabilityGradeText;

        ReadabilityGradeText = string.Format(
          "{0} : {1} : {2}",
          MacroscopeAnalyzeReadability.FormatAnalyzeReadabilityMethod(
            ReadabilityMethod: this.GetReadabilityGradeMethod()
          ),
          this.GetReadabilityGrade().ToString( "00.00" ),
          this.GetReadabilityGradeDescription()
        );

        DetailsList.Add( new KeyValuePair<string, string>( "Text Readability", ReadabilityGradeText ) );

      }

      DetailsList.Add( new KeyValuePair<string, string>( "AltText", this.GetAltText() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Checksum", this.GetChecksum() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "ETag", this.GetEtag() ) );

      for( ushort HeadingLevel = 1 ; HeadingLevel <= 6 ; HeadingLevel++ )
      {
        string HeadingText;
        if( this.GetHeadings( HeadingLevel ).Count > 0 )
        {
          HeadingText = this.GetHeadings( HeadingLevel )[ 0 ];
        }
        else
        {
          HeadingText = null;
        }
        if( HeadingText != null )
        {
          DetailsList.Add( new KeyValuePair<string, string>( string.Format( "H{0}", HeadingLevel ), HeadingText ) );
          DetailsList.Add( new KeyValuePair<string, string>( string.Format( "H{0} Length", HeadingLevel ), HeadingText.Length.ToString() ) );
        }
      }

      DetailsList.Add( new KeyValuePair<string, string>( "Page Depth", this.GetDepth().ToString() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Server Name", this.GetServerName() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Scheme", this.GetScheme() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Host and Port", this.GetHostAndPort() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Host", this.GetHostname() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Port", this.GetPort().ToString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Path", this.GetPath() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Query", this.GetQueryString() ) );
      DetailsList.Add( new KeyValuePair<string, string>( "Fragment", this.GetFragment() ) );

      DetailsList.Add( new KeyValuePair<string, string>( "Server Addresses", this.GetHostAddressesAsCsv() ) );

      return ( DetailsList );

    }

    /**************************************************************************/

  }

}
