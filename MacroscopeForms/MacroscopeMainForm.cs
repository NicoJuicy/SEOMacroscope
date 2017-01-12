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
using System.Windows.Forms;
using System.Threading;

namespace SEOMacroscope
{

	public partial class MacroscopeMainForm : Form
	{
		
		/**************************************************************************/

		Thread ThreadScanner;

		MacroscopeJobMaster msJobMaster;

		Boolean StartUrlDirty;

		MacroscopeDisplayStructure msDisplayStructure;
		MacroscopeDisplayCanonical msDisplayCanonical;
		MacroscopeDisplayHrefLang msDisplayHrefLang;
		MacroscopeDisplayEmailAddresses msDisplayEmailAddresses;
		MacroscopeDisplayTelephoneNumbers msDisplayTelephoneNumbers;
		MacroscopeDisplayHistory msDisplayHistory;

			
		/**************************************************************************/

		public MacroscopeMainForm ()
		{
			
			InitializeComponent();// The InitializeComponent() call is required for Windows Forms designer support.

			MacroscopePreferencesManager.LoadPreferences();
						
			msJobMaster = new MacroscopeJobMaster ( this );

			StartUrlDirty = false;
			
			msDisplayStructure = new MacroscopeDisplayStructure ( this );
			msDisplayCanonical = new MacroscopeDisplayCanonical ( this );
			msDisplayHrefLang = new MacroscopeDisplayHrefLang ( this );
			msDisplayEmailAddresses = new MacroscopeDisplayEmailAddresses ( this );
			msDisplayTelephoneNumbers = new MacroscopeDisplayTelephoneNumbers ( this );
			msDisplayHistory = new MacroscopeDisplayHistory ( this );

			this.textBoxStartUrl.Text = MacroscopePreferencesManager.GetStartUrl();

			#if DEBUG
			//this.textBoxStartUrl.Text = Environment.GetEnvironmentVariable( "seomacroscope_scan_url" );
			#endif

			this.ScanningControlsEnable( true );

		}
		
		/**************************************************************************/

		~MacroscopeMainForm ()
		{
			debug_msg( "MacroscopeMainForm DESTRUCTOR CALLED" );
			this.Cleanup();
		}
		
		/**************************************************************************/

		void Cleanup ()
		{
			
			debug_msg( "MacroscopeMainForm Cleanup CALLED" );
						
			MacroscopePreferencesManager.SavePreferences();
						
			if( this.ThreadScanner != null ) {
				debug_msg( "Cleaning up ThreadScanner" );
				this.ThreadScanner.Abort();
			}

			if( this.msJobMaster != null ) {
				this.msJobMaster.WorkerUpdateDisplayShutdown();
				this.msJobMaster = null;
			}

		}

		/**************************************************************************/

		public ListView GetDisplayStructure ()
		{
			return( this.listViewStructure );
		}

		/**************************************************************************/
				
		public ListView GetDisplayCanonicalAnalysis ()
		{
			return( this.listViewCanonicalAnalysis );
		}

		/**************************************************************************/

		public ListView GetDisplayHrefLang ()
		{
			return( this.listViewHrefLang );
		}

		/**************************************************************************/
		
		public ListView GetDisplayEmailAddresses ()
		{
			return( this.listViewEmailAddresses );
		}
		
		/**************************************************************************/

		public ListView GetDisplayTelephoneNumbers ()
		{
			return( this.listViewTelephoneNumbers );
		}

		/**************************************************************************/

		public ListView GetDisplayHistory ()
		{
			return( this.listViewHistory );
		}

		/**************************************************************************/
				
		public string GetURL ()
		{
			return( this.textBoxStartUrl.Text );
		}

		/**************************************************************************/

		public void SetURL ( string sURL )
		{
			this.textBoxStartUrl.Text = sURL;
		}

		/**************************************************************************/

		void CallbackFormClosing ( object sender, FormClosingEventArgs e )
		{
			this.Cleanup();
		}

		/** MAIN MENU *************************************************************/

