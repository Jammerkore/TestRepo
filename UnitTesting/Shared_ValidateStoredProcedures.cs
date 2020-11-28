using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;

namespace UnitTesting
{
    public static class Shared_ValidateStoredProcedures
    {
     


       


        /// <summary>
        /// Returns "OK" if the stored procedure is valid, otherwise returns the check# that failed
        /// </summary>
        /// <param name="hasNoLock"></param>
        /// <param name="hasRowLock"></param>
        /// <returns></returns>
        public static string ValidateStoredProcedure(string procedureName, out bool hasNoLock, out bool hasRowLock, out bool hasOrderBy)
        {
            //go thru a series of checks and place the string result into the "validation" column

            //foreach (DataRow dr in dsStoredProcedures.Tables[0].Rows)
            //{
            string result = "OK";
            bool isOK = true;
            hasNoLock = false;
            hasRowLock = false;
            hasOrderBy = false;
            //string procedureName = (string)dr["procedureName"];

            //Check #1 - Ensure procedure exists in project
            string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\" + procedureName + ".SQL";

            if (Shared_BaseStoredProcedures.BypassValidation(procedureName) == true)
            {
                return result;
            }

            if (System.IO.File.Exists(sPath) == false)
            {
                result = "Check #1 Failed: Procedure not in project.";
                isOK = false;
            }

            string procedureBody = string.Empty;
            if (isOK && procedureName.StartsWith("SP_") == false)
            {
                //Check #2 - Ensure parameters match
                procedureBody = System.IO.File.ReadAllText(sPath);



                List<sqlParms> parmListFromBody = GetSqlParmList(procedureBody);

                MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                foreach (MIDRetail.Data.baseParameter bp in sp.inputParameterList)
                {
                    bool foundParmInBody = false;
                    int i = 0;
                    while (foundParmInBody == false && i <= parmListFromBody.Count - 1)
                    {
                        if (bp.parameterName == ("@" + parmListFromBody[i].paramName))
                        {
                            foundParmInBody = true;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    if (foundParmInBody == false)
                    {
                        result = "Check #2 Failed: Parameter not found. Class Parameter Name: " + bp.parameterName;
                        isOK = false;
                    }

                }
            }
            //check #3 Filename matches procedure name inside file
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string[] itemSplit = procedureBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
                if (itemName != procedureName)
                {
                    result = "Check #3 Failed: Procedure Name: " + procedureName + " does not equal name inside file: " + itemName;
                    isOK = false;
                }

            }
            //check #4 Contains NOLOCK but does not contain WITH (NOLOCK)
            //  See article: http://msdn.microsoft.com/en-us/library/ms143729%28SQL.100%29.aspx and search for table hint
            hasNoLock = false;
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string[] itemSplit = procedureBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sline in itemSplit)
                {
                    string s = sline.Trim().Replace("\t", string.Empty);
                    if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                    {
                        continue;
                    }
                    if (s.ToUpper().Contains("NOLOCK") == true && s.ToUpper().Contains("_NOLOCK") == false)
                    {
                        hasNoLock = true;
                        if (s.ToUpper().Contains("WITH (NOLOCK)") == false)
                        {
                            result = "Check #4 Failed: Contains NOLOCK, but not WITH (NOLOCK)";
                            isOK = false;
                        }
                    }
                }
            }
            //check #5 Contains ROWLOCK but does not contain WITH (ROWLOCK)
            //  See article: http://msdn.microsoft.com/en-us/library/ms143729%28SQL.100%29.aspx and search for table hint
            hasRowLock = false;
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string[] itemSplit = procedureBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sline in itemSplit)
                {
                    string s = sline.Trim().Replace("\t", string.Empty);
                    if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                    {
                        continue;
                    }
                    if (s.ToUpper().Contains("ROWLOCK") == true && s.ToUpper().Contains("_ROWLOCK") == false)
                    {
                        hasRowLock = true;
                        if (s.ToUpper().Contains("WITH (ROWLOCK)") == false)
                        {
                            result = "Check #5 Failed: Contains ROWLOCK, but not WITH (ROWLOCK)";
                            isOK = false;
                        }
                    }
                }
            }
            //check #6 Matching procedure type with procedure name
            if (isOK)
            {
                if (procedureName.StartsWith("SP") == false)
                {

                    baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    if ((bp.procedureType == storedProcedureTypes.Insert || bp.procedureType == storedProcedureTypes.InsertAndReturnRID) && procedureName.Contains("_INSERT") == false)
                    {
                        if (procedureName.Contains("DO_ANALYSIS") == false && procedureName.Contains("UPSERT") == false)
                        {
                            result = "Check #6 Failed: Type is Insert, but name does not contain INSERT";
                            isOK = false;
                        }
                    }
                    if ((bp.procedureType == storedProcedureTypes.Update) && procedureName.Contains("_UPDATE") == false)
                    {
                        if (procedureName.Contains("UPSERT") == false)
                        {
                            result = "Check #6 Failed: Type is Update, but name does not contain UPDATE";
                            isOK = false;
                        }
                    }
                    if ((bp.procedureType == storedProcedureTypes.Delete) && procedureName.Contains("_DELETE") == false)
                    {
                        result = "Check #6 Failed: Type is Delete, but name does not contain DELETE";
                        isOK = false;
                    }
                    if ((bp.procedureType == storedProcedureTypes.Read || bp.procedureType == storedProcedureTypes.ReadAsDataset) && procedureName.Contains("_READ") == false && procedureName.Contains("_GET") == false)
                    {
                        if (procedureName.Contains("PRELOAD") == false && procedureName.Contains("INSERT_SUMMARY") == false)
                        {
                            result = "Check #6 Failed: Type is Read, but name does not contain READ";
                            isOK = false;
                        }
                    }
                    if (procedureName.Contains("_INSERT") == true && bp.procedureType != storedProcedureTypes.Insert && bp.procedureType != storedProcedureTypes.InsertAndReturnRID)
                    {
                        if (procedureName.Contains("INSERT_SUMMARY") == false)
                        {
                            result = "Check #6 Failed: Name contains INSERT, but procedure type is " + bp.procedureType.ToString();
                            isOK = false;
                        }
                    }
                    if (procedureName.Contains("_UPDATE") == true && bp.procedureType != storedProcedureTypes.Update)
                    {
                        result = "Check #6 Failed: Name contains UPDATE, but procedure type is " + bp.procedureType.ToString();
                        isOK = false;
                    }
                    if (procedureName.Contains("_DELETE") == true && bp.procedureType != storedProcedureTypes.Delete)
                    {
                        if (procedureName.Contains("DELETED") == false && procedureName.Contains("UPDATE") == false && procedureName.Contains("INSERT") == false)
                        {
                            result = "Check #6 Failed: Name contains DELETE, but procedure type is " + bp.procedureType.ToString();
                            isOK = false;
                        }
                    }
                    if ((procedureName.Contains("_READ") == true || procedureName.Contains("_GET") == true) && bp.procedureType != storedProcedureTypes.Read && bp.procedureType != storedProcedureTypes.ReadAsDataset && bp.procedureType != storedProcedureTypes.ScalarValue && bp.procedureType != storedProcedureTypes.RecordCount)
                    {

                        if (procedureName.Contains("_READ") == true)
                        {
                            result = "Check #6 Failed: Name contains READ, but procedure type is " + bp.procedureType.ToString();
                        }
                        else
                        {
                            result = "Check #6 Failed: Name contains GET, but procedure type is " + bp.procedureType.ToString();
                        }
                        isOK = false;

                    }
                }
            }
            //check #7 InsertAndReturnRID must contain SCPOPE_IDENTITY
            if (isOK)
            {
                baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                if (bp.procedureType == storedProcedureTypes.InsertAndReturnRID)
                {
                    if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                    {
                        procedureBody = System.IO.File.ReadAllText(sPath);
                    }

                    if (procedureBody.ToUpper().Contains("SCOPE_IDENTITY") == false)
                    {
                        result = "Check #7 Failed: Type is InsertAndReturnRID, but procedure does not contain SCOPE_IDENTITY";
                        isOK = false;
                    }
                }
            }
            //check #8 Contains SCPOPE_IDENTITY, but type is not InsertAndReturnRID
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string exclude_flag = "BYPASS_SCOPEIDENTITY_VALIDATION";
                if (procedureBody.Contains(exclude_flag) == false)
                {
                    baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    if (procedureBody.ToUpper().Contains("SCOPE_IDENTITY") && bp.procedureType != storedProcedureTypes.InsertAndReturnRID)
                    {
                        result = "Check #8 Failed: Procedure contains SCOPE_IDENTITY, but type is not InsertAndReturnRID";
                        isOK = false;
                    }
                }
            }
            // See if the procedure has an order by clause
            hasOrderBy = false;
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string[] itemSplit = procedureBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sline in itemSplit)
                {
                    string s = sline.Trim().Replace("\t", string.Empty);
                    if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                    {
                        continue;
                    }
                    if (s.ToUpper().Contains("ORDER BY") == true)
                    {
                        hasOrderBy = true;

                    }
                }
              
            }
            //check #9 Procedure does not contain CREATE PROCEDURE [dbo].
            bool hasDBO = false;
            if (isOK)
            {
                if (procedureBody == string.Empty)  //do not read the file twice if it can be avoided
                {
                    procedureBody = System.IO.File.ReadAllText(sPath);
                }
                string[] itemSplit = procedureBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sline in itemSplit)
                {
                    string s = sline.Trim().Replace("\t", string.Empty);
                    if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                    {
                        continue;
                    }
                    if (s.ToUpper().Contains("CREATE PROCEDURE [DBO].") == true)
                    {
                        hasDBO = true;
                        
                    }
                }
                if (hasDBO == false)
                {
                    result = "Check #9 Failed: Procedure does not contain CREATE PROCEDURE [dbo].";
                    isOK = false;
                }
            }
            return result;
        }

        private class sqlParms
        {
            public string paramName;
            public string paramDBType;
            public string paramWrapperClass;
            public bool isOutput;
        }
        private static List<sqlParms> GetSqlParmList(string sProcedure)
        {
            List<sqlParms> sqlParmList = new List<sqlParms>();
            string[] sProcedureSplit = sProcedure.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            bool beforeAS = true;
            int iCounter = 0;
            while (beforeAS)
            {
                string sLine = sProcedureSplit[iCounter];
                //sLine = sLine.Trim().ToUpper();
                sLine = sLine.Trim();
                iCounter++;

                if (sLine.StartsWith("@"))
                {
                    sqlParms p = new sqlParms();

                    if (sLine.ToUpper().Contains("OUTPUT"))
                    {
                        p.isOutput = true;
                    }
                    sLine = sLine.Replace("\t", " ");
                    int spaceLen = sLine.IndexOf(' ');
                    p.paramName = sLine.Substring(1, spaceLen - 1);
                    p.paramDBType = sLine.Substring(spaceLen + 1);


                    int spaceOrEqualsLen = p.paramDBType.IndexOf(' ');
                    if (spaceOrEqualsLen == -1)
                    {
                        spaceOrEqualsLen = p.paramDBType.IndexOf('=');
                    }
                    if (spaceOrEqualsLen == -1)
                    {
                        spaceOrEqualsLen = p.paramDBType.IndexOf('(');
                    }
                    if (spaceOrEqualsLen != -1)
                    {
                        p.paramDBType = p.paramDBType.Substring(0, spaceOrEqualsLen);
                    }

                    p.paramDBType = p.paramDBType.Replace(",", string.Empty);
                    //p.paramWrapperClass = GetWrapperParameterClassFromSQLDBType(p.paramDBType);
                    sqlParmList.Add(p);
                }

                if (sLine.ToUpper() == "AS")
                {
                    beforeAS = false;
                }
            }


            return sqlParmList;
        }
       
    }

}
