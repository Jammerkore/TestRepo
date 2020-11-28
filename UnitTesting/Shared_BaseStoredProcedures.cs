using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

using MIDRetail.Data;

namespace UnitTesting
{
    public static class Shared_BaseStoredProcedures
    {
        public static List<baseStoredProcedure> storedProcedureList = new List<baseStoredProcedure>();
        private static List<projectFile> moduleList = new List<projectFile>();
        private static DataSet dsDefaultParameters;
        private static DataSet dsDefaultAttributes;

        public static void PopulateStoredProcedureListFromAssembly()
        {
            Assembly a = Assembly.GetAssembly(typeof(MIDRetail.Data.baseStoredProcedure));  //Assembly=MIDRetail.Data
            Type[] assemblyTypes = a.GetTypes();

            CreateDefaultParameterDataset();
            CreateDefaultAttributeDataset();
            moduleList.Clear();

            string sDataProjectDir = Shared_UtilityFunctions.GetCurrentProjectPath() + "Data";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sDataProjectDir);

            string matchingFile = string.Empty;
            foreach (System.IO.FileInfo fi in di.GetFiles("*SP.cs"))
            {
                projectFile pf = new projectFile();
                string fileName = fi.FullName;
                int lastSlash = fileName.LastIndexOf('\\');
                if (lastSlash != -1)
                {
                    fileName = fileName.Substring(lastSlash + 1);
                }
                pf.moduleName = fileName;
                pf.filePath = fi.FullName;
                moduleList.Add(pf);
            }

            storedProcedureList.Clear();
            foreach (Type t in assemblyTypes)
            {
                string tName = t.Name;
                if (t.BaseType == typeof(MIDRetail.Data.baseStoredProcedure))
                {
                    storedProcedureList.Add(Activator.CreateInstance(t) as MIDRetail.Data.baseStoredProcedure);
                }

                GetDefaultParameters(t.GetNestedTypes());
            }
        }

        private static void CreateDefaultParameterDataset()
        {
            dsDefaultParameters = new DataSet();
            dsDefaultParameters.Tables.Add("DefaultParameters");
            dsDefaultParameters.Tables[0].Columns.Add("procedureName", typeof(string));
            dsDefaultParameters.Tables[0].Columns.Add("parameterName", typeof(string));
            dsDefaultParameters.Tables[0].Columns.Add("environmentName", typeof(string));
            dsDefaultParameters.Tables[0].Columns.Add("defaultValue", typeof(string)); 
        }

        private static void CreateDefaultAttributeDataset()
        {
            dsDefaultAttributes = new DataSet();
            dsDefaultAttributes.Tables.Add("DefaultAttributes");
            dsDefaultAttributes.Tables[0].Columns.Add("procedureName", typeof(string));
            dsDefaultAttributes.Tables[0].Columns.Add("selectStatement", typeof(string));
            dsDefaultAttributes.Tables[0].Columns.Add("bypassValidation", typeof(bool));
        }