		void CallbackFileExit ( object sender, EventArgs e )
		{

			debug_msg( "CallbackFileExit Called" );

			this.Cleanup();

			Program.Exit();

		}

		/**************************************************************************/

		void CallbackEditPreferencesClick ( object sender, EventArgs e )
		{
			// TODO: Implement this
			
			debug_msg( "CallbackEditPreferencesClick Called" );
			
			Form fPreferencesForm = new Form ();
			
			
			fPreferencesForm.Show();
			
		}

		/** DIALOGUE BOXES ********************************************************/
		
		void DialogueBoxError ( string sTitle, string sMessage )
		{
			MessageBox.Show(
				sMessage,
				sTitle,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error,
				MessageBoxDefaultButton.Button1
			);
		}

		void DialogueBoxStartUrlInvalid ()
		{
			DialogueBoxError( "Error", "Please enter a valid URL" );
		}

		/** MAIN CONTROL STRIP CALLBACKS ******************************************/
				
		void CallbackStartUrlTextChanged ( object sender, EventArgs e )
		{

			string sStartUrl = this.textBoxStartUrl.Text;
			StartUrlDirty = true;

			if( MacroscopeURLTools.ValidateUrl( sStartUrl ) ) {
				MacroscopePreferencesManager.SetStartUrl( sStartUrl );
			}

		}

		/**************************************************************************/
				
		void CallbackScanStart ( object sender, EventArgs e )
		{

			string sStartUrl = this.textBoxStartUrl.Text;
						
			if( MacroscopeURLTools.ValidateUrl( sStartUrl ) ) {

				this.ScanningControlsStart( true );

				if( StartUrlDirty ) {
					this.msJobMaster.WorkerUpdateDisplayShutdown();
					this.msJobMaster = new MacroscopeJobMaster ( this );
					this.ClearDisplay();
					StartUrlDirty = false;
				}

				MacroscopePreferencesManager.SetStartUrl( sStartUrl );

				MacroscopePreferencesManager.SavePreferences();

				this.ThreadScanner = new Thread ( new ThreadStart ( this.ScanningThread ) );
				this.ThreadScanner.Start();

			} else {
				
				DialogueBoxStartUrlInvalid();
				
			}

		}

		/**************************************************************************/
		
		void CallbackScanStop ( object sender, EventArgs e )
		{

			this.ScanningControlsStopping( true );
			
			this.msJobMaster.WorkersStop();

			while( this.msJobMaster.RunningThreadsCount() > 0 ) {
				debug_msg( "CallbackScanStop: WAITING" );
				Thread.Sleep( 100 );
			}
			
			this.ScanningControlsStopped( true );
	
		}

		/**************************************************************************/

		void CallbackScanReset ( object sender, EventArgs e )
		{

			if( this.msJobMaster.WorkersStopped() ) {

				this.ScanningControlsReset( true );

				this.msJobMaster.WorkerUpdateDisplayShutdown();

				this.msJobMaster = new MacroscopeJobMaster ( this );

				this.ClearDisplay();
				
			}
			
		}
		
		/**************************************************************************/
		
		public void CallbackScanComplete ()
		{
			if( this.InvokeRequired ) {
				this.Invoke(
					new MethodInvoker (
						delegate
						{
							this.UpdateStatusBar();
							this.ScanningControlsComplete( true );
						}
					)
				);
			} else {
				this.UpdateStatusBar();
				this.ScanningControlsComplete( true );
			}
		}

		/** TAB PAGES *************************************************************/

		void CallbackTabPageStructureOverviewShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageStructureOverviewShow" );
			this.macroscopeDocumentDetailsMain.ClearData();
			this.msDisplayStructure.RefreshData( this.msJobMaster.DocCollectionGet() );
		}

		void CallbackListViewStructureOverviewClick ( object sender, EventArgs e )
		{
			ListView lvListView = ( ListView )sender;
			lock( lvListView ) {
				foreach( ListViewItem lvItem in lvListView.SelectedItems ) {
					string sURL = lvItem.SubItems[ 0 ].Text.ToString();
					this.macroscopeDocumentDetailsMain.UpdateDisplay( this.msJobMaster, sURL );
				}
			}
		}

		void CallbackTabPageCanonicalAnalysisShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageCanonicalAnalysisShow" );
			this.msDisplayCanonical.RefreshData( this.msJobMaster.DocCollectionGet() );
		}

		void CallbackTabPageHrefLangAnalysisShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageHrefLangAnalysisShow" );
			this.msDisplayHrefLang.RefreshData( this.msJobMaster.DocCollectionGet(), msJobMaster.LocalesGet() );
		}

		void CallbackTabPageRedirectsAuditShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageRedirectsAuditShow" );
		}

		void CallbackTabPageEmailAddressesShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageEmailAddressesShow" );
			this.msDisplayEmailAddresses.RefreshData( this.msJobMaster.DocCollectionGet() );
		}

		void CallbackTabPageTelephoneNumbersShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageTelephoneNumbersShow" );
			this.msDisplayTelephoneNumbers.RefreshData( this.msJobMaster.DocCollectionGet() );
		}

		void CallbackTabPageHistoryShow ( object sender, EventArgs e )
		{
			debug_msg( "EVENT: CallbackTabPageHistoryShow" );
			this.msDisplayHistory.RefreshData( this.msJobMaster.HistoryGet() );
		}

		/** Scanning Controls *****************************************************/

		void ScanningControlsEnable ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = true;
			this.ButtonStart.Enabled = true;
			this.ButtonStop.Enabled = false;
			this.ButtonReset.Enabled = false;
		}

		void ScanningControlsStart ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = false;
			this.ButtonStart.Enabled = false;
			this.ButtonStop.Enabled = true;
			this.ButtonReset.Enabled = false;
		}

		void ScanningControlsStopping ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = false;
			this.ButtonStart.Enabled = false;
			this.ButtonStop.Enabled = false;
			this.ButtonReset.Enabled = false;
		}

		void ScanningControlsStopped ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = true;
			this.ButtonStart.Enabled = true;
			this.ButtonStop.Enabled = false;
			this.ButtonReset.Enabled = true;
		}

		void ScanningControlsPause ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = false;
			this.ButtonStart.Enabled = false;
			this.ButtonStop.Enabled = true;
			this.ButtonReset.Enabled = false;
		}

		void ScanningControlsResume ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = false;
			this.ButtonStart.Enabled = false;
			this.ButtonStop.Enabled = true;
			this.ButtonReset.Enabled = false;
		}

		void ScanningControlsReset ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = true;
			this.ButtonStart.Enabled = true;
			this.ButtonStop.Enabled = false;
			this.ButtonReset.Enabled = false;
		}
		
		void ScanningControlsComplete ( Boolean bState )
		{
			this.textBoxStartUrl.Enabled = true;
			this.ButtonStart.Enabled = true;
			this.ButtonStop.Enabled = false;
			this.ButtonReset.Enabled = true;
		}

		/**************************************************************************/

		void ScanningThread ()
		{
			debug_msg( "Scanning Thread: Started." );
			this.msJobMaster.StartUrl = this.GetURL();
			this.msJobMaster.Execute();
			debug_msg( "Scanning Thread: Done." );
		}

		/**************************************************************************/

		public void ClearDisplay ()
		{

			this.msDisplayStructure.ClearData();
			
			this.msDisplayCanonical.ClearData();
			
			this.msDisplayHrefLang.ClearData();
			
			this.msDisplayEmailAddresses.ClearData();
			
			this.msDisplayTelephoneNumbers.ClearData();
			
			this.msDisplayHistory.ClearData();

			this.macroscopeDocumentDetailsMain.ClearData();

		}

		/**************************************************************************/

		public void UpdateDisplaySingle ( string sURL )
		{

			if( this.msJobMaster != null ) {
				
				this.msDisplayStructure.RefreshDataSingle( this.msJobMaster.DocCollectionGet().Get( sURL ), sURL );

				this.msDisplayCanonical.RefreshDataSingle( this.msJobMaster.DocCollectionGet().Get( sURL ), sURL );
						
				this.msDisplayEmailAddresses.RefreshDataSingle( this.msJobMaster.DocCollectionGet().Get( sURL ), sURL );
						
				this.msDisplayTelephoneNumbers.RefreshDataSingle( this.msJobMaster.DocCollectionGet().Get( sURL ), sURL );

			}
			
		}

		/**************************************************************************/

		public void UpdateDisplayHrefLang ()
		{

			debug_msg( string.Format( "UpdateDisplayHrefLang: {0}", "CALLED" ) );

			if( this.msJobMaster != null ) {
			
				this.msDisplayHrefLang.RefreshData( this.msJobMaster.DocCollectionGet(), this.msJobMaster.LocalesGet() );

				if( this.InvokeRequired ) {
					this.Invoke(
						new MethodInvoker (
							delegate
							{
								this.Refresh();
								this.Update();
							}
						)
					);
				} else {
					this.Refresh();
					this.Update();
				}

			}
			
		}

		/**************************************************************************/

		public void UpdateStatusBar ()
		{
			if( this.msJobMaster != null ) {

				string sThreads = string.Format( "Threads: {0}", this.msJobMaster.RunningThreadsCount().ToString() );
				string sUrlCount = string.Format( "URLs in Queue: {0}", this.msJobMaster.UrlQueueCount().ToString() );
				string sUrlsFound = string.Format( "URLs Found: {0}", this.msJobMaster.DocCollectionGet().Count() );

				if( this.InvokeRequired ) {
					this.Invoke(
						new MethodInvoker (
							delegate
							{
								this.toolStripThreads.Text = sThreads;
								this.toolStripUrlCount.Text = sUrlCount;
								this.toolStripFound.Text = sUrlsFound;
							}
						)
					);
				} else {
					this.toolStripThreads.Text = sThreads;
					this.toolStripUrlCount.Text = sUrlCount;
					this.toolStripFound.Text = sUrlsFound;
				}
			
			}
			
		}

		/**************************************************************************/
		
		void CallbackSaveOverviewExcelReport ( object sender, EventArgs e )
		{
			SaveFileDialog Dialog = new SaveFileDialog ();
			Dialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
			Dialog.FilterIndex = 2;
			Dialog.RestoreDirectory = true;
			Dialog.DefaultExt = "xlsx";
			Dialog.AddExtension = true;
			if( Dialog.ShowDialog() == DialogResult.OK ) {
				string sPath = Dialog.FileName;
				MacroscopeExcelOverviewReport msExcelReport = new MacroscopeExcelOverviewReport ();
				msExcelReport.WriteXslx( this.msJobMaster, sPath );
			}
			Dialog.Dispose();
		}
		
		/**************************************************************************/
				
		void CallbackSaveHrefLangExcelReport ( object sender, EventArgs e )
		{
			SaveFileDialog Dialog = new SaveFileDialog ();
			Dialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
			Dialog.FilterIndex = 2;
			Dialog.RestoreDirectory = true;
			Dialog.DefaultExt = "xlsx";
			Dialog.AddExtension = true;
			if( Dialog.ShowDialog() == DialogResult.OK ) {
				string sPath = Dialog.FileName;
				MacroscopeExcelHrefLangReport msExcelReport = new MacroscopeExcelHrefLangReport ();
				msExcelReport.WriteXslx( this.msJobMaster, sPath );
			}
			Dialog.Dispose();
		}

		/**************************************************************************/
	
		static void debug_msg ( String sMsg )
		{
			System.Diagnostics.Debug.WriteLine( sMsg );
		}

		static void debug_msg ( String sMsg, int iOffset )
		{
			String sMsgPadded = new String ( ' ', iOffset * 2 ) + sMsg;
			System.Diagnostics.Debug.WriteLine( sMsgPadded );
		}

		

		



		


		/**************************************************************************/
			
	}
	
}
