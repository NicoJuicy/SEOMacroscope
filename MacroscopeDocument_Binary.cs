﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Threading;

namespace SEOMacroscope
{

	public partial class MacroscopeDocument : Macroscope
	{

		/**************************************************************************/
		
		Boolean IsBinaryPage()
		{
			HttpWebRequest req = null;
			HttpWebResponse res = null;
			Boolean bIs = false;
			try {
				req = WebRequest.CreateHttp( this.url );
				req.Method = "HEAD";
				req.Timeout = this.timeout;
				req.KeepAlive = false;
				res = ( HttpWebResponse )req.GetResponse();
				debug_msg( string.Format( "Status: {0}", res.StatusCode ), 2 );
				debug_msg( string.Format( "ContentType: {0}", res.ContentType.ToString() ), 2 );
				bIs = true;
				res.Close();
			} catch( WebException ex ) {
				debug_msg( string.Format( "IsBinaryPage :: WebException: {0}", ex.Message ), 2 );
			}
			return( bIs );
		}

		/**************************************************************************/
		
		Boolean ProcessBinaryPage()
		{

			HttpWebRequest req = null;
			HttpWebResponse res = null;

			try {
				req = WebRequest.CreateHttp( this.url );
				req.Method = "HEAD";
				req.Timeout = this.timeout;
				req.KeepAlive = false;
				res = ( HttpWebResponse )req.GetResponse();
			} catch( WebException ex ) {
				debug_msg( string.Format( "ProcessBinaryPage :: WebException: {0}", ex.Message ), 3 );
				debug_msg( string.Format( "ProcessBinaryPage :: WebException: {0}", this.url ), 3 );
			}

			if( res != null ) {
											
				// Status Code
				this.status_code = this.ProcessStatusCode( res.StatusCode );
				debug_msg( string.Format( "Status: {0}", this.status_code ), 2 );

				// Probe HTTP Headers
				foreach( string sHeader in res.Headers ) {
					debug_msg( string.Format( "HTTP HEADER: {0} :: {1}", sHeader, res.GetResponseHeader( sHeader ) ), 3 );
				}

				// Stash HTTP Headers
				this.mime_type = res.ContentType;
				this.content_length = res.ContentLength;
				debug_msg( string.Format( "Content-Type: {0}", this.mime_type ), 3 );			
				debug_msg( string.Format( "Content-Length: {0}", this.content_length.ToString() ), 3 );

				{ // Title
					MatchCollection reMatches = Regex.Matches( this.url, "/([^/]+)$" );
					string sTitle = null;
					foreach( Match match in reMatches ) {
						if( match.Groups[ 0 ].Value.Length > 0 ) {
							sTitle = match.Groups[ 0 ].Value.ToString();
							break;
						}
					}
					if( sTitle != null ) {
						this.title = sTitle;
						debug_msg( string.Format( "TITLE: {0}", this.title ), 3 );
					} else {
						debug_msg( string.Format( "TITLE: {0}", "MISSING" ), 3 );
					}
				}

				res.Close();

			} else {
				this.status_code = 500;
			}

			return( true );
		}

		/**************************************************************************/

	}

}
