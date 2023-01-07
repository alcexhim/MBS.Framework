using System;

namespace MBS.Framework.Scripting.Strings
{
	public abstract class StringComponent : ICloneable
	{
		public class StringComponentCollection
			: System.Collections.ObjectModel.Collection<StringComponent>
		{
		}

		public abstract string ToString(ScriptEnvironment environment);

		public abstract object Clone();
	}
}