        private static void GetDefaultParameters(Type[] nestedTypes)
        {
            foreach (Type t in nestedTypes)
            {
                System.Reflection.FieldInfo[] fList = t.GetFields();
                foreach (System.Reflection.FieldInfo f in fList)
                {
                    string fName = f.Name;
                    object[] customFieldAttr2 = f.GetCustomAttributes(false);
                    if (customFieldAttr2.Length > 0 && customFieldAttr2[0].GetType() == typeof(UnitTestParameterAttribute))
                    {
                        UnitTestParameterAttribute unitTestAttribute = (UnitTestParameterAttribute)customFieldAttr2[0];
                        string defaultValue = unitTestAttribute.DefaultValue;
                        string DB = unitTestAttribute.DB;
                        MIDRetail.Data.baseStoredProcedure sp = (MIDRetail.Data.baseStoredProcedure)Activator.CreateInstance(t);

                        AssignDefaultParameterValues(sp.procedureName, "@" + fName, DB, defaultValue);
                    }
                }

                System.Reflection.MethodInfo[] mList = t.GetMethods();
                foreach (System.Reflection.MethodInfo m in mList)
                {
                    string mName = m.Name;
                    object[] customFieldAttr2 = m.GetCustomAttributes(false);
                    if (customFieldAttr2.Length > 0 && customFieldAttr2[0].GetType() == typeof(UnitTestMethodAttribute))
                    {
                        UnitTestMethodAttribute unitTestUpdateAttribute = (UnitTestMethodAttribute)customFieldAttr2[0];
                        string updateSelectStatement = unitTestUpdateAttribute.SelectStatement;
                        //string DB = unitTestUpdateAttribute.DB;
                        //MIDRetail.Data.storedProcedureTestKinds testKind = unitTestUpdateAttribute.testKinds;
                        //int expectedCount = unitTestUpdateAttribute.expectedCount;
                        bool bypassValidation = unitTestUpdateAttribute.BypassValidation;
                        MIDRetail.Data.baseStoredProcedure sp = (MIDRetail.Data.baseStoredProcedure)Activator.CreateInstance(t);

                        AssignUnitTestAttributes(sp.procedureName, updateSelectStatement, bypassValidation);
                    }
                }

                GetDefaultParameters(t.GetNestedTypes());
            }
        }
        public static bool HasDefaults(string procedureName)
        {
            DataRow[] drFind = dsDefaultParameters.Tables[0].Select("procedureName='" + procedureName + "'");
            if (drFind.Length > 0)
                return true;
            else
                return false;
        }
        //private static bool HasSelectStatements(string procedureName)
        //{
        //    DataRow[] drFind = dsDefaultAttributes.Tables[0].Select("procedureName='" + procedureName + "'");
        //    if (drFind.Length > 0)
        //        return true;
        //    else
        //        return false;
        //}
        public static bool BypassValidation(string procedureName)
        {
            DataRow[] drFind = dsDefaultAttributes.Tables[0].Select("procedureName='" + procedureName + "'");
            if (drFind.Length == 0)
            {
                return false;
            }
            else
            {
                return (bool)drFind[0]["bypassValidation"];
            }
        }
        private static void AssignDefaultParameterValues(string procedureName, string parameterName, string environmentName, string defaultValue)
        {
            DataRow dr = dsDefaultParameters.Tables[0].NewRow();
            dr["procedureName"] = procedureName;
            dr["parameterName"] = parameterName;
            dr["environmentName"] = environmentName;
            dr["defaultValue"] = defaultValue;

            dsDefaultParameters.Tables[0].Rows.Add(dr);
        }
        private static void AssignUnitTestAttributes(string procedureName, string updateSelectStatement, bool bypassValidation)
        {
            DataRow dr = dsDefaultAttributes.Tables[0].NewRow();
            dr["procedureName"] = procedureName;
            dr["selectStatement"] = updateSelectStatement;
            dr["bypassValidation"] = bypassValidation;

            dsDefaultAttributes.Tables[0].Rows.Add(dr);
        }
        public static baseStoredProcedure GetStoredProcedure(string procedureName)
        {
            baseStoredProcedure result = storedProcedureList.Find(
              delegate(baseStoredProcedure sp)
              {
                  return sp.procedureName == procedureName;
              }
              );
            if (result != null)
            {
                return result;
            }
            else
            {
                //stored procedure was not found in the list
                return null;
            }
        }

        private static DataSet dsDataModules;
        public const string defaultModule = " ...Select a module";
        private static void FillModuleDataSetFromList()
        {
            dsDataModules.Tables[0].Rows.Clear();
            //moduleList.Sort();
            DataRow drBlank = dsDataModules.Tables[0].NewRow();
            drBlank["moduleName"] = defaultModule;
            drBlank["filePath"] = string.Empty;
            dsDataModules.Tables[0].Rows.Add(drBlank);

            int index = 0;
            foreach (projectFile pf in moduleList)
            {
                DataRow dr = dsDataModules.Tables[0].NewRow();
                dr["moduleName"] = pf.moduleName;
                dr["filePath"] = pf.filePath;
                dsDataModules.Tables[0].Rows.Add(dr);
                index++;
            }
        }
        public static DataSet GetModuleListAsDataset()
        {
            if (dsDataModules == null)
            {
                dsDataModules = new DataSet();
                dsDataModules.Tables.Add("Modules");
                dsDataModules.Tables[0].Columns.Add("moduleName");
                dsDataModules.Tables[0].Columns.Add("filePath");
                FillModuleDataSetFromList();
            }
            return dsDataModules;
        }

        public static string GetFileForModule(string sModuleName)
        {
            DataRow[] drFound = dsDataModules.Tables[0].Select("moduleName='" + sModuleName + "'");
            if (drFound.Length > 0)
            {
                return (string)drFound[0]["filePath"];
            }
            else
            {
                return string.Empty;
            }
        }
      
        private class projectFile
        {
            public string moduleName;
            public string filePath;
        }
    }

}
