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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RobotsTxt;

namespace SEOMacroscope
{

  public class MacroscopeRobots : Macroscope
  {

    /**************************************************************************/

    private Dictionary<string, Robots> RobotSquad;

    private Dictionary<Uri, bool> BadRobots;

    private MacroscopeHttpTwoClient Client;

    /**************************************************************************/

    public MacroscopeRobots ()
    {

      this.SuppressDebugMsg = true;

      this.RobotSquad = new Dictionary<string, Robots>( 8 );

      this.BadRobots = new Dictionary<Uri, bool>( 8 );

      this.Client = new MacroscopeHttpTwoClient();

    }

    /** ROBOT RULES ***********************************************************/

    public async Task<bool> CheckRobotRule ( string Url )
    {

      bool Allowed = true;
      Robots robot = await this.FetchRobot( Url: Url );
      Uri BaseUri = null;

      try
      {
        BaseUri = new Uri( Url, UriKind.Absolute );
      }
      catch ( UriFormatException ex )
      {
        this.DebugMsg( string.Format( "ApplyRobotRule: {0}", ex.Message ) );
      }
      catch ( Exception ex )
      {
        this.DebugMsg( string.Format( "ApplyRobotRule: {0}", ex.Message ) );
      }

      if ( ( robot != null ) && ( BaseUri != null ) )
      {
        if ( robot.IsPathAllowed( "*", BaseUri.AbsolutePath ) )
        {
          Allowed = true;
        }
        else
        {
          Allowed = false;
          this.DebugMsg( string.Format( "ROBOTS Disallowed: {0}", Url ) );
          this.DebugMsg( string.Format( "ROBOTS AbsolutePath: {0}", BaseUri.AbsolutePath ) );
        }
      }

      return ( Allowed );

    }

    /** -------------------------------------------------------------------- **/

    public async Task<bool> ApplyRobotRule ( string Url )
    {

      bool Allowed = true;

      if ( MacroscopePreferencesManager.GetFollowRobotsProtocol() )
      {
        Allowed = await this.CheckRobotRule( Url: Url );
      }

      return ( Allowed );

    }

    /** Sitemaps **************************************************************/

    public async Task<List<string>> GetSitemapsAsList ( string Url )
    {

      List<string> SitemapsList = new List<string>();

      if ( MacroscopePreferencesManager.GetFollowRobotsProtocol() )
      {

        Robots robot = await this.FetchRobot( Url: Url );

        try
        {

          if ( ( robot != null ) && ( robot.Sitemaps != null ) )
          {

            foreach ( Sitemap SitemapEntry in robot.Sitemaps )
            {

              string SitemapUrl = SitemapEntry.Url.ToString();
              SitemapsList.Add( SitemapUrl );

              this.DebugMsg( string.Format( "ROBOTS SitemapUrl: {0}", SitemapUrl ) );

            }

          }

        }
        catch ( Exception ex )
        {
          this.DebugMsg( ex.Message );
        }

      }

      return ( SitemapsList );

    }

    /** Crawl Delay ***********************************************************/

    public async Task<int> GetCrawlDelay ( string Url )
    {

      int Delay = 0;
      Robots robot;

      if ( !MacroscopePreferencesManager.GetFollowRobotsProtocol() )
      {
        return ( Delay );
      }

      robot = await this.FetchRobot( Url: Url );

      if ( robot != null )
      {

        long CrawlDelayTime = robot.CrawlDelay( this.UserAgent() );

        if ( CrawlDelayTime == 0 )
        {
          CrawlDelayTime = robot.CrawlDelay( "*" );
        }

        if ( CrawlDelayTime > 0 )
        {
          Delay = (int) ( CrawlDelayTime / 1000 );
        }

        DebugMsg( string.Format( "ROBOTS CrawlDelayTime: {0}", CrawlDelayTime ) );
        DebugMsg( string.Format( "ROBOTS Delay: {0}", Delay ) );

      }

      return ( Delay );

    }

    /** Generate Robot URL ****************************************************/

    public static string GenerateRobotUrl ( string Url )
    {

      string RobotUrl = null;

      if ( MacroscopePreferencesManager.GetFollowRobotsProtocol() )
      {

        DebugMsg( string.Format( "ROBOTS Disabled: {0}", Url ), true );

        Uri BaseUri = null;
        string BaseUriPort = "";
        Uri RobotsUri = null;
        string RobotsTxtUrl = null;

        try
        {

          BaseUri = new Uri( Url, UriKind.Absolute );

          if ( BaseUri.Port > 0 )
          {
            BaseUriPort = string.Format( ":{0}", BaseUri.Port );
          }

          RobotsUri = new Uri(
            string.Format(
              "{0}://{1}{2}{3}",
              BaseUri.Scheme,
              BaseUri.Host,
              BaseUriPort,
              "/robots.txt"
            ),
            UriKind.Absolute
          );

          RobotsTxtUrl = RobotsUri.ToString();

        }
        catch ( InvalidOperationException ex )
        {
          DebugMsg( string.Format( "GenerateRobotUrl: {0}", ex.Message ), true );
        }
        catch ( UriFormatException ex )
        {
          DebugMsg( string.Format( "GenerateRobotUrl: {0}", ex.Message ), true );
        }

        if ( !string.IsNullOrEmpty( RobotsTxtUrl ) )
        {
          RobotUrl = RobotsTxtUrl;
        }

      }

      return ( RobotUrl );

    }

