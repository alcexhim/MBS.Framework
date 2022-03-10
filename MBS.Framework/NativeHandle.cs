using System;
namespace MBS.Framework
{
	public class NativeHandle
	{
	}
	public class NativeHandle<THandle> : NativeHandle
	{
		public THandle Handle { get; private set; }

		public NativeHandle(THandle handle)
		{
			Handle = handle;
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if ((object)obj == null && (object)this == null)
				return true;
			if ((object)obj == null || (object)this == null)
				return false;

			if (obj is NativeHandle<THandle>)
				return Equals(obj as NativeHandle<THandle>);

			return false;
		}

		public static bool operator ==(NativeHandle<THandle> left, NativeHandle<THandle> right)
		{
			if ((object)left == null && (object)right == null)
				return true;
			if ((object)left == null || (object)right == null)
				return false;

			return left.Equals(right);
		}
		public static bool operator !=(NativeHandle<THandle> left, NativeHandle<THandle> right)
		{
			return !left.Equals(right);
		}
	}
}
