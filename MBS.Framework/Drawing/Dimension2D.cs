using System;

namespace MBS.Framework.Drawing
{
	public class Dimension2D : Dimension, IEquatable<Dimension2D>
	{
		private double mvarWidth = 0.0;
		public double Width { get { return mvarWidth; } set { mvarWidth = value; } }
		private double mvarHeight = 0.0;
		public double Height { get { return mvarHeight; } set { mvarHeight = value; } }

		public static Dimension2D Empty = new Dimension2D();

		public Dimension2D()
		{
		}
		public Dimension2D(double width, double height)
		{
			mvarWidth = width;
			mvarHeight = height;
		}

		public Dimension2D Rotate()
		{
			Dimension2D size = this;
			double width = size.Width;
			size.Width = size.Height;
			size.Height = width;
			return size;
		}

		public override string ToString()
		{
			return Width.ToString() + "x" + Height.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is Dimension2D)
			{
				return ((Dimension2D)obj).Width.Equals(Width) && ((Dimension2D)obj).Height.Equals(Height);
			}
			return base.Equals(obj);
		}
		public bool Equals(Dimension2D other)
		{
			if ((object)other == null) return (object)this == null;
			return Width.Equals(other.Width) && Height.Equals(other.Height);
		}

		public static bool operator ==(Dimension2D left, Dimension2D right)
		{
			if ((left is null) && (right is null))
				return true;
			if ((left is null) || (right is null))
				return false;
			return left.Equals(right);
		}
		public static bool operator !=(Dimension2D left, Dimension2D right)
		{
			if ((left is null) && (right is null))
				return false;
			if ((left is null) || (right is null))
				return true;
			return !left.Equals(right);
		}
	}
}
