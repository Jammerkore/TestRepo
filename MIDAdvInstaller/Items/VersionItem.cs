using System;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MIDAdvInstaller {
	[Serializable]
	public class VersionItem {
		public string ProductName = string.Empty;
		public string Version = string.Empty;
		public string InstallLocation = string.Empty;
		public DateTime InstalledDateTime = DateTime.Now;

		[DefaultValueAttribute(false)]
		public bool StartMenuShortcutInstalled = false;
		[DefaultValueAttribute(false)]
		public bool DesktopShortCutInstalled = false;
		[DefaultValueAttribute(false)]
		public bool StartMenuShortcutIsGlobal = false;
		[DefaultValueAttribute(false)]
		public bool DesktopShortCutIsGlobal = false;

		public ConfigFileItemList ConfigLocations = new ConfigFileItemList();
		public AvailableItemList ServicesAvailable = new AvailableItemList();
		public AvailableItemList ModuleDescriptors = new AvailableItemList();

		[DefaultValueAttribute(OptionType.Client)]
		public OptionType InstallType = OptionType.Client;

		public VersionItem() {
		}

		public override string ToString() {
			return ProductName + " " + Version;
		}

	}

	[Serializable]
	public class VersionItemList: ArrayList {
		[XmlIgnoreAttribute]
		private Hashtable ht = new Hashtable();

		public VersionItemList(): base() {
		}

		new public int Add(object obj) {
			int add = base.Add(obj);
			ht.Add(obj.ToString(), add);
			return add;
		}


		public VersionItem this[string myKey] {
			get {		
				VersionItem ret = null;

				if(ht[myKey] != null) {
					ret = (VersionItem) base[(int)ht[myKey]];
				}

				return ret;
			}			
		}

		new public VersionItem this[int myKey] {
			get {		
				object ret = base[myKey];

				if(ret != null)
					return (VersionItem) ret;

				return null;
			}		
		}
	}
}
