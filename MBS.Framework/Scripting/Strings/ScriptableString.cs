using System;
using System.Text;

namespace MBS.Framework.Scripting.Strings
{
	public class ScriptableString : ICloneable
	{
		public ScriptableString(StringComponent[] components = null)
		{
			if (components != null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					Components.Add(components[i]);
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Components.Count; i++)
			{
				sb.Append(Components[i].ToString());
				if (i < Components.Count - 1)
					sb.Append(' ');
			}
			return sb.ToString();
		}
		public string ToString(ScriptEnvironment environment)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Components.Count; i++)
			{
				sb.Append(Components[i].ToString(environment));
				if (i < Components.Count - 1)
					sb.Append(' ');
			}
			return sb.ToString();
		}

		public StringComponent.StringComponentCollection Components { get; } = new StringComponent.StringComponentCollection();

		public object Clone()
		{
			ScriptableString clone = new ScriptableString();
			for (int i = 0; i < Components.Count; i++)
			{
				clone.Components.Add(Components[i].Clone() as StringComponent);
			}
			return clone;
		}
	}
}
