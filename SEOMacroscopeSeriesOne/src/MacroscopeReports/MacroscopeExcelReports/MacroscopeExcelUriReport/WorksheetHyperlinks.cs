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
using ClosedXML.Excel;

namespace SEOMacroscope
{

  public partial class MacroscopeExcelUriReport : MacroscopeExcelReports
  {

    /**************************************************************************/

    private void BuildWorksheetPageHyperlinks (
      MacroscopeJobMaster JobMaster,
      XLWorkbook wb,
      string WorksheetLabel
    )
    {

      var ws = wb.Worksheets.Add( WorksheetLabel );

      int iRow = 1;
      int iCol = 1;
      int iColMax = 1;

      MacroscopeDocumentCollection DocCollection = JobMaster.GetDocCollection();
      MacroscopeAllowedHosts AllowedHosts = JobMaster.GetAllowedHosts();

      {

        ws.Cell( iRow, iCol ).Value = "Source URL";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Target URL";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Follow";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Target";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Anchor Text";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Title";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Alt Text";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Raw Target URL";

      }

      iColMax = iCol;

      iRow++;

      foreach( MacroscopeDocument msDoc in DocCollection.IterateDocuments() )
      {

        MacroscopeHyperlinksOut HyperlinksOut = msDoc.GetHyperlinksOut();

        foreach( MacroscopeHyperlinkOut HyperlinkOut in HyperlinksOut.IterateLinks() )
        {

          string HyperlinkOutUrl = HyperlinkOut.GetTargetUrl();
          string DoFollow = "No Follow";
          string LinkTarget = HyperlinkOut.GetLinkTarget();
          string AnchorText = HyperlinkOut.GetAnchorText();
          string Title = HyperlinkOut.GetTitle();
          string AltText = HyperlinkOut.GetAltText();

          string RawTargetUrl = HyperlinkOut.GetRawTargetUrl();

          if( HyperlinkOutUrl == null )
          {
            HyperlinkOutUrl = "";
          }

          if( HyperlinkOut.GetDoFollow() )
          {
            DoFollow = "Follow";
          }

          iCol = 1;

          this.InsertAndFormatUrlCell( ws, iRow, iCol, msDoc );

          if( AllowedHosts.IsInternalUrl( Url: msDoc.GetUrl() ) )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Gray );
          }

          iCol++;

          this.InsertAndFormatUrlCell( ws, iRow, iCol, HyperlinkOutUrl );

          if( ( HyperlinkOutUrl.Length > 0 ) && ( AllowedHosts.IsInternalUrl( Url: HyperlinkOutUrl ) ) )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }
          else
          if( ( HyperlinkOutUrl.Length > 0 ) && ( AllowedHosts.IsExternalUrl( Url: HyperlinkOutUrl ) ) )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Gray );
          }
          else
          {
            this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( HyperlinkOutUrl ) );
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
          }

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, DoFollow );

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, LinkTarget );

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( AnchorText ) );

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( Title ) );

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( AltText ) );

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, RawTargetUrl );

          iRow++;

        }

      }

      {
        var rangeData = ws.Range( 1, 1, iRow - 1, iColMax );
        var excelTable = rangeData.CreateTable();
      }

    }

    /**************************************************************************/

  }

}
