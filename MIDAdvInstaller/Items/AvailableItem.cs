using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;


namespace MIDRetail.MIDAdvInstaller
{
	[Serializable]
    [XmlInclude(typeof(AvailableConfigItem))]
	public class AvailableItem {
		public string Location = string.Empty;
        public string Location64 = string.Empty;
		public string Description = string.Empty;
		public string FriendlyName = string.Empty;
        public eProcessType ProcessType = eProcessType.Undefined;
		public eOptionType MyType;
        public AvailableConfigItemList AvailableConfigItems = new AvailableConfigItemList();
        //public AvailableConfigItem[] AvailableConfigItems;

		[DefaultValueAttribute("")]
		public string ShortCutExe = string.Empty;

		public AvailableItem() {
		}

		public override string ToString() {
			return FriendlyName;
		}
	}

	[Serializable]
	public class AvailableItemList: ArrayList {
		[System.Xml.Serialization.XmlIgnoreAttribute]
		private Hashtable ht = new Hashtable();

		public AvailableItemList(): base() {
		}

		new public int Add(object obj) {
			int add = base.Add(obj);
			ht.Add(obj.ToString(), add);
			return add;
		}

		new public void Clear() {
			ht.Clear();
			base.Clear();
		}

		new public AvailableItem this[int myKey] {
			get {		
				object ret = base[myKey];

				if(ret != null)
					return (AvailableItem) ret;

				return null;
			}		
		}

		public AvailableItem this[string myKey] {
			get {		
				AvailableItem ret = null;

				if(ht[myKey] != null) {
					ret = (AvailableItem) base[(int)ht[myKey]];
				}

				return ret;
			}		
		}
	}

    [Serializable]
    public class AvailableConfigItem
    {
        [DefaultValueAttribute("")]
        public string Key = string.Empty;

        [DefaultValueAttribute("")]
        public string DefaultValue = string.Empty;

        public AvailableConfigItem()
        {
        }
   }

    [Serializable]
    public class AvailableConfigItemList : ArrayList
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private Hashtable ht = new Hashtable();

        public AvailableConfigItemList()
            : base()
        {
        }

        new public int Add(object obj)
        {
            int add = base.Add(obj);
            ht.Add(obj.ToString(), add);
            return add;
        }

        new public void Clear()
        {
            ht.Clear();
            base.Clear();
        }

        new public AvailableConfigItem this[int myKey]
        {
            get
            {
                object ret = base[myKey];

                if (ret != null)
                    return (AvailableConfigItem)ret;

                return null;
            }
        }

        public AvailableConfigItem this[string myKey]
        {
            get
            {
                AvailableConfigItem ret = null;

                if (ht[myKey] != null)
                {
                    ret = (AvailableConfigItem)base[(int)ht[myKey]];
                }

                return ret;
            }
        }
    }
}