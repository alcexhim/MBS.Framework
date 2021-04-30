﻿//
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
	public abstract class CommandLine
	{
		/// <summary>
		/// Gets the original <see cref="String" /> array of arguments.
		/// </summary>
		/// <value>The arguments.</value>
		public string[] Arguments { get; protected set; }

		/// <summary>
		/// Gets the list of file names passed on the command line.
		/// </summary>
		/// <value>The file names.</value>
		public List<string> FileNames { get; } = new List<string>();

		public CommandLineOption.CommandLineOptionCollection Options { get; } = new CommandLineOption.CommandLineOptionCollection();

		protected CommandLine()
		{
		}
		protected internal CommandLine(string[] arguments)
		{
			this.Arguments = arguments;
		}

		public override string ToString()
		{
			return String.Join(" ", Arguments);
		}
	}
}
