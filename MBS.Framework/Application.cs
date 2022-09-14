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

		public string[] FindFiles(string filename, FindFileOptions options = FindFileOptions.All)
		{
			if (filename.StartsWith("~/"))
			{
				filename = filename.Substring(2);
			}

			List<string> files = new List<string>();
			string[] paths = EnumerateDataPaths();
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
		public string FindFile(string fileName, FindFileOptions options = FindFileOptions.All)
		{
			string[] files = FindFiles(fileName, options);
			if (files.Length > 0)
			{
				return files[0];
			}
			return null;
		}

		public EventFilter.EventFilterCollection EventFilters { get; } = new EventFilter.EventFilterCollection();

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

		protected virtual Context FindContextInternal(Guid contextID)
		{
			return null;
		}
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

		public string[] EnumerateDataPaths()
		{
			return new string[]
			{
				// first look in the application root directory since this will override everything else
				BasePath,
				// then look in /usr/share/universal-editor or C:\ProgramData\Mike Becker's Software\Universal Editor
				String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
				{
					System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
					ShortName
				}),
				// then look in ~/.local/share/universal-editor or C:\Users\USERNAME\AppData\Local\Mike Becker's Software\Universal Editor
				String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
				{
					System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
					ShortName
				}),
				// then look in ~/.universal-editor or C:\Users\USERNAME\AppData\Roaming\Mike Becker's Software\Universal Editor
				String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[]
				{
					System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
					ShortName
				})
			};
		}

		public string ShortName { get; set; }
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

			InitializeInternal();
			Initialized = true;
		}

		protected virtual int StartInternal()
		{
			return 0;
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
				return;
			}

			ce = new System.ComponentModel.CancelEventArgs();

			// OnStopping called after setting Stopping to True, otherwise there is no real difference
			OnStopping(ce);
			if (ce.Cancel)
				return;

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
	}
}
