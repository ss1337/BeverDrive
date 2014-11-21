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

namespace BeverDrive.Gui.Core
{
	public class Playlist : List<PlaylistItem>
	{
		private List<string> filenames;

		private int currentIndex;
		public int CurrentIndex
		{
			get
			{
				return currentIndex;
			}

			set
			{
				currentIndex = value;

				if (currentIndex > this.Count - 1)
					currentIndex = 0;

				if (currentIndex < 0)
					currentIndex = 0;
			}
		}

		public PlaylistItem CurrentItem
		{
			get
			{
				if (this.Count > this.CurrentIndex)
					return this[this.CurrentIndex];

				return null;
			}
		}

		public Playlist()
		{
			this.filenames = new List<string>();
		}

		public void AddFile(string filename)
		{
			this.AddFile(filename, true);
		}

		public void AddFile(string filename, bool addToVlc)
		{
			var item = new PlaylistItem(filename, addToVlc);
			this.Add(item);
			this.filenames.Add(item.Filename);
		}

		public void Shuffle()
		{
			PlaylistItem item;
			Random rng = new Random();
			int n = this.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);

				if (n != currentIndex && k != currentIndex)
				{
					item = this[k];
					this[k] = this[n];
					this[n] = item;
				}
			}
		}

		public void Unshuffle()
		{
			PlaylistItem item;
			int i = 0;
			int n = this.Count;
			while (n > 1)
			{
				n--;
				if (n > 0 && n != currentIndex && this[n].Filename != this.filenames[n])
				{
					// Find the item
					for (i = 0; i < n; i++)
					{
						if (this[i].Filename == this.filenames[n])
						{
							item = this[n];
							this[n] = this[i];
							this[i] = item;
						}
					}
				}
			}
		}
	}
}
