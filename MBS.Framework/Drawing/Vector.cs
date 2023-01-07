using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBS.Framework.Drawing
{
	public struct Vector2D : IEquatable<Vector2D>
	{
		private bool _IsNotEmpty;
		public bool IsEmpty { get { return !_IsNotEmpty; } }

		public static Vector2D Empty = new Vector2D();

		public double X { get; set; }
		public double Y { get; set; }

		public Vector2D(double x, double y)
		{
			X = x;
			Y = y;
			_IsNotEmpty = true;
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", X, Y);
		}

		#region IEquatable implementation

		public bool Equals(Vector2D other)
		{
			return (this.X.Equals(other.X) && this.Y.Equals(other.Y));
		}

		public static bool operator ==(Vector2D left, Vector2D right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Vector2D left, Vector2D right)
		{
			return !left.Equals(right);
		}
		public override bool Equals(object obj)
		{
			if (obj is Vector2D)
			{
				Vector2D other = (Vector2D)obj;
				return X.Equals(other.X) && Y.Equals(other.Y) && IsEmpty.Equals(other.IsEmpty);
			}
			return base.Equals(obj);
		}

		#endregion

	}
}
