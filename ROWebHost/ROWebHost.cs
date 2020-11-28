using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

using Logility.ROWeb;

namespace Logility.ROWebHost
{
    class ROWebHost
    {
        //=======
        // FIELDS
        //=======

        static void Main(string[] args)
        {
            ServiceHost ROWebConsoleService = null;
            try
            {
                string port = "9100";
                if (args.Length > 0)
                {
                    port = Convert.ToString(args[0]);
                }
                //Base Address for ROWebConsoleService
                Uri baseAddress = new Uri("net.tcp://" + Environment.MachineName + ":" + port + "/ROWebConsoleService");
                
                //Instantiate ServiceHost
                ROWebConsoleService = new ServiceHost(typeof(Logility.ROWeb.ROWebConsoleService),
                    baseAddress);

                //Open
                ROWebConsoleService.Open();
                Console.WriteLine("Service is live now at : {0}", baseAddress);
                Console.ReadKey();                
            }

            catch (Exception ex)
            {
                ROWebConsoleService = null;
                Console.WriteLine("There is an issue with ROWebConsoleService: " + ex.Message);
            }

        }
    }
}
