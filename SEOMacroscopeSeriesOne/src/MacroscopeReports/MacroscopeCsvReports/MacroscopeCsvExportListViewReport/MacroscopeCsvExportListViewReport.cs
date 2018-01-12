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
using System.Windows.Forms;
using CsvHelper;

namespace SEOMacroscope
{

  public partial class MacroscopeCsvExportListViewReport : MacroscopeCsvReports
  {

    /**************************************************************************/

    private ListView TargetListView;
    
    /**************************************************************************/

    public MacroscopeCsvExportListViewReport ( ListView SelectedListView )
    {
      this.TargetListView = SelectedListView;
    }

    /**************************************************************************/

    public void WriteCsv (
      MacroscopeJobMaster JobMaster,
      string OutputFilename
    )
    {

      try
      {
              
        using( StreamWriter writer = File.CreateText( OutputFilename ) )
        {

          CsvWriter ws = new CsvWriter ( writer );

          lock( this.TargetListView )
          {
            this.BuildWorksheetListView( JobMaster, ws );
          }

        }

      }
      catch( CsvHelperException )
      {
        MacroscopeSaveCsvFileException CannotSaveCsvFileException;
        CannotSaveCsvFileException = new MacroscopeSaveCsvFileException (
          string.Format( "Cannot write to CSV file at {0}", OutputFilename )
        );
        throw CannotSaveCsvFileException;
      }
      catch( IOException )
      {
        MacroscopeSaveCsvFileException CannotSaveCsvFileException;
        CannotSaveCsvFileException = new MacroscopeSaveCsvFileException (
          string.Format( "Cannot write to CSV file at {0}", OutputFilename )
        );
        throw CannotSaveCsvFileException;
      }

    }

    /**************************************************************************/

  }

}
