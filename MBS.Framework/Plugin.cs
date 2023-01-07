//
//  Plugin.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace MBS.Framework
{
	public class Plugin
	{
		public virtual string Title { get; set; } = null;
		public Feature.FeatureCollection ProvidedFeatures { get; } = new Feature.FeatureCollection();

		public bool Initialized { get; private set; } = false;
		public void Initialize()
		{
			if (Initialized)
				return;

			InitializeInternal();
			Initialized = true;
		}

		public Guid ID { get; set; } = Guid.Empty;

		protected virtual void InitializeInternal()
		{
			// this method intentionally left blank
		}

		protected virtual bool IsSupportedInternal()
		{
			return true;
		}
		public bool IsSupported()
		{
			return IsSupportedInternal();
		}

		public Plugin()
		{
		}
	}
}