    /** Fetch Robot ***********************************************************/

    public async Task<Robots> FetchRobot ( string Url )
    {

      Robots robot = null;
      Uri BaseUri = null;
      Uri RobotsUri = null;
      string RobotsTxtUrl = null;

      /*
      if( !MacroscopePreferencesManager.GetFollowRobotsProtocol() )
      {
        DebugMsg( string.Format( "ROBOTS Disabled: {0}", Url ) );
        return ( robot );
      }
      */

      try
      {

        BaseUri = new Uri( Url, UriKind.Absolute );

        string BaseUriPort = "";

        if ( BaseUri.Port > 0 )
        {
          BaseUriPort = string.Format( ":{0}", BaseUri.Port );
        }

        RobotsUri = new Uri(
          string.Format(
            "{0}://{1}{2}{3}",
            BaseUri.Scheme,
            BaseUri.Host,
            BaseUriPort,
            "/robots.txt"
          ),
          UriKind.Absolute
        );

        RobotsTxtUrl = RobotsUri.ToString();

      }
      catch ( InvalidOperationException ex )
      {
        DebugMsg( string.Format( "FetchRobot: {0}", ex.Message ) );
      }
      catch ( UriFormatException ex )
      {
        DebugMsg( string.Format( "FetchRobot: {0}", ex.Message ) );
      }
      catch ( Exception ex )
      {
        DebugMsg( string.Format( "FetchRobot: {0}", ex.Message ) );
      }

      /*
      lock( this.BadRobots )
      {
        if( !this.BadRobots.ContainsKey( RobotsUri ) )
        {
          return( robot );
        }
      }
      */

      if ( !string.IsNullOrEmpty( RobotsTxtUrl ) )
      {

        if ( this.RobotSquad.ContainsKey( RobotsTxtUrl ) )
        {
          lock ( this.RobotSquad )
          {
            robot = this.RobotSquad[ RobotsTxtUrl ];
          }
        }
        else
        {

          String RobotsText = await this.FetchRobotTextFile( RobotsUri: RobotsUri );

          if ( !string.IsNullOrEmpty( RobotsText ) )
          {
            robot = new Robots( content: RobotsText );
          }
          else
          {
            robot = new Robots( content: "" );
          }

          lock ( this.RobotSquad )
          {
            if ( this.RobotSquad.ContainsKey( RobotsTxtUrl ) )
            {
              robot = this.RobotSquad[ RobotsTxtUrl ];
            }
            else
            {
              this.RobotSquad.Add( RobotsTxtUrl, robot );
            }
          }

        }

      }

      return ( robot );

    }

    /** Fetch Robots Text *****************************************************/

    private async Task<string> FetchRobotTextFile ( Uri RobotsUri )
    {

      MacroscopeHttpTwoClientResponse Response = null;
      bool Proceed = false;
      string RobotText = "";
      string RawData = "";

      if ( !MacroscopeDnsTools.CheckValidHostname( Url: RobotsUri.ToString() ) )
      {
        DebugMsg( string.Format( "FetchRobotTextFile :: CheckValidHostname: {0}", "NOT OK" ) );
        return ( RobotText );
      }

      try
      {

        Response = await this.Client.Get(
          RobotsUri,
          this.ConfigureHeadRequestHeadersCallback,
          this.PostProcessRequestHttpHeadersCallback,
          MacroscopeHttpTwoClient.DecodeResponseContentAs.STRING
        );

        if ( Response != null )
        {
          Proceed = true;
        }

      }
      catch ( MacroscopeDocumentException ex )
      {
        DebugMsg( string.Format( "MacroscopeDocumentException: {0}", ex.Message ) );
        DebugMsg( string.Format( "MacroscopeDocumentException: {0}", RobotsUri.ToString() ) );
      }
      catch ( Exception ex )
      {
        DebugMsg( string.Format( "Exception: {0}", ex.Message ) );
        DebugMsg( string.Format( "Exception: {0}", RobotsUri.ToString() ) );
      }

      if ( ( Proceed ) && ( Response != null ) )
      {

        try
        {
          RawData = Response.GetContentAsString();
        }
        catch ( Exception ex )
        {
          DebugMsg( string.Format( "FetchRobotTextFile: Exception: {0}", ex.Message ) );
          RawData = "";
        }

      }
      else
      {

        lock ( this.BadRobots )
        {
          if ( !this.BadRobots.ContainsKey( RobotsUri ) )
          {
            this.BadRobots.Add( RobotsUri, true );
            RobotText = "";
          }
        }

      }

      if ( !string.IsNullOrEmpty( RawData ) )
      {
        RobotText = RawData;
      }

      return ( RobotText );

    }

    /** HTTP Headers **********************************************************/

    // https://en.wikipedia.org/wiki/List_of_HTTP_header_fields

    private void ConfigureHeadRequestHeadersCallback ( HttpRequestMessage Request )
    {

      /*
      string HostAndPort = RobotsUri.Host;

      if( RobotsUri.Port > 0 )
      {
        HostAndPort = string.Join( ":", RobotsUri.Host, RobotsUri.Port.ToString() );
      }

      req.Host = HostAndPort;
      */

    }

    /** -------------------------------------------------------------------- **/

    private void PostProcessRequestHttpHeadersCallback ( HttpRequestMessage Request )
    {
    }

    /**************************************************************************/

  }

}
