﻿/*

	This file is part of SEOMacroscope.

	Copyright 2019 Jason Holland.

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
using System.Drawing;
using System.Windows.Forms;

namespace SEOMacroscope
{

  /// <summary>
  /// Description of MacroscopeAboutForm.
  /// </summary>

  public partial class MacroscopeAboutForm : Form
  {

    /**************************************************************************/

    public MacroscopeAboutForm ()
    {
		  
      InitializeComponent(); // The InitializeComponent() call is required for Windows Forms designer support.

      this.Shown += this.CallbackAboutFormShown;

    }

    /**************************************************************************/

    private void CallbackAboutFormShown ( object sender, EventArgs e )
    {
      
      this.labelVersionString.Text = string.Format( "Version: {0}", this.ProductVersion );
      
      if( System.Environment.Is64BitProcess )
      {
        this.labelArchitectureString.Text = string.Format( "Architecture: {0}", "64 bit" );
      }
      else
      {
        this.labelArchitectureString.Text = string.Format( "Architecture: {0}", "32 bit" );
      }
            
    }

    /**************************************************************************/
    
  }

}
