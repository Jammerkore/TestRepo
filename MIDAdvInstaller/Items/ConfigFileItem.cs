using System;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MIDRetail.MIDAdvInstaller
{
	[Serializable]
	public class ConfigFileItem {
		[DefaultValueAttribute("")]
		public string ConfigFileName = string.Empty;
        public string Version = string.Empty;
        public string InstallLocation = string.Empty;
        public eInstallerType InstallerType = eInstallerType.Undefined;
        public DateTime InstalledDateTime = DateTime.Now;
        public string ProcessDescription = string.Empty;
        public eSpecialNodeType SpecialNodeType = eSpecialNodeType.Undefined;
		public ConfigItemList ConfigValues = new ConfigItemList();

		public ConfigFileItem() {
		}

		public ConfigFileItem(string loc) {
            InstallLocation = loc;
		}

		public override string ToString() {
            return InstallLocation;
		}
	}

	[Serializable]
	public class ConfigFileItemList: ArrayList {
		[XmlIgnoreAttribute]
		private Hashtable ht = new Hashtable();

		public ConfigFileItemList(): base() {
		}

		new public int Add(object obj) {
			int add = base.Add(obj);
			ht.Add(obj.ToString(), add);
			return add;
		}

		public override void Remove(object obj) {
			this.RemoveAt(Convert.ToInt32(ht[obj.ToString()]));
		}

		public override void RemoveAt(int index) {
			ht.Remove(base[index].ToString());
			base.RemoveAt(index);
			// update indicies for hashtable
			for(int i = index; i < base.Count; i++) {
				ht[base[i].ToString()] = i;
			}
		}

		public ConfigFileItem this[string myKey] {
			get {		
				ConfigFileItem ret = null;

				if(ht[myKey] != null) {
					ret = (ConfigFileItem) base[(int)ht[myKey]];
				}

				return ret;
			}		
		}

		public new ConfigFileItem this[int myKey] {
			get {		
				object ret = base[myKey];

				if(ret != null)
					return (ConfigFileItem) ret;

				return null;
			}		
		}
	}
}