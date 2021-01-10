using System;
using System.Collections.Generic;

namespace MBS.Framework
{
	public class Command : ISupportsExtraData
	{
		public class CommandCollection
			: System.Collections.ObjectModel.Collection<Command>
		{
			public Command this[string ID]
			{
				get
				{
					foreach (Command command in this)
					{
						if (command.ID == ID) return command;
					}
					return null;
				}
			}
		}

		public Command()
		{
		}
		public Command(string id, string title, CommandItem[] items = null)
		{
			ID = id;
			Title = title;
			if (items != null)
			{
				for (int i = 0; i < items.Length; i++)
				{
					Items.Add(items[i]);
				}
			}
		}
		/// <summary>
		/// Determines whether this command displays as checked.
		/// </summary>
		public bool Checked { get; set; } = false;
		/// <summary>
		/// The ID of the command, used to reference it in <see cref="CommandReferenceCommandItem"/>.
		/// </summary>
		public string ID { get; set; } = String.Empty;
		/// <summary>
		/// The title of the command (including mnemonic prefix, if applicable).
		/// </summary>
		public string Title { get; set; } = String.Empty;

		private string mvarDefaultCommandID = String.Empty;
		public string DefaultCommandID { get { return mvarDefaultCommandID; } set { mvarDefaultCommandID = value; } }

		/// <summary>
		/// A <see cref="StockType"/> that represents a predefined, platform-themed command.
		/// </summary>
		public StockType StockType { get; set; } = StockType.None;

		private string mvarImageFileName = String.Empty;
		/// <summary>
		/// The file name of the image to be displayed on the command.
		/// </summary>
		public string ImageFileName { get { return mvarImageFileName; } set { mvarImageFileName = value; } }


		/// <summary>
		/// The child <see cref="CommandItem"/>s that are contained within this <see cref="Command"/>.
		/// </summary>
		public CommandItem.CommandItemCollection Items { get; } = new CommandItem.CommandItemCollection();
		
		/// <summary>
		/// The event that is fired when the command is executed.
		/// </summary>
		public event EventHandler Executed;

		/// <summary>
		/// Determines whether this <see cref="Command" /> is enabled in all <see cref="CommandBar" />s and <see cref="MenuBar" />s
		/// that reference it.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Enabled { get; set; } = true;

		/// <summary>
		/// Determines whether this <see cref="Command" /> is visible in all <see cref="CommandBar" />s and <see cref="MenuBar" />s
		/// that reference it.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible { get; set; }

		/// <summary>
		/// Executes this <see cref="Command"/>.
		/// </summary>
		public void Execute()
		{
			if (Executed != null) Executed(this, EventArgs.Empty);
		}

		public override string ToString()
		{
			return String.Format("{0} [{1}]", ID, Title);
		}

		private Dictionary<string, object> _extraData = new Dictionary<string, object>();
		public T GetExtraData<T>(string key, T defaultValue = default(T))
		{
			if (_extraData.ContainsKey(key))
			{
				if (_extraData[key] is T)
				{
					return (T)_extraData[key];
				}
			}
			return defaultValue;
		}

		public void SetExtraData<T>(string key, T value)
		{
			_extraData[key] = value;
		}

		public object GetExtraData(string key, object defaultValue = null)
		{
			if (_extraData.ContainsKey(key))
				return _extraData[key];
			return defaultValue;
		}

		public void SetExtraData(string key, object value)
		{
			_extraData[key] = value;
		}
	}
}

