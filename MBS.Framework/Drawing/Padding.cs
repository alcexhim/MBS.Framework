using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBS.Framework.Drawing
{
	public struct Padding
	{
		public static Padding Empty;

		private bool _IsNotEmpty;
		public bool IsEmpty { get { return !_IsNotEmpty; } }

		private int mvarLeft;
		public int Left { get { return mvarLeft; } set { mvarLeft = value; } }
		private int mvarTop;
		public int Top { get { return mvarTop; } set { mvarTop = value; } }
		private int mvarRight;
		public int Right { get { return mvarRight; } set { mvarRight = value; } }
		private int mvarBottom;
		public int Bottom { get { return mvarBottom; } set { mvarBottom = value; } }

		public override bool Equals(object obj)
		{
			if (obj is Padding)
			{
				return (Padding)obj == this;
			}
			return false;
		}
		public static bool operator==(Padding left, Padding right)
		{
			return ((left.IsEmpty == right.IsEmpty)
				&& (left.Left == right.Left)
				&& (left.Right == right.Right)
				&& (left.Top == right.Top)
				&& (left.Bottom == right.Bottom));
		}
		public static bool operator !=(Padding left, Padding right)
		{
			return (!((left.IsEmpty == right.IsEmpty)
				&& (left.Left == right.Left)
				&& (left.Right == right.Right)
				&& (left.Top == right.Top)
				&& (left.Bottom == right.Bottom)));
		}

		public int All
		{
			get
			{
				if (mvarLeft == mvarTop && mvarTop == mvarRight && mvarRight == mvarBottom) return mvarLeft;
				return -1;
			}
			set
			{
				mvarLeft = value;
				mvarTop = value;
				mvarRight = value;
				mvarBottom = value;
			}
		}

		public Padding(int all)
		{
			mvarBottom = all;
			mvarLeft = all;
			mvarRight = all;
			mvarTop = all;
			_IsNotEmpty = true;
		}
		public Padding(int top, int bottom, int left, int right)
		{
			mvarBottom = bottom;
			mvarLeft = left;
			mvarRight = right;
			mvarTop = top;
			_IsNotEmpty = true;
		}
	}
}
