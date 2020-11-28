using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;   // MID Track 3994 Performance
using System.Runtime.Serialization; // MID Track 3994 Performance
using System.Runtime.Serialization.Formatters.Binary; // MID Track 3994 Performance


namespace MIDRetail.DataCommon
{
    //Begin TT#1192 - JSmith - Batch Blocked
    public class MIDDeserializationBinder : SerializationBinder
    {

        public override Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                Type typeToDeserialize;

                assemblyName = assemblyName.Replace("MID.MRS.", "MIDRetail.");
                typeName = typeName.Replace("MID.MRS.", "MIDRetail.");

                //Begin TT#1192 - JSmith - Batch Blocked
                //typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                //                typeName, assemblyName));
                string shortAssemblyName = assemblyName.Split(',')[0];
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, shortAssemblyName));
                //End TT#1192

                return typeToDeserialize;
            }
            catch
            {
                throw;
            }
        }

    }
    //End TT#1192
}
