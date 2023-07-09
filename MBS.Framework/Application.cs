//
//  Application.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WAR+RANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Text;
using MBS.Framework.Collections.Generic;

namespace MBS.Framework
{
	public class Application
	{
		/// <summary>
		/// Implements a <see cref="Context.ContextCollection" /> which notifies
		/// the containing <see cref="Application" /> of context addition and
		/// removal.
		/// </summary>
		public class ApplicationContextCollection
			: Context.ContextCollection
		{
			protected override void ClearItems()
			{
				for (int i = 0; i < this.Count; i++)
				{
					Instance.RemoveContext(this[i]);
				}
				base.ClearItems();
			}
			protected override void InsertItem(int index, Context item)
			{
				base.InsertItem(index, item);
				Instance.AddContext(item);
			}
			protected override void RemoveItem(int index)
			{
				Instance.RemoveContext(this[index]);
				base.RemoveItem(index);
			}
		}

		/// <summary>
		/// Gets or sets the currently-running <see cref="Application" />
		/// instance.
		/// </summary>
		/// <value>
		/// The currently-running <see cref="Application" /> instance.
		/// </value>
		public static Application Instance { get; set; } = null;

		/// <summary>
		/// Searches for the file with the given <paramref name="filename" /> and
		/// returns all matching fully-qualified file names.
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="filename">Filename.</param>
		/// <param name="options">Options.</param>
		public string[] FindFiles(string filename, FindFileOptions options = FindFileOptions.All)
		{
			if (filename.StartsWith("~/"))
			{
				filename = filename.Substring(2);
			}

			List<string> files = new List<string>();
			string[] paths = EnumerateDataPaths(options);
			foreach (string path in paths)
			{
				string file = System.IO.Path.Combine(new string[] { path, filename });
				if (System.IO.File.Exists(file))
				{
					files.Add(file);
				}
			}
			return files.ToArray();
		}
		/// <summary>
		/// Searches for the file with the given <paramref name="filename" /> and
		/// returns the first matching fully-qualified file name.
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="filename">Filename.</param>
		/// <param name="options">Options.</param>
		public string FindFile(string filename, FindFileOptions options = FindFileOptions.All)
		{
			string[] files = FindFiles(filename, options);
			if (files.Length > 0)
			{
				return files[0];
			}
			if ((options & FindFileOptions.Create) == FindFileOptions.Create)
			{
				string[] paths = EnumerateDataPaths(options);
				return System.IO.Path.Combine(paths[0], filename);
			}
			return null;
		}

		/// <summary>
		/// Gets a collection of <see cref="EventFilter" />s registered for this
		/// <see cref="Application" />.
		/// </summary>
		/// <value>The event filters.</value>
		public EventFilter.EventFilterCollection EventFilters { get; } = new EventFilter.EventFilterCollection();

		/// <summary>
		/// Finds the command with the given <paramref name="commandID" />.
		/// </summary>
		protected virtual Command FindCommandInternal(string commandID)
		{
			return null;
		}
		/// <summary>
		/// Finds the command with the given <paramref name="commandID" />,
		/// searching across application-global commands as well as commands
		/// defined in the currently-loaded <see cref="Context" />s.
		/// </summary>
		/// <returns>
		/// The command with the given <paramref name="commandID" />, or
		/// <see langword="null" /> if the command was not found.
		/// </returns>
		/// <param name="commandID">Command identifier.</param>
		public Command FindCommand(string commandID)
		{
			Command cmd = Commands[commandID];
			if (cmd != null) return cmd;

			cmd = FindCommandInternal(commandID);
			if (cmd != null) return cmd;

			foreach (Context ctx in Contexts)
			{
				cmd = ctx.Commands[commandID];
				if (cmd != null) return cmd;
			}

			return null;
		}

		/// <summary>
		/// Finds the <see cref="Context" /> with the given
		/// <paramref name="contextID" />.
		/// </summary>
		protected virtual Context FindContextInternal(Guid contextID)
		{
			return null;
		}
		/// <summary>
		/// Finds the <see cref="Context" /> with the given
		/// <paramref name="contextID" />.
		/// </summary>
		public Context FindContext(Guid contextID)
		{
			Context ctx = FindContextInternal(contextID);
			if (ctx != null) return ctx;

			ctx = Contexts[contextID];
			if (ctx != null) return ctx;
			return null;
		}

