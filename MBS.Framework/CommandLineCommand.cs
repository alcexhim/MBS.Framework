//
//  CommandLineCommand.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2022 Mike Becker's Software
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
using MBS.Framework.Collections.Generic;

namespace MBS.Framework
{
	public class CommandLineCommand
	{
		public class CommandLineCommandCollection
			: System.Collections.ObjectModel.Collection<CommandLineCommand>
		{
			public CommandLineCommand this[string name]
			{
				get
				{
					foreach (CommandLineCommand item in this)
					{
						if (item.Name == name)
							return item;
					}
					return null;
				}
			}
		}

		public string Name { get; set; } = null;
		public string Description { get; set; } = null;

		public CommandLineOption.CommandLineOptionCollection Options { get; } = new CommandLineOption.CommandLineOptionCollection();

		public CommandLineCommand(string command, CommandLineOption[] options = null)
		{
			Name = command;
			if (options != null)
			{
				Options.AddRange(options);
			}
		}
	}
}
