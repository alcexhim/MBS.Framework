using System;
namespace MBS.Framework.Scripting.Strings.StringComponents
{
	public class LiteralStringComponent : StringComponent
	{
		public string Value { get; set; } = null;

		public override string ToString(ScriptEnvironment environment)
		{
			return Value;
		}
		public override string ToString()
		{
			return String.Format("\"{0}\"", Value);
		}

		public LiteralStringComponent(string value)
		{
			Value = value;
		}

		public override object Clone()
		{
			return new LiteralStringComponent(Value?.Clone() as string);
		}
	}
}
