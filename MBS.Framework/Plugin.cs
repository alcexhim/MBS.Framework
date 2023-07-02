//
//  Plugin.cs
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
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Linq;

namespace MBS.Framework
{
	public class Plugin
	{
		public virtual string Title { get; set; } = null;
		public Feature.FeatureCollection ProvidedFeatures { get; } = new Feature.FeatureCollection();

		public bool Initialized { get; private set; } = false;
		public void Initialize()
		{
			if (Initialized)
				return;

			InitializeInternal();
			Initialized = true;
		}

		public Guid ID { get; set; } = Guid.Empty;

		protected virtual void InitializeInternal()
		{
			// this method intentionally left blank
		}

		protected virtual bool AutoRegister => true;

		protected virtual bool IsSupportedInternal()
		{
			return true;
		}
		public bool IsSupported()
		{
			return IsSupportedInternal();
		}
		private static Plugin[] _plugins = null;
		public static TPlugin[] Get<TPlugin>(bool resetCache = false) where TPlugin : Plugin
		{
			Plugin[] plugins = Get(resetCache);
			return plugins.OfType<TPlugin>().ToArray();
		}
		public static Plugin[] Get(bool resetCache = false)
		{
			if (resetCache)
			{
				_plugins = null; // should not be cached? // actually, yes it should...

				// 2020-12-12 20:54 by beckermj
				// ACTUALLY, it depends on whether the configuration needs to be persisted across calls to Get()
				// 				[it does] and whether the list of plugins needs to be reloaded when CustomPlugins is modified
				//				[it shouldn't, but it does] .
				//
				// The safest way we can handle this RIGHT NOW is to prevent any plugin from being loaded until after CustomPlugins
				// is initialized by UIApplication.
				//
				// We call Plugins.Get(new Feature[] { KnownFeatures.UWTPlatform }) to retrieve the available User Interface plugins
				// that supply the UWT Platform implementation (e.g. GTK, Windows Forms, etc.) - and this causes CustomPlugins to not
				// load properly since it loads AFTER this initial call to Plugins.Get().
				//
				// So I add ed a resetCache parameter that when specified TRUE will clear the cache and then return the appropriate
				// plugin. This way we can continue caching future calls to Get() without missing out on CustomPlugins.
			}

			if (_plugins == null)
			{
				Type[] types = MBS.Framework.Reflection.GetAvailableTypes(new Type[] { typeof(Plugin) });
				System.Collections.Generic.List<Plugin> plugins = new System.Collections.Generic.List<Plugin>();
				for (int i = 0; i < types.Length; i++)
				{
					try
					{
						if (types[i].IsSubclassOf(typeof(ICustomPlugin)))
						{
							continue;
						}

						Plugin plg = (Plugin)types[i].Assembly.CreateInstance(types[i].FullName);
						plugins.Add(plg);
					}
					catch (Exception ex)
					{
					}
				}

				Plugin[] plugins2 = Application.Instance.GetAdditionalPlugins();
				for (int i = 0; i < plugins2.Length; i++)
				{
					plugins.Add(plugins2[i]);
				}
				_plugins = plugins.ToArray();

				if (resetCache)
				{
					_plugins = null;
					return plugins.ToArray();
				}
			}
			return _plugins;
		}

		public static TPlugin[] Get<TPlugin>(Feature[] providedFeatures, bool resetCache = false) where TPlugin : Plugin
		{
			Plugin[] plugins = Get(providedFeatures, resetCache);
			return plugins.OfType<TPlugin>().ToArray();
		}
		public static Plugin[] Get(Feature[] providedFeatures, bool resetCache = false)
		{
			System.Collections.Generic.List<Plugin> list = new System.Collections.Generic.List<Plugin>();
			Plugin[] plugins = Get(resetCache);
			for (int i = 0; i < plugins.Length; i++)
			{
				if (!plugins[i].AutoRegister)
					continue;

				if (!plugins[i].IsSupported())
					continue;

				for (int j = 0; j < providedFeatures.Length; j++)
				{
					if (plugins[i].ProvidedFeatures.Contains(providedFeatures[j]))
						list.Add(plugins[i]);
				}
			}
			return list.ToArray();
		}

		public static TPlugin Get<TPlugin>(Guid id) where TPlugin : Plugin
		{
			Plugin plugin = Get(id);
			return (TPlugin)plugin;
		}
		public static Plugin Get(Guid id)
		{
			Plugin[] plugins = Get();
			for (int i = 0; i < plugins.Length; i++)
			{
				if (plugins[i].ID == id)
					return plugins[i];
			}
			return null;
		}
	}
}
