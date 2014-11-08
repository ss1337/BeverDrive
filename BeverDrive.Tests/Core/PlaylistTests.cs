//
// Copyright 2012-2014 Sebastian Sjödin
//
// This file is part of BeverDrive.
//
// BeverDrive is free software: you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option)
// any later version.
//
// BeverDrive is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// BeverDrive. If not, see http://www.gnu.org/licenses/.
//
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeverDrive.Gui.Core;
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture]
	public class PlaylistTests
	{
		public PlaylistTests()
		{
		}

		[Test]
		public void Adding_files_works()
		{
			int i = 0;
			int num = 10;
			string format = "C:\\Mp3\\0{0}-asdf.mkv";
			var pl = new Playlist();
			for (i = 1; i < num; i++)
			{
				pl.AddFile(string.Format(format, i), false);
			}

			Assert.AreEqual(num - 1, pl.Count);
			for (i = 1; i < num; i++)
				Assert.True(string.Format(format, i).Contains(pl[i-1].Filename));
		}

		[Test]
		public void Shuffling_works()
		{
			int i = 0;
			int num = 10;
			string format1 = "C:\\Mp3\\0{0}-asdf.mkv";
			string format2 = "0{0}-asdf.mkv";
			var pl = new Playlist();
			for (i = 1; i < num; i++)
				pl.AddFile(string.Format(format1, i));

			pl.Shuffle();

			// Actual test
			bool consecutive = true;
			for (int j = 0; j < pl.Count - 1; j++)
			{
				for (i = 1; i < num - 1; i++)
				{
					string filename1 = string.Format(format2, i);
					string filename2 = string.Format(format2, i+1);
					if (pl[j].Filename != filename1 && pl[j + 1].Filename != filename2)
						consecutive = false;
				}
			}

			Assert.False(consecutive);
		}

		[Test]
		public void Shuffling_doesnt_change_index()
		{
			int i = 0;
			int num = 10;
			string format1 = "C:\\Mp3\\0{0}-asdf.mkv";
			var pl = new Playlist();
			for (i = 1; i < num; i++)
				pl.AddFile(string.Format(format1, i));

			pl.CurrentIndex = 4;
			string currentFilename = pl.CurrentItem.Filename;
			int currentIndex = 4;

			pl.Shuffle();

			Assert.AreEqual(currentFilename, pl.CurrentItem.Filename);
			Assert.AreEqual(currentIndex, pl.CurrentIndex);
		}

		[Test]
		public void Shuffling_keeping_track_at_index_works()
		{
			int i = 0;
			int num = 10;
			string format1 = "C:\\Mp3\\0{0}-asdf.mkv";
			string format2 = "0{0}-asdf.mkv";
			var pl = new Playlist();
			for (i = 1; i < num; i++)
				pl.AddFile(string.Format(format1, i));

			pl.CurrentIndex = 4;
			string currentFilename = pl.CurrentItem.Filename;

			pl.Shuffle();

			// Actual test
			bool consecutive = true;
			for (int j = 0; j < pl.Count - 1; j++)
			{
				for (i = 1; i < num - 1; i++)
				{
					string filename1 = string.Format(format2, i);
					string filename2 = string.Format(format2, i + 1);
					if (pl[j].Filename != filename1 && pl[j + 1].Filename != filename2)
						consecutive = false;
				}
			}
			Assert.AreEqual(currentFilename, pl.CurrentItem.Filename);
			Assert.False(consecutive);
		}

		[Test]
		public void UnShuffling_keeping_track_at_index_works()
		{
			int i = 0;
			int num = 10;
			string format1 = "C:\\Mp3\\0{0}-asdf.mkv";
			string format2 = "0{0}-asdf.mkv";
			var pl = new Playlist();
			for (i = 1; i < num; i++)
				pl.AddFile(string.Format(format1, i));

			pl.CurrentIndex = 4;
			string currentFilename = pl.CurrentItem.Filename;

			pl.Shuffle();
			pl.Unshuffle();

			// Actual test
			bool consecutive = false;
			for (int j = 0; j < pl.Count - 1; j++)
			{
				string filename1 = string.Format(format2, j + 1);
				string filename2 = string.Format(format2, j + 2);
				if (pl[j].Filename == filename1 && pl[j + 1].Filename == filename2)
					consecutive = true;
			}
			Assert.AreEqual(currentFilename, pl.CurrentItem.Filename);
			Assert.True(consecutive);
		}
	}
}
