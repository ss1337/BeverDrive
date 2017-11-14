//
// Copyright 2012-2017 Sebastian Sjödin
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

namespace BeverDrive.Modules
{
	[AttributeUsage(AttributeTargets.Class)]
	public class BackButtonVisibleAttribute : Attribute
	{
		private bool backButtonVisible;

		public bool BackButtonVisible { get { return backButtonVisible; } }

		public BackButtonVisibleAttribute(bool visible)
		{
			this.backButtonVisible = visible;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class HandlesSelectionAttribute : Attribute
	{
		private bool handlesSelection;

		public bool HandlesSelection { get { return handlesSelection; } }

		public HandlesSelectionAttribute(bool handlesSelection)
		{
			this.handlesSelection = handlesSelection;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class MenuTextAttribute : Attribute
	{
		private string menutext;

		public string MenuText { get { return menutext; } }

		public MenuTextAttribute(string text)
		{
			this.menutext = text;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class OverlayAttribute : Attribute
	{
		private bool overlayed;

		public bool Overlayed { get { return overlayed; } }

		public OverlayAttribute(bool overlayed)
		{
			this.overlayed = overlayed;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class PlaybackModuleAttribute : Attribute
	{
		private bool playback;

		public bool Playback { get { return playback; } }

		public PlaybackModuleAttribute()
		{
			this.playback = true;
		}
	}
}
