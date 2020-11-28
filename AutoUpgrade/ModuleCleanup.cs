using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace MIDRetail.AutoUpgrade
{
    class ModuleCleanup
    {
        // Begin TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
        //List<string> files = new List<string>();
        List<string> preDeleteFiles = new List<string>();
        List<string> postDeletefiles = new List<string>();
        // End TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder

        public ModuleCleanup()
        {
            // ********************************************************* //
            // *                     WARNING                           * //
            // *                                                       * //
            // * Any changes to these lists must also be made to the   * //
            // * corresponding module in the MIDAdvInstaller project   * //
            // *                                                       * //
            // * preDeleteFiles -= deleted before upgrade              * //
            // * postDeleteFiles -= deleted after upgrade              * //
            // ********************************************************* //
            preDeleteFiles.Add("MIDRetail.Windows.dll");     // TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
        }

        public void RemoveOldFiles(string aRootFolder, bool isPreDelete)    // TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
        {
            FileInfo fileInfo;
            // Begin TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
            List<string> files = postDeletefiles;
            if (isPreDelete)
            {
                files = preDeleteFiles;
            }
            // End TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder

            try
            {
                foreach (string f in Directory.GetFiles(aRootFolder))
                {
                    fileInfo = new FileInfo(f);
                    if (files.Contains(fileInfo.Name) ||
                        fileInfo.Name.StartsWith("Infragistics2."))
                    {
                        DeleteFile(f);
                    }
                }
                foreach (string d in Directory.GetDirectories(aRootFolder))
                {
                    RemoveOldFiles(d, isPreDelete);     // TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
                }
            }
            catch 
            {
                throw;
            }
        }

        private void DeleteFile(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
            }
            catch 
            {
                throw;
            }
        }

        private void SetFileNotReadOnly(string FileName)
        {
            try
            {
                System.IO.File.SetAttributes(FileName, System.IO.File.GetAttributes(FileName) & ~(FileAttributes.ReadOnly));
            }
            catch
            {
                
            }
        }
    }

    
}