		public Guid ID { get; set; } = Guid.Empty;

		protected virtual void EnableDisableCommandInternal(Command command, bool enable)
		{
		}
		internal void _EnableDisableCommand(Command command, bool enable)
		{
			EnableDisableCommandInternal(command, enable);
		}

		private string _UniqueName = null;
		public string UniqueName
		{
			get
			{
				if (_UniqueName == null)
				{
					return ShortName;
				}
				return _UniqueName;
			}
			set
			{
				_UniqueName = value;
			}
		}

		private string mvarBasePath = null;
		public string BasePath
		{
			get
			{
				if (mvarBasePath == null)
				{
					// Set up the base path for the current application. Should this be able to be
					// overridden with a switch (/basepath:...) ?
					mvarBasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				}
				return mvarBasePath;
			}
		}

		protected virtual string DataDirectoryName => ShortName;
		public string[] EnumerateDataPaths() => EnumerateDataPaths(FindFileOptions.All);
		public string[] EnumerateDataPaths(FindFileOptions options)
		{
			List<string> list = new List<string>();

			string[] dataDirectoryNames = null;
			if (AdditionalShortNames != null)
			{
				dataDirectoryNames = new string[AdditionalShortNames.Length + 1];
				dataDirectoryNames[0] = DataDirectoryName;
				for (int i = 0; i < AdditionalShortNames.Length; i++)
				{
					dataDirectoryNames[i + 1] = AdditionalShortNames[i];
				}
			}
			else
			{
				dataDirectoryNames = new string[] { DataDirectoryName };
			}

			if ((options & FindFileOptions.All) == FindFileOptions.All)
			{
				// first look in the application root directory since this will override everything else
				list.Add(BasePath);

				foreach (string dataDirName in dataDirectoryNames)
				{
					if (Environment.OSVersion.Platform == PlatformID.Unix)
					{
						// if we are on Unix or Mac OS X, look in /etc/...
						list.Add(String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
						{
							String.Empty, // *nix root directory
							"etc",
							dataDirName
						}));
					}

					// then look in /usr/share/universal-editor or C:\ProgramData\Mike Becker's Software\Universal Editor
					list.Add(String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
					{
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
						dataDirName
					}));

					// then look in ~/.local/share/universal-editor or C:\Users\USERNAME\AppData\Local\Mike Becker's Software\Universal Editor
					list.Add(String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
					{
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
						dataDirName
					}));
				}
			}



			//fixme: addd finddfileoption.userconfig, localdata, etc.
			// now for the user-writable locations...

			foreach (string dataDirName in dataDirectoryNames)
			{
				// then look in ~/.universal-editor or C:\Users\USERNAME\AppData\Roaming\Mike Becker's Software\Universal Editor
				list.Add(String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
				{
					System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
					dataDirName
				}));
			}
			return list.ToArray();
		}

		public string ShortName { get; set; }
		public virtual string[] AdditionalShortNames => null;

		public string Title { get; set; } = String.Empty;
		public int ExitCode { get; protected set; } = 0;

		protected virtual InstallationStatus GetInstallationStatusInternal()
		{
			return InstallationStatus.Unknown;
		}
		public InstallationStatus InstallationStatus { get { return GetInstallationStatusInternal(); } }

		public CommandLine CommandLine { get; } = new CommandLine();

		private Dictionary<string, List<EventHandler>> _CommandEventHandlers = new Dictionary<string, List<EventHandler>>();
		public Command.CommandCollection Commands { get; } = new Command.CommandCollection();

		/// <summary>
		/// Attachs the specified <see cref="EventHandler" /> to handle the <see cref="Command" /> with the given <paramref name="commandID" />.
		/// </summary>
		/// <returns><c>true</c>, if the command was found, <c>false</c> otherwise.</returns>
		/// <param name="commandID">Command identifier.</param>
		/// <param name="handler">Handler.</param>
		public bool AttachCommandEventHandler(string commandID, EventHandler handler)
		{
			Command cmd = Commands[commandID];
			if (cmd != null)
			{
				cmd.Executed += handler;
				return true;
			}
			Console.WriteLine("attempted to attach handler for unknown command '" + commandID + "'");

			// handle command event handlers attached without a Command instance
			if (!_CommandEventHandlers.ContainsKey(commandID))
			{
				_CommandEventHandlers.Add(commandID, new List<EventHandler>());
			}
			if (!_CommandEventHandlers[commandID].Contains(handler))
			{
				_CommandEventHandlers[commandID].Add(handler);
			}
			return false;
		}

		/// <summary>
		/// Executes the command with the given <paramref name="id" />.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="namedParameters">Named parameters.</param>
		public void ExecuteCommand(string id, KeyValuePair<string, object>[] namedParameters = null)
		{
			Command cmd = Commands[id];

			// handle command event handlers attached without a Command instance
			if (_CommandEventHandlers.ContainsKey(id))
			{
				List<EventHandler> c = _CommandEventHandlers[id];
				for (int i = 0; i < c.Count; i++)
				{
					c[i](this, new CommandEventArgs(cmd, namedParameters));
				}
				return;
			}

			// handle command event handlers attached in a context, most recently added first
			for (int i = Contexts.Count - 1; i >= 0; i--)
			{
				if (Contexts[i].ExecuteCommand(id))
					return;
			}

			if (cmd == null)
				return;

			cmd.Execute();
		}

		protected virtual void InitializeInternal()
		{
		}

		public bool Initialized { get; private set; } = false;

		[System.Diagnostics.DebuggerNonUserCode()]
		public void Initialize()
		{
			if (Initialized)
				return;

			if (ShortName == null)
				throw new ArgumentException("must specify a ShortName for the application");

			Console.CancelKeyPress += Console_CancelKeyPress;

			InitializeInternal();
			Initialized = true;
		}

		protected virtual void OnCancelKeyPress(ConsoleCancelEventArgs e)
		{
		}

		private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			OnCancelKeyPress(e);
		}


		public event ApplicationActivatedEventHandler BeforeActivated;
		protected virtual void OnBeforeActivated(ApplicationActivatedEventArgs e)
		{
			BeforeActivated?.Invoke(this, e);
		}

		public event ApplicationActivatedEventHandler Activated;
		protected virtual void OnActivated(ApplicationActivatedEventArgs e)
		{
			Activated?.Invoke(this, e);
		}


		public event ApplicationActivatedEventHandler AfterActivated;
		protected virtual void OnAfterActivated(ApplicationActivatedEventArgs e)
		{
			AfterActivated?.Invoke(this, e);
		}

		protected virtual int StartInternal()
		{
			CommandLine cline = new CommandLine();
			string[] args = CommandLine.Arguments;

			if (args.Length > 0)
			{
				int i = 0;
				for (i = 0; i < args.Length; i++)
				{
					if (ParseOption(args, ref i, cline.Options, CommandLine.Options))
						break;
				}

				// we have finished parsing the first set of options ("global" options)
				// now we see if we have commands
				if (CommandLine.Commands.Count > 0 && i < args.Length)
				{
					// we support commands like git and apt, "appname --app-global-options <command> --command-options"
					CommandLineCommand cmd = CommandLine.Commands[args[i]];
					if (cmd != null)
					{
						for (i++; i < args.Length; i++)
						{
							if (ParseOption(args, ref i, cline.Options, cmd.Options))
								break;
						}

						cline.Command = cmd;
					}
					else
					{
						// assume filename
					}
				}

				for (/* intentionally left blank */; i < args.Length; i++)
				{
					cline.FileNames.Add(args[i]);
				}
			}

			ApplicationActivatedEventArgs e = new ApplicationActivatedEventArgs(true, ApplicationActivationType.CommandLineLaunch, cline);

			if (CommandLine.Options["help"]?.Value is bool && ((bool)CommandLine.Options["help"]?.Value) == true)
			{
				if (ShowCommandLineHelp(out int resultCode))
				{
					return resultCode;
				}
			}

			if (cline.Command != null && cline.Command.ActivationDelegate != null)
			{
				OnBeforeActivated(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}

				// use the activation delegate instead of calling OnActivated
				cline.Command.ActivationDelegate(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}

				OnAfterActivated(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}
			}
			else
			{
				OnBeforeActivated(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}

				OnActivated(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}

				OnAfterActivated(e);
				if (!e.Success)
				{
					Console.WriteLine(String.Format("Try '{0} --help' for more information.", ShortName));
					return e.ExitCode;
				}
			}
			return e.ExitCode;
		}


		protected void PrintUsageStatement(CommandLineCommand command = null)
		{
			string shortOptionPrefix = CommandLine.ShortOptionPrefix ?? "-";
			string longOptionPrefix = CommandLine.LongOptionPrefix ?? "--";

			Console.Write("usage: {0} ", ShortName);
			foreach (CommandLineOption option in CommandLine.Options)
			{
				PrintUsageStatementOption(option);
			}

			if (CommandLine.HelpTextPrefix != null)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine(CommandLine.HelpTextPrefix);
			}

			Console.WriteLine();

			bool printDescriptions = true;
			if (printDescriptions)
			{
				foreach (CommandLineOption option in CommandLine.Options)
				{
					Console.WriteLine("  {0}{1}", option.Abbreviation != '\0' ? String.Format("{0}{1}, {2}{3}", shortOptionPrefix, option.Abbreviation, longOptionPrefix, option.Name) : String.Format("{0}{1}", longOptionPrefix, option.Name), option.Description == null ? null : String.Format(" - {0}", option.Description));
				}
			}
			// Console.Write("[<global-options...>]");

			if (command != null)
			{
				Console.WriteLine("  {0}{1}", command.Name, command.Description == null ? null : String.Format(" - {0}", command.Description));
				foreach (CommandLineOption option in command.Options)
				{
					PrintUsageStatementOption(option);
				}
			}
			else
			{
				if (CommandLine.Commands.Count > 0)
				{
					Console.WriteLine(" <command> [<command-options...>]");
					Console.WriteLine();

					List<CommandLineCommand> commands = new List<CommandLineCommand>(CommandLine.Commands);
					commands.Sort((x, y) =>
					{
						return x.Name.CompareTo(y.Name);
					});
					foreach (CommandLineCommand command1 in commands)
					{
						Console.WriteLine("  {0}{1}", command1.Name, command1.Description == null ? null : String.Format(" - {0}", command1.Description));
					}
				}
			}

			if (CommandLine.HelpTextSuffix != null)
			{
				Console.WriteLine();
				Console.WriteLine(CommandLine.HelpTextSuffix);
			}
		}

		private void PrintUsageStatementOption(CommandLineOption option)
		{
			Console.Write("  ");
			if (option.Optional)
			{
				Console.Write('[');
			}

			string shortOptionPrefix = CommandLine.ShortOptionPrefix ?? "-";
			string longOptionPrefix = CommandLine.LongOptionPrefix ?? "--";

			if (option.Abbreviation != '\0')
			{
				Console.Write("{0}{1}", shortOptionPrefix, option.Abbreviation);
				if (option.Type == CommandLineOptionValueType.Single)
				{
					Console.Write(" <value>");
				}
				else if (option.Type == CommandLineOptionValueType.Multiple)
				{
					Console.Write(" <value1>[,<value2>,...]");
				}

				Console.Write(" | {0}{1}", longOptionPrefix, option.Name);
				if (option.Type == CommandLineOptionValueType.Single)
				{
					Console.Write("=<value>");
				}
				else if (option.Type == CommandLineOptionValueType.Multiple)
				{
					Console.Write("=<value1>[,<value2>,...]");
				}
			}
			else
			{
				Console.Write(longOptionPrefix ?? "--");
				Console.Write("{0}", option.Name);
				if (option.Type == CommandLineOptionValueType.Single)
				{
					Console.Write("=<value>");
				}
				else if (option.Type == CommandLineOptionValueType.Multiple)
				{
					Console.Write("=<value1>[,<value2>,...]");
				}
			}
			if (option.Optional)
			{
				Console.Write(']');
			}
			// Console.WriteLine();
		}

		/// <summary>
		/// Parses the option.
		/// </summary>
		/// <returns><c>true</c>, if option was parsed, <c>false</c> otherwise.</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="index">Index.</param>
		/// <param name="list">The list into which to add the option if it has been specified.</param>
		/// <param name="optionSet">The set of available options.</param>
		private bool ParseOption(string[] args, ref int index, IList<CommandLineOption> list, CommandLineOption.CommandLineOptionCollection optionSet)
		{
			string longOptionPrefix = "--", shortOptionPrefix = "-";

			bool breakout = false;
			if (args[index].StartsWith(shortOptionPrefix) && args[index].Length == (shortOptionPrefix.Length + 1))
			{
				char shortOptionChar = args[index][args[index].Length - 1];
				CommandLineOption option = optionSet[shortOptionChar];
				if (option != null)
				{
					if (option.Abbreviation == shortOptionChar)
					{
						if (option.Type != CommandLineOptionValueType.None)
						{
							index++;
							option.Value = args[index];
						}
						else
						{
							option.Value = true;
						}

						if (list != null)
							list.Add(option);
					}
				}
				else
				{
					list.Add(new CommandLineOption() { Abbreviation = shortOptionChar });
				}
			}
			else if (args[index].StartsWith(longOptionPrefix))
			{
				// long option format is --name[=value]
				string name = args[index].Substring(longOptionPrefix.Length);
				string value = null;
				if (name.Contains("="))
				{
					int idx = name.IndexOf('=');
					value = name.Substring(idx + 1);
					name = name.Substring(0, idx);
				}
				CommandLineOption option = optionSet[name];
				if (option != null)
				{
					if (option.Type != CommandLineOptionValueType.None)
					{
						// index++;
						// option.Value = args[index];
						if (!name.Contains("="))
						{
							// already taken care of the true case above
							if (index + 1 < args.Length)
							{
								index++;
								value = args[index];
							}
						}
						option.Value = value;
					}
					else
					{
						option.Value = true;
					}
					list.Add(option);
				}
				else
				{
					list.Add(new CommandLineOption() { Name = name });
				}
			}
			else
			{
				// we have reached a non-option
				return true;
			}
			return false;
		}

		public int Start()
		{
			if (Application.Instance == null)
				Application.Instance = this;

			Initialize();

			int exitCode = StartInternal();
			return exitCode;
		}

		// CONTEXTS

		/// <summary>
		/// Gets a collection of <see cref="Context" /> objects representing
		/// system, application, user, and custom contexts for settings and other
		/// items.
		/// </summary>
		/// <value>
		/// A collection of <see cref="Context" /> objects representing system,
		/// application, user, and custom contexts for settings and other items.
		/// </value>
		public ApplicationContextCollection Contexts { get; } = new ApplicationContextCollection();

		/// <summary>
		/// The event raised when a <see cref="Context" /> is added to the
		/// <see cref="Application" />.
		/// </summary>
		public ContextChangedEventHandler ContextAdded;
		/// <summary>
		/// Called when a <see cref="Context" /> is added to the
		/// <see cref="Application" />. The default behavior is to simply fire
		/// the <see cref="ContextAdded" /> event. Implementations should handle
		/// adding context-specific behaviors and UI elements in this method.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnContextAdded(ContextChangedEventArgs e)
		{
			ContextAdded?.Invoke(this, e);
		}

		/// <summary>
		/// The event raised when a <see cref="Context" /> is removed from the
		/// <see cref="Application" />.
		/// </summary>
		public ContextChangedEventHandler ContextRemoved;
		/// <summary>
		/// Called when a <see cref="Context" /> is removed from the
		/// <see cref="Application" />. The default behavior is to simply fire
		/// the <see cref="ContextRemoved" /> event. Implementations should handle
		/// removing context-specific behaviors and UI elements in this method.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnContextRemoved(ContextChangedEventArgs e)
		{
			ContextRemoved?.Invoke(this, e);
		}

		/// <summary>
		/// Handles updating the menus, toolbars, keyboard shortcuts, and other
		/// UI elements associated with the application <see cref="Context" />.
		/// </summary>
		private void AddContext(Context ctx)
		{
			OnContextAdded(new ContextChangedEventArgs(ctx));
		}

		/// <summary>
		/// Handles updating the menus, toolbars, keyboard shortcuts, and other
		/// UI elements associated with the application <see cref="Context" />.
		/// </summary>
		private void RemoveContext(Context ctx)
		{
			OnContextRemoved(new ContextChangedEventArgs(ctx));
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		public void Log(string message)
		{
			Log(null, 0, message);
		}
		/// <summary>
		/// Log the specified message, including information about the relevant
		/// object instance or static class <see cref="Type" />, line number of
		/// the corresponding source code.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="message">Message.</param>
		public void Log(object obj, int lineNumber, string message)
		{
			Type type = (obj is Type ? ((Type)obj) : (obj != null ? obj.GetType() : null));
			StringBuilder sb = new StringBuilder();
			if (type != null)
			{
				sb.Append('[');
				sb.Append(type.Assembly.GetName().Name);
				sb.Append("] ");
				sb.Append(type.FullName);
				sb.Append('(');
				sb.Append(lineNumber);
				sb.Append(')');
				sb.Append(": ");
			}
			sb.Append(message);

			System.Diagnostics.Debug.WriteLine(sb);
		}

		private Language mvarDefaultLanguage = null;
		/// <summary>
		/// The default <see cref="Language"/> used to display translatable text in this application.
		/// </summary>
		public Language DefaultLanguage { get { return mvarDefaultLanguage; } set { mvarDefaultLanguage = value; } }

		private Language.LanguageCollection mvarLanguages = new Language.LanguageCollection();
		/// <summary>
		/// The languages defined for this application. Translations can be added through XML files in the ~/Languages folder.
		/// </summary>
		public Language.LanguageCollection Languages { get { return mvarLanguages; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Application" /> is
		/// currently in the process of shutting down.
		/// </summary>
		/// <value><c>true</c> if stopping; otherwise, <c>false</c>.</value>
		public bool Stopping { get; private set; } = false;

		protected virtual void OnStopping(System.ComponentModel.CancelEventArgs e)
		{

		}
		protected virtual void OnStopped(EventArgs e)
		{

		}

		/// <summary>
		/// The event raised when the <see cref="Stop" /> method is called, before
		/// the application is stopped.
		/// </summary>
		public event System.ComponentModel.CancelEventHandler BeforeShutdown;
		/// <summary>
		/// Event handler for <see cref="BeforeShutdown" /> event. Called when the
		/// <see cref="Stop" /> method is called, before the application is stopped.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnBeforeShutdown(System.ComponentModel.CancelEventArgs e)
		{
			BeforeShutdown?.Invoke(this, e);
		}

		public event EventHandler Shutdown;
		private void OnShutdown(EventArgs e)
		{
			Shutdown?.Invoke(this, e);
		}

		public void Restart()
		{
			Stop();

			Start();
		}

		/// <summary>
		/// Informs the underlying system backend that it is to begin the process
		/// of application shutdown, gracefully ending the main loop before
		/// returning the specified <paramref name="exitCode" /> to the operating
		/// system.
		/// </summary>
		/// <param name="exitCode">
		/// The exit code to return to the operating system.
		/// </param>
		protected virtual void StopInternal(int exitCode)
		{
		}

		/// <summary>
		/// Shuts down the application gracefully, calling any event handlers
		/// attached to the shutdown event to give listeners the opportunity to
		/// cancel the shutdown and passing the specified
		/// <paramref name="exitCode" /> to the operating system.
		/// </summary>
		/// <param name="exitCode">
		/// The exit code to return to the operating system.
		/// </param>
		public void Stop(int exitCode = 0)
		{
			if (Stopping)
				return;

			Stopping = true;

			System.ComponentModel.CancelEventArgs ce = new System.ComponentModel.CancelEventArgs();
			OnBeforeShutdown(ce);
			if (ce.Cancel)
			{
				Stopping = false;
				return;
			}

			ce = new System.ComponentModel.CancelEventArgs();

			// OnStopping called after setting Stopping to True, otherwise there is no real difference
			OnStopping(ce);
			if (ce.Cancel)
			{
				Stopping = false;
				return;
			}

			StopInternal(exitCode);

			OnShutdown(EventArgs.Empty);

			OnStopped(EventArgs.Empty);
			Stopping = false;
		}

		private Dictionary<Guid, object> _settings = new Dictionary<Guid, object>();
		public T GetSetting<T>(Guid id, T defaultValue = default(T))
		{
			object value = GetSetting(id, (object)defaultValue);
			if (value is T)
			{
				return (T)value;
			}
			else if (value is string && ((string)value).TryParse(typeof(T), out object val))
			{
				return (T)val;
			}

			return defaultValue;
		}
		public object GetSetting(Guid id, object defaultValue = null)
		{
			if (_settings.ContainsKey(id))
				return _settings[id];
			return defaultValue;
		}
		public void SetSetting<T>(Guid id, T value)
		{
			_settings[id] = value;
		}

		protected virtual bool ShowCommandLineHelp(out int resultCode)
		{
			// bash: cd returns 2 if --help is specified OR invalid option selected, 1 if file not found
			PrintUsageStatement();
			resultCode = 2;
			return true;
		}

		protected virtual Plugin[] GetAdditionalPluginsInternal()
		{
			return new Plugin[0];
		}
		public Plugin[] GetAdditionalPlugins() { return GetAdditionalPluginsInternal(); }
	}
}
