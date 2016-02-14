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
using System.IO;
using System.Linq;
using System.Text;
using nVlc.LibVlcWrapper.Declarations.Media;

namespace BeverDrive.Core
{
	public class PlaylistItem
	{
		public string Album { get; private set; }

		public string Artist { get; private set; }

		public string Title { get; private set; }

		public string Filename { get; private set; }

		public IMedia VlcMedia { get; private set; }

		public PlaylistItem(string filename)
		{
			this.Album = string.Empty;
			this.Artist = string.Empty;
			this.Title = string.Empty;
			this.Filename = filename.Substring(filename.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			this.VlcMedia = VlcContext.Factory.CreateMedia<IMedia>(filename);
			//this.VlcMedia.Parse(false);
			
			if (filename.EndsWith(".mp3"))
			{
				var file = TagLib.File.Create(filename);
				this.Album = file.Tag.Album;
				this.Artist = file.Tag.FirstPerformer;
				this.Title = file.Tag.Title;
				file.Dispose();
			}
		}
	}
}
