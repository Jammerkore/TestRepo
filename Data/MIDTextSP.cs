using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class MIDText
    {
        protected static class StoredProcedures
        {

            public static MID_APPLICATION_TEXT_READ_ALL_LABELS_def MID_APPLICATION_TEXT_READ_ALL_LABELS = new MID_APPLICATION_TEXT_READ_ALL_LABELS_def();
			public class MID_APPLICATION_TEXT_READ_ALL_LABELS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_ALL_LABELS.SQL"

			
			    public MID_APPLICATION_TEXT_READ_ALL_LABELS_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_ALL_LABELS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_ALL_LABELS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_APPLICATION_TEXT_READ_FROM_CODE_def MID_APPLICATION_TEXT_READ_FROM_CODE = new MID_APPLICATION_TEXT_READ_FROM_CODE_def();
            public class MID_APPLICATION_TEXT_READ_FROM_CODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FROM_CODE.SQL"

                private intParameter TEXT_CODE;

                public MID_APPLICATION_TEXT_READ_FROM_CODE_def()
                {
                    base.procedureName = "MID_APPLICATION_TEXT_READ_FROM_CODE";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("APPLICATION_TEXT");
                    TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? TEXT_CODE)
                {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FROM_CODE_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

			public static MID_PRELOAD_APPLICATION_TEXT_def MID_PRELOAD_APPLICATION_TEXT = new MID_PRELOAD_APPLICATION_TEXT_def();
			public class MID_PRELOAD_APPLICATION_TEXT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRELOAD_APPLICATION_TEXT.SQL"

			
			    public MID_PRELOAD_APPLICATION_TEXT_def()
			    {
			        base.procedureName = "MID_PRELOAD_APPLICATION_TEXT";
			        base.procedureType = storedProcedureTypes.ReadAsDataset;
			        base.tableNames.Add("APPLICATION_TEXT");
			    }
			
			    public DataSet ReadAsDataSet(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PRELOAD_APPLICATION_TEXT_def))
                    {
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_def MID_APPLICATION_TEXT_READ = new MID_APPLICATION_TEXT_READ_def();
			public class MID_APPLICATION_TEXT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ.SQL"

                private intParameter TEXT_CODE;
			
			    public MID_APPLICATION_TEXT_READ_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_CODE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_SET_TEXT_def SP_MID_SET_TEXT = new SP_MID_SET_TEXT_def();
            public class SP_MID_SET_TEXT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SET_TEXT.SQL"

                private intParameter TEXT_CODE;
                private stringParameter TEXT_VALUE;
                private intParameter TEXT_MAX_LENGTH;
                private intParameter TEXT_TYPE;
                private intParameter TEXT_LEVEL;
                private intParameter TEXT_VALUE_TYPE;
                private intParameter TEXT_ORDER;
                private intParameter TEXT_UPDATE;
			
			    public SP_MID_SET_TEXT_def()
			    {
                    base.procedureName = "SP_MID_SET_TEXT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
			        TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
			        TEXT_MAX_LENGTH = new intParameter("@TEXT_MAX_LENGTH", base.inputParameterList);
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			        TEXT_VALUE_TYPE = new intParameter("@TEXT_VALUE_TYPE", base.inputParameterList);
			        TEXT_ORDER = new intParameter("@TEXT_ORDER", base.inputParameterList);
			        TEXT_UPDATE = new intParameter("@TEXT_UPDATE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TEXT_CODE,
			                      string TEXT_VALUE,
			                      int? TEXT_MAX_LENGTH,
			                      int? TEXT_TYPE,
			                      int? TEXT_LEVEL,
			                      int? TEXT_VALUE_TYPE,
			                      int? TEXT_ORDER
			                      )
			    {
                    lock (typeof(SP_MID_SET_TEXT_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.TEXT_MAX_LENGTH.SetValue(TEXT_MAX_LENGTH);
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        this.TEXT_VALUE_TYPE.SetValue(TEXT_VALUE_TYPE);
                        this.TEXT_ORDER.SetValue(TEXT_ORDER);
                        this.TEXT_UPDATE.SetValue(0);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_UPDATE_def MID_APPLICATION_TEXT_UPDATE = new MID_APPLICATION_TEXT_UPDATE_def();
			public class MID_APPLICATION_TEXT_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_UPDATE.SQL"

                private intParameter TEXT_CODE;
                private stringParameter TEXT_VALUE;
                private intParameter TEXT_LEVEL;
			
			    public MID_APPLICATION_TEXT_UPDATE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
			        TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? TEXT_CODE,
			                      string TEXT_VALUE,
			                      int? TEXT_LEVEL
			                      )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_UPDATE_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE_def MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE = new MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE_def();
			public class MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE.SQL"

                private intParameter TEXT_CODE;
			
			    public MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? TEXT_CODE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE.SQL"

			
			    public MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE.SQL"

			
			    public MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_def MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE = new MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_def();
			public class MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE.SQL"

                private intParameter TEXT_CODE_BEGIN_VALUE;
                private intParameter TEXT_CODE_END_VALUE;
			
			    public MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE_BEGIN_VALUE = new intParameter("@TEXT_CODE_BEGIN_VALUE", base.inputParameterList);
			        TEXT_CODE_END_VALUE = new intParameter("@TEXT_CODE_END_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_CODE_BEGIN_VALUE,
			                          int? TEXT_CODE_END_VALUE
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_def))
                    {
                        this.TEXT_CODE_BEGIN_VALUE.SetValue(TEXT_CODE_BEGIN_VALUE);
                        this.TEXT_CODE_END_VALUE.SetValue(TEXT_CODE_END_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE.SQL"

                private intParameter TEXT_CODE_BEGIN_VALUE;
                private intParameter TEXT_CODE_END_VALUE;
			
			    public MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE_BEGIN_VALUE = new intParameter("@TEXT_CODE_BEGIN_VALUE", base.inputParameterList);
			        TEXT_CODE_END_VALUE = new intParameter("@TEXT_CODE_END_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_CODE_BEGIN_VALUE,
			                          int? TEXT_CODE_END_VALUE
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE_def))
                    {
                        this.TEXT_CODE_BEGIN_VALUE.SetValue(TEXT_CODE_BEGIN_VALUE);
                        this.TEXT_CODE_END_VALUE.SetValue(TEXT_CODE_END_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_LEVEL_def MID_APPLICATION_TEXT_READ_LEVEL = new MID_APPLICATION_TEXT_READ_LEVEL_def();
			public class MID_APPLICATION_TEXT_READ_LEVEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_LEVEL.SQL"

                private intParameter TEXT_CODE;
			
			    public MID_APPLICATION_TEXT_READ_LEVEL_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_LEVEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_CODE = new intParameter("@TEXT_CODE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_CODE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_LEVEL_def))
                    {
                        this.TEXT_CODE.SetValue(TEXT_CODE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
                private intParameter TEXT_LEVEL;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          int? TEXT_LEVEL
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
                private intParameter TEXT_LEVEL;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          int? TEXT_LEVEL
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
                private intParameter TEXT_LEVEL;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			        TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          int? TEXT_LEVEL,
			                          DataTable TEXT_CODE_EXCLUDE_LIST
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
                private intParameter TEXT_LEVEL;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_LEVEL = new intParameter("@TEXT_LEVEL", base.inputParameterList);
			        TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          int? TEXT_LEVEL,
			                          DataTable TEXT_CODE_EXCLUDE_LIST
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_LEVEL.SetValue(TEXT_LEVEL);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE_def();
            public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;

                public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE_def()
                {
                    base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_TEXT");
                    TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
                    TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? TEXT_TYPE,
                                      DataTable TEXT_CODE_EXCLUDE_LIST
                                      )
                {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE_def();
            public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;

                public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE_def()
                {
                    base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_TEXT");
                    TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
                    TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? TEXT_TYPE,
                                      DataTable TEXT_CODE_EXCLUDE_LIST
                                      )
                {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_TYPE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
			
			    public MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_TYPE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;
			
			    public MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          DataTable TEXT_CODE_EXCLUDE_LIST
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
                private tableParameter TEXT_CODE_EXCLUDE_LIST;
			
			    public MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			        TEXT_CODE_EXCLUDE_LIST = new tableParameter("@TEXT_CODE_EXCLUDE_LIST", "TEXT_CODE_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TEXT_TYPE,
			                          DataTable TEXT_CODE_EXCLUDE_LIST
			                          )
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        this.TEXT_CODE_EXCLUDE_LIST.SetValue(TEXT_CODE_EXCLUDE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE_def MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE = new MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE_def();
			public class MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE.SQL"

                private intParameter TEXT_TYPE;
			
			    public MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_TYPE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE_def MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE = new MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE_def();
			public class MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE.SQL"

                private intParameter TEXT_TYPE;
			
			    public MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE_def()
			    {
			        base.procedureName = "MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_TEXT");
			        TEXT_TYPE = new intParameter("@TEXT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TEXT_TYPE)
			    {
                    lock (typeof(MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE_def))
                    {
                        this.TEXT_TYPE.SetValue(TEXT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
