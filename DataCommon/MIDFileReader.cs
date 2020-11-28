// Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MIDRetail.DataCommon
{
    public class MIDFileReader
    {
        static MIDFileReader()
		{
			
		}

        public static object ReadXMLFile(string aLocation, Type aType)
        {
            TextReader r = null;

            try
            {
                XmlSerializer s = new XmlSerializer(aType);		
                r = new StreamReader(aLocation);	

                byte[] bytes = Encoding.ASCII.GetBytes(r.ReadToEnd());
                MemoryStream mem = new MemoryStream(bytes);
                return s.Deserialize(mem);
            }
            finally
            {
                if (r != null)
                    r.Close();
            }
        }
    }
}
// End TT#3523 - JSmith - Performance of Anthro morning processing jobs
