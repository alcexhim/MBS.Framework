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

namespace MBS.Framework
{
	public class Application
	{
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

		public static Application Instance { get; set; } = null;

		protected virtual Command FindCommandInternal(string commandID)
		{
			return null;
		}
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

		public string ShortName { get; set; }
		public string Title { get; set; } = String.Empty;
		public int ExitCode { get; protected set; } = 0;

		protected virtual InstallationStatus GetInstallationStatusInternal()
		{
			return InstallationStatus.Unknown;
		}
		public InstallationStatus InstallationStatus { get { return GetInstallationStatusInternal(); } }

		public CommandLine CommandLine { get; protected set; } = null;

		public Application()
		{
			CommandLine = new DefaultCommandLine();
		}

		private Dictionary<string, List<EventHandler>> _CommandEventHandlers = new Dictionary<string, List<EventHandler>>();
		public Command.CommandCollection Commands { get; } = new Command.CommandCollection();
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

		public void ExecuteCommand(string id, KeyValuePair<string, object>[] namedParameters = null)
		{
			Command cmd = Commands[id];

			// handle command event handlers attached without a Command instance
			if (_CommandEventHandlers.ContainsKey(id))
			{
				List<EventHandler> c = _CommandEventHandlers[id];
				for (int i = 0; i < c.Count; i++)
				{
					c[i](cmd, new CommandEventArgs(cmd, namedParameters));
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
		/// Gets a collection of <see cref="Context" /> objects representing system, application, user, and custom contexts for settings and other items.
		/// </summary>
		/// <value>A collection of <see cref="Context" /> objects representing cpublic  for settings and other items.</value>
		public ApplicationContextCollection Contexts { get; } = new ApplicationContextCollection();

		public ContextChangedEventHandler ContextAdded;
		protected virtual void OnContextAdded(ContextChangedEventArgs e)
		{
			ContextAdded?.Invoke(this, e);
		}

		public ContextChangedEventHandler ContextRemoved;
		protected virtual void OnContextRemoved(ContextChangedEventArgs e)
		{
			ContextRemoved?.Invoke(this, e);
		}

		/// <summary>
		/// Handles updating the menus, toolbars, keyboard shortcuts, and other UI elements associated with the application <see cref="Context" />.
		/// </summary>
		private void AddContext(Context ctx)
		{
			OnContextAdded(new ContextChangedEventArgs(ctx));
		}

		/// <summary>
		/// Handles updating the menus, toolbars, keyboard shortcuts, and other UI elements associated with the application <see cref="Context" />.
		/// </summary>
		private void RemoveContext(Context ctx)
		{
			OnContextRemoved(new ContextChangedEventArgs(ctx));
		}

		// LOGGING
		public void Log(string message)
		{
			Log(null, 0, message);
		}
		public void Log(object obj, int lineNumber, string message)
		{
			if (obj is Type)
			{
				Console.WriteLine("{0}: {1}", ((Type)obj).FullName, message);
			}
			else if (obj != null)
			{
				Console.WriteLine("{0}: {1}", obj.GetType().FullName, message);
			}
			else
			{
				Console.WriteLine("{0}", message);
			}
		}

		public bool Stopping { get; private set; } = false;

		protected virtual void OnStopping(System.ComponentModel.CancelEventArgs e)
		{

		}
		protected virtual void OnStopped(EventArgs e)
		{

		}

		public event System.ComponentModel.CancelEventHandler BeforeShutdown;
		protected virtual void OnBeforeShutdown(System.ComponentModel.CancelEventArgs e)
		{
			BeforeShutdown?.Invoke(this, e);
		}

		public event EventHandler Shutdown;
		private void OnShutdown(EventArgs e)
		{
			Shutdown?.Invoke(this, e);
		}


		protected virtual void StopInternal(int exitCode)
		{
		}
		public void Stop(int exitCode = 0)
		{
			if (Stopping)
				return;

			Stopping = true;

			System.ComponentModel.CancelEventArgs ce = new System.ComponentModel.CancelEventArgs();
			OnStopping(ce);
			if (ce.Cancel)
				return;

			ce = new System.ComponentModel.CancelEventArgs();
			OnBeforeShutdown(ce);
			if (ce.Cancel)
			{
				return;
			}

			StopInternal(exitCode);

			OnShutdown(EventArgs.Empty);

			OnStopped(EventArgs.Empty);
			Stopping = false;
		}

	}
}
