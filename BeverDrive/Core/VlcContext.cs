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
using nVlc.LibVlcWrapper.Declarations;
using nVlc.LibVlcWrapper.Declarations.Players;
using nVlc.LibVlcWrapper.Implementation;
using System.IO;

namespace BeverDrive.Gui.Core
{
	public static class VlcContext
	{
		/// <summary>
		/// Which disc is currently playing, usually 6
		/// </summary>
		public static int CurrentDisc { get; set; }

		/// <summary>
		/// Current track playing, between 1 and 99
		/// </summary>
		private static int currentTrack;
		public static int CurrentTrack
		{
			get { return currentTrack; }
			set
			{ 
				currentTrack = value;
				if (currentTrack > 99) { currentTrack = 99; }
				if (currentTrack < 1) { currentTrack = 1; }
			}
		}

		public static bool IsPlaying { get { return AudioPlayer.IsPlaying || VideoPlayer.IsPlaying; } }

		/// <summary>
		/// Vlc Factory yada yada
		/// </summary>
		public static IMediaPlayerFactory Factory { get; private set; }

		/// <summary>
		/// Player 1, this one is the actual sound
		/// </summary>
		public static IAudioPlayer AudioPlayer { get; private set; }

		/// <summary>
		/// Player 2, this one is for the visualization stuff
		/// </summary>
		public static IAudioPlayer VizPlayer { get; private set; }

		/// <summary>
		/// Video player, this is used for video
		/// </summary>
		public static IVideoPlayer VideoPlayer { get; private set; }

		static VlcContext()
		{
		}

		public static void Initialize(string libvlcPath)
		{
			CurrentDisc = 6;
			CurrentTrack = 1;

			Factory = new MediaPlayerFactory(libvlcPath);
			//m_player = m_factory.CreatePlayer<IDiskPlayer>();
			AudioPlayer = Factory.CreatePlayer<IAudioPlayer>();
			VizPlayer = Factory.CreatePlayer<IAudioPlayer>();
			VideoPlayer = Factory.CreatePlayer<IVideoPlayer>();
		}
	}
}
