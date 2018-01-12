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
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace SEOMacroscope
{
  
  /// <summary>
  /// Displayes the licence file in a dialogue.
  /// </summary>
	
  public partial class MacroscopeLicenceForm : Form
  {
		
    /**************************************************************************/
	      
    public MacroscopeLicenceForm ()
    {

      InitializeComponent(); // The InitializeComponent() call is required for Windows Forms designer support.
      
      this.Shown += this.CallbackLicenceFormShown;
      
    }
    
    /**************************************************************************/

    private void CallbackLicenceFormShown ( object sender, EventArgs e )
    {

      string LicenceText;
      StreamReader Reader = new StreamReader ( Assembly.GetExecutingAssembly().GetManifestResourceStream( "LICENCE" ) );

      LicenceText = Reader.ReadToEnd();

      Reader.Close();
      Reader.Dispose();

      this.richTextBoxLicence.Text = LicenceText;

    }
	      
    /**************************************************************************/
	          
  }
	
}
