//
//  ArrayExtensions.cs
//
//  Author:
//       Mike Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019 Mike Becker
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
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace MBS.Framework
{
	public static class MemsetExtension
	{
		private static Action<IntPtr, byte, int> MemsetDelegate = null;

		/// <summary>
		/// Clears the specified <paramref name="array"/>, filling all values with the desired <paramref name="value"/>.
		/// </summary>
		/// <param name="array">The array to clear..</param>
		/// <param name="value">The value to set all items in the array to.</param>
		public static void Clear(this byte[] array, byte value = 0x00)
		{
			// Thanks https://stackoverflow.com/questions/1897555/what-is-the-equivalent-of-memset-in-c
			if (MemsetDelegate == null)
			{
				var dynamicMethod = new DynamicMethod("Memset", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard,
				null, new[] { typeof(IntPtr), typeof(byte), typeof(int) }, typeof(MemsetExtension), true);

				var generator = dynamicMethod.GetILGenerator();
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldarg_1);
				generator.Emit(OpCodes.Ldarg_2);
				generator.Emit(OpCodes.Initblk);
				generator.Emit(OpCodes.Ret);

				MemsetDelegate = (Action<IntPtr, byte, int>)dynamicMethod.CreateDelegate(typeof(Action<IntPtr, byte, int>));
			}

			var gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			MemsetDelegate(gcHandle.AddrOfPinnedObject(), value, array.Length);
			gcHandle.Free();

		}
	}
}
