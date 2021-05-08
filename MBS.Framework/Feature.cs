//
//  Feature.cs
//
//  Author:
//       Mike Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker
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
	public class Feature
	{
		public class FeatureCollection
			: System.Collections.ObjectModel.Collection<Feature>
		{
		}

		public Guid ID { get; private set; } = Guid.Empty;
		public string Title { get; private set; } = null;

		public Feature(Guid id, string title)
		{
			ID = id;
			Title = title;
		}

		public override bool Equals(object obj)
		{
			if (!(this is null) && (obj is null)) return false;
			if (this is null && !(obj is null)) return false;

			if (obj is Feature)
			{
				Feature feat = (obj as Feature);
				return feat.ID.Equals(ID);
			}
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Feature left, Feature right)
		{
			return (left.ID == right.ID);
		}
		public static bool operator !=(Feature left, Feature right)
		{
			return (left.ID != right.ID);
		}
	}
}
