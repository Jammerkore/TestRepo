using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Xml.Serialization;

namespace MIDRetail.MIDAdvInstaller
{
	[Serializable]
	public class StatusTracker {
		[XmlIgnoreAttribute]
		public int SelectedVersion = -1;
		public int MaxVersion = 1;
		public ProcessItemList Processes = new ProcessItemList();
        public ConfigFileItemList ConfigFiles = new ConfigFileItemList();

		public StatusTracker() {
		}

		public void serialize(string filename) {
			using(StreamWriter sw = File.CreateText(filename)) {
				XmlSerializer xs = new XmlSerializer(typeof(StatusTracker));
				xs.Serialize(sw,this);
			}
		}

		public static StatusTracker deserialize(string filename) {
			StatusTracker ret = null;

			try {
				using(StreamReader sr = new StreamReader(filename)) {
					XmlSerializer xs = new XmlSerializer(typeof(StatusTracker));
					ret = (StatusTracker) xs.Deserialize(sr);
				}
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}

			return ret;
		}
	}
}