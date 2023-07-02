using System;
using System.Text;

namespace MBS.Framework
{
	public static class StringBuilderExtensions
	{
		public static void AppendLineIndented(this StringBuilder sb, object value, int indentLevel, char indentChar = '\t', int indentCharCountPerLevel = 1)
		{
			for (int i = 0; i < indentLevel * indentCharCountPerLevel; i++)
			{
				sb.Append(indentChar);
			}
			sb.Append(value);
			sb.AppendLine();
		}
	}
}
