﻿/*

  This file is part of SEOMacroscope.

  Copyright 2020 Jason Holland.

  The GitHub repository may be found at:

    https://github.com/nazuke/SEOMacroscope

  SEOMacroscope is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  SEOMacroscope is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with SEOMacroscope.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using NUnit.Framework;

namespace SEOMacroscope
{

  [TestFixture]
  public class TestMacroscopeNamedQueue : Macroscope
  {

    /**************************************************************************/

    private static string QUEUENAME = "TEST";

    /**************************************************************************/

    [Test]
    public void TestCreateNamedQueue ()
    {
      MacroscopeNamedQueue<MacroscopeJobItem> NamedQueue = new MacroscopeNamedQueue<MacroscopeJobItem>();
      NamedQueue.CreateNamedQueue( QUEUENAME );
      MacroscopeJobItem JobItem1 = new MacroscopeJobItem( "http://www.company.com/" );
      NamedQueue.AddToNamedQueue( QUEUENAME, JobItem1 );
      Assert.AreEqual( 1, NamedQueue.CountNamedQueueItems( QUEUENAME ) );
    }

    /**************************************************************************/

    [Test]
    public void TestFailAddingDuplicateToNamedQueue ()
    {
      MacroscopeNamedQueue<MacroscopeJobItem> NamedQueue = new MacroscopeNamedQueue<MacroscopeJobItem>();
      NamedQueue.CreateNamedQueue( QUEUENAME, MacroscopeNamedQueue<MacroscopeJobItem>.MODE.USE_HISTORY );
      MacroscopeJobItem JobItem1 = new MacroscopeJobItem( "http://www.company.com/" );
      MacroscopeJobItem JobItem2 = new MacroscopeJobItem( "http://www.company.com/" );
      NamedQueue.AddToNamedQueue( QUEUENAME, JobItem1 );
      NamedQueue.AddToNamedQueue( QUEUENAME, JobItem2 );
      Assert.AreEqual( 1, NamedQueue.CountNamedQueueItems( QUEUENAME ) );
    }

    /**************************************************************************/

    [Test]
    public void TestAddRemoveAddAgainToNamedQueue ()
    {

      MacroscopeNamedQueue<MacroscopeJobItem> NamedQueue = new MacroscopeNamedQueue<MacroscopeJobItem>();
      NamedQueue.CreateNamedQueue( QUEUENAME, MacroscopeNamedQueue<MacroscopeJobItem>.MODE.USE_HISTORY );

      MacroscopeJobItem JobItem1 = new MacroscopeJobItem( "http://www.company.com/" );

      NamedQueue.AddToNamedQueue( QUEUENAME, JobItem1 );

      Assert.AreEqual( 1, NamedQueue.CountNamedQueueItems( QUEUENAME ) );

      MacroscopeJobItem JobItem2 = NamedQueue.GetNamedQueueItem( QUEUENAME  );

      Assert.AreEqual( 0, NamedQueue.CountNamedQueueItems( QUEUENAME ) );

      bool Forgotten = NamedQueue.ForgetNamedQueueItem( QUEUENAME, JobItem1 );

      NamedQueue.AddToNamedQueue( QUEUENAME, JobItem1 );

      Assert.AreEqual( 1, NamedQueue.CountNamedQueueItems( QUEUENAME ) );

    }

    /**************************************************************************/

  }

}
