using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MIDRetail.MIDAdvInstaller
{
	[Serializable]
	[XmlInclude(typeof(AvailableItem))]
	[XmlInclude(typeof(ConfigItem))]
    [XmlInclude(typeof(AvailableConfigItem))]
	public class InstallDescription {
        //[DefaultValueAttribute("")]
        //public string ClientText = string.Empty;
		[DefaultValueAttribute("")]
		public string ServicesText = string.Empty;
        //[DefaultValueAttribute("")]
        //public string APIText = string.Empty;
		[DefaultValueAttribute("")]
		public string ProductName = string.Empty;
        [DefaultValueAttribute("")]
        public string MIDSettingsName = string.Empty;

		public ConfigItemList ConfigValues = new ConfigItemList();
		public AvailableItemList ServicesAvailable = new AvailableItemList();
		public AvailableItemList ModuleDescriptors = new AvailableItemList();
		public AvailableItem Client = new AvailableItem();
        public AvailableItem ControlServer = new AvailableItem();
        public AvailableItem APIs = new AvailableItem();

		public InstallDescription() {

		}

		public void serialize(string filename) {
			using(StreamWriter sw = File.CreateText(filename)) {
				XmlSerializer xs = new XmlSerializer(typeof(InstallDescription));
				xs.Serialize(sw,this);
			}
		}

		public static InstallDescription deserialize(string filename) {
			InstallDescription ret = null;

			try {
				using(StreamReader sr = new StreamReader(filename)) {
					XmlSerializer xs = new XmlSerializer(typeof(InstallDescription));
					ret = (InstallDescription) xs.Deserialize(sr);
				}
			} catch (Exception) {}

			return ret;
		}
	}
}
