using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MIDRetail.MIDAdvInstaller
{
	[Serializable]
	public class ConfigItem {
		[DefaultValueAttribute("")]
		public string Key = string.Empty;
		[DefaultValueAttribute("")]
		public string DefaultValue = string.Empty;
		[DefaultValueAttribute("")]
		public string Description = string.Empty;
		public eConfigType MyType;
		[DefaultValueAttribute("")]
		public string DisplayTrueValue = string.Empty;
		[DefaultValueAttribute("")]
		public string DisplayFalseValue = string.Empty;
		
		[XmlIgnoreAttribute]
		public int isChangedIndex = -1;
		[DefaultValueAttribute(true)]
		public bool PropagateChanges = true;

		public ConfigItem() {
		}

		public override string ToString() {
			return Key;
		}

		[XmlIgnoreAttribute]
		public string Name {
			get {
				return Key;
			}
		}

		[XmlIgnoreAttribute]
		public string Value {
			get {
				return DefaultValue;
			}
		}
	}

	[Serializable]
	public class ConfigItemList: ArrayList {
		[XmlIgnoreAttribute]
		private Hashtable ht = new Hashtable();

		public ConfigItemList(): base() {
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

		public ConfigItem this[string myKey] {
			get {		
				ConfigItem ret = null;

				if(ht[myKey] != null) {
					ret = (ConfigItem) base[(int)ht[myKey]];
				}

				return ret;
			}		
		}

		public new ConfigItem this[int myKey] {
			get {		
				object ret = base[myKey];

				if(ret != null)
					return (ConfigItem) ret;

				return null;
			}		
		}
		public void CopyFrom(ConfigItemList lst) {
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < lst.Count; i++) {
				ConfigItem itm = new ConfigItem();
				
				sb.Remove(0, sb.Length);
				sb.Append(lst[i].DefaultValue);
				itm.DefaultValue = sb.ToString();
				
				sb.Remove(0, sb.Length);
				sb.Append(lst[i].Key);
				itm.Key = sb.ToString();

				itm.MyType = lst[i].MyType;
				Add(itm);
			}
		}
	}
}