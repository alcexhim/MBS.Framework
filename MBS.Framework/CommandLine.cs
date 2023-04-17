//
//  CommandLine.cs
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
using System.Collections.Generic;

namespace MBS.Framework
{
	public class CommandLine
	{
		/// <summary>
		/// Gets the original <see cref="String" /> array of arguments.
		/// </summary>
		/// <value>The arguments.</value>
		public string[] Arguments { get; internal set; }

		/// <summary>
		/// Gets the list of file names passed on the command line.
		/// </summary>
		/// <value>The file names.</value>
		public List<string> FileNames { get; } = new List<string>();

		public CommandLineParser Parser { get; set; } = null;

		public string HelpTextPrefix { get; set; } = null;
		public string HelpTextSuffix { get; set; } = null;

		public CommandLineOption.CommandLineOptionCollection Options { get; } = new CommandLineOption.CommandLineOptionCollection();
		public CommandLineCommand.CommandLineCommandCollection Commands { get; } = new CommandLineCommand.CommandLineCommandCollection();
		public CommandLineCommand Command { get; internal set; } = null;
		public string ShortOptionPrefix { get; set; } = null;
		public string LongOptionPrefix { get; set; } = null;

		public CommandLine()
		{
			string[] args = Environment.GetCommandLineArgs();
			string[] args2 = new string[args.Length - 1];
			Array.Copy(args, 1, args2, 0, args2.Length);
			Arguments = args2;

			Options.Add(new CommandLineOption() { Abbreviation = 'A', Name = "activation-type", Description = "The type of activation for this app", Type = CommandLineOptionValueType.Single, Optional = true });
			Options.Add(new CommandLineOption() { Name = "help", Description = "Displays help", Type = CommandLineOptionValueType.None, Optional = true });
		}

		public override string ToString()
		{
			return String.Join(" ", Arguments);
		}
	}
}
