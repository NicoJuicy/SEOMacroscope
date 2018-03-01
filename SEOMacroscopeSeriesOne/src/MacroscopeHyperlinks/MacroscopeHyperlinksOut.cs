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

namespace SEOMacroscope
{

  /// <summary>
  /// Description of MacroscopeHyperlinksOut.
  /// </summary>

  public class MacroscopeHyperlinksOut : Macroscope
  {

    // TODO: Write unit tests

    /**************************************************************************/

    private List<MacroscopeHyperlinkOut> Links;

    /**************************************************************************/

    public MacroscopeHyperlinksOut ()
    {
      this.SuppressDebugMsg = true;
      this.Links = new List<MacroscopeHyperlinkOut> ( 256 );
    }

    /**************************************************************************/

    public void Clear ()
    {
      lock( this.Links )
      {
        this.Links.Clear();
      }
    }
    
    /**************************************************************************/

    public MacroscopeHyperlinkOut Add (
      MacroscopeConstants.HyperlinkType LinkType,
      string UrlTarget
    )
    {
      MacroscopeHyperlinkOut HyperlinkOut = new MacroscopeHyperlinkOut ();
      HyperlinkOut.SetHyperlinkType( LinkType );
      HyperlinkOut.SetTargetUrl( UrlTarget );
      lock( this.Links )
      {
        this.Links.Add( HyperlinkOut );
      }
      return( HyperlinkOut );
    }

    /**************************************************************************/

    public void Remove ( MacroscopeHyperlinkOut HyperlinkOut )
    {
      lock( this.Links )
      {
        foreach( MacroscopeHyperlinkOut HyperlinkOutOld in this.Links )
        {
          if( HyperlinkOutOld.Equals( HyperlinkOut ) )
          {
            this.Links.Remove( HyperlinkOutOld );
          }
        }
      }
    }

    /**************************************************************************/

    public bool ContainsLink ( MacroscopeHyperlinkOut  Link )
    {
      bool LinkPresent = false;
      lock( this.Links )
      {
        if( this.Links.Contains( Link ) )
        {
          LinkPresent = true;
        }
      }
      return( LinkPresent );
    }

    /**************************************************************************/

    public bool ContainsHyperlinkOut ( string Url )
    {
      bool LinkPresent = false;
      lock( this.Links )
      {
        foreach( MacroscopeHyperlinkOut HyperlinkOut in this.Links )
        {
          if( HyperlinkOut.GetTargetUrl() == Url )
          {
            LinkPresent = true;
          }
        }
      }
      return( LinkPresent );
    }

    /**************************************************************************/

    public IEnumerable<MacroscopeHyperlinkOut> IterateLinks ()
    {
      lock( this.Links )
      {
        foreach( MacroscopeHyperlinkOut HyperlinkOut in this.Links )
        {
          yield return HyperlinkOut;
        }
      }
    }

    /**************************************************************************/

    public int Count ()
    {
      return( this.Links.Count );
    }

    /**************************************************************************/

  }

}
