using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ThemeData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_THEME_READ_def MID_THEME_READ = new MID_THEME_READ_def();
			public class MID_THEME_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_THEME_READ.SQL"

			    private intParameter THEME_RID;
			
			    public MID_THEME_READ_def()
			    {
			        base.procedureName = "MID_THEME_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("THEME");
			        THEME_RID = new intParameter("@THEME_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? THEME_RID)
			    {
                    lock (typeof(MID_THEME_READ_def))
                    {
                        this.THEME_RID.SetValue(THEME_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_THEME_READ_FROM_USER_def MID_THEME_READ_FROM_USER = new MID_THEME_READ_FROM_USER_def();
			public class MID_THEME_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_THEME_READ_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_THEME_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_THEME_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("THEME");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_THEME_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_THEME_UPDATE_def MID_THEME_UPDATE = new MID_THEME_UPDATE_def();
			public class MID_THEME_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_THEME_UPDATE.SQL"

                private intParameter THEME_RID;
                private stringParameter THEMENAME;
                private intParameter VIEWSTYLE;
                private intParameter CELLBORDERSTYLE;
                private intParameter CELLBORDERCOLOR;
                private intParameter DIVWIDTH;
                private charParameter DISPLAYROWGROUPDIV;
                private intParameter ROWGRPROWHDRDIVCOLOR;
                private intParameter ROWGRPROWHDRDIVBRUSHCOLOR;
                private intParameter ROWGRPDIVCOLOR;
                private intParameter ROWGRPDIVBRUSHCOLOR;
                private charParameter DISPLAYCOLUMNGROUPDIV;
                private intParameter COLGRPDIVCOLOR;
                private intParameter COLGRPDIVBRUSHCOLOR;
                private stringParameter NODEDESCFONTFAMILY;
                private floatParameter NODEDESCFONTSIZE;
                private intParameter NODEDESCFONTSTYLE;
                private intParameter NODEDESCTEXTEFFECTS;
                private stringParameter COLGRPHDRFONTFAMILY;
                private floatParameter COLGRPHDRFONTSIZE;
                private intParameter COLGRPHDRFONTSTYLE;
                private intParameter COLGRPHDRTEXTEFFECTS;
                private stringParameter COLHDRFONTFAMILY;
                private floatParameter COLHDRFONTSIZE;
                private intParameter COLHDRFONTSTYLE;
                private intParameter COLHDRTEXTEFFECTS;
                private stringParameter ROWHDRFONTFAMILY;
                private floatParameter ROWHDRFONTSIZE;
                private intParameter ROWHDRFONTSTYLE;
                private stringParameter DISPLAYONLYFONTFAMILY;
                private floatParameter DISPLAYONLYFONTSIZE;
                private intParameter DISPLAYONLYFONTSTYLE;
                private stringParameter EDITABLEFONTFAMILY;
                private floatParameter EDITABLEFONTSIZE;
                private intParameter EDITABLEFONTSTYLE;
                private stringParameter INELIGSTRFONTFAMILY;
                private floatParameter INELIGSTRFONTSIZE;
                private intParameter INELIGSTRFONTSTYLE;
                private stringParameter LOCKEDFONTFAMILY;
                private floatParameter LOCKEDFONTSIZE;
                private intParameter LOCKEDFONTSTYLE;
                private intParameter NODEDESCFORECOLOR;
                private intParameter COLGRPHDRFORECOLOR;
                private intParameter COLHDRFORECOLOR;
                private intParameter NEGATIVEFORECOLOR;
                private intParameter NODEDESCBACKCOLOR;
                private intParameter COLGRPHDRBACKCOLOR;
                private intParameter COLHDRBACKCOLOR;
                private intParameter STRDETROWHEADERFORECOLOR;
                private intParameter STRDETFORECOLOR;
                private intParameter STRSETROWHEADERFORECOLOR;
                private intParameter STRSETFORECOLOR;
                private intParameter STRTOTROWHDRFORECOLOR;
                private intParameter STRTOTALFORECOLOR;
                private intParameter STRDETROWHEADERBACKCOLOR;
                private intParameter STRDETROWHEADERALTCOLOR;
                private intParameter STRDETBACKCOLOR;
                private intParameter STRDETALTCOLOR;
                private intParameter STRSETROWHEADERBACKCOLOR;
                private intParameter STRSETROWHEADERALTCOLOR;
                private intParameter STRSETBACKCOLOR;
                private intParameter STRSETALTCOLOR;
                private intParameter STRTOTROWHDRBACKCOLOR;
                private intParameter STRTOTROWHDRALTCOLOR;
                private intParameter STRTOTALBACKCOLOR;
                private intParameter STRTOTALALTCOLOR;
			
			    public MID_THEME_UPDATE_def()
			    {
			        base.procedureName = "MID_THEME_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("THEME");
			        THEME_RID = new intParameter("@THEME_RID", base.inputParameterList);
			        THEMENAME = new stringParameter("@THEMENAME", base.inputParameterList);
			        VIEWSTYLE = new intParameter("@VIEWSTYLE", base.inputParameterList);
			        CELLBORDERSTYLE = new intParameter("@CELLBORDERSTYLE", base.inputParameterList);
			        CELLBORDERCOLOR = new intParameter("@CELLBORDERCOLOR", base.inputParameterList);
			        DIVWIDTH = new intParameter("@DIVWIDTH", base.inputParameterList);
			        DISPLAYROWGROUPDIV = new charParameter("@DISPLAYROWGROUPDIV", base.inputParameterList);
			        ROWGRPROWHDRDIVCOLOR = new intParameter("@ROWGRPROWHDRDIVCOLOR", base.inputParameterList);
			        ROWGRPROWHDRDIVBRUSHCOLOR = new intParameter("@ROWGRPROWHDRDIVBRUSHCOLOR", base.inputParameterList);
			        ROWGRPDIVCOLOR = new intParameter("@ROWGRPDIVCOLOR", base.inputParameterList);
			        ROWGRPDIVBRUSHCOLOR = new intParameter("@ROWGRPDIVBRUSHCOLOR", base.inputParameterList);
			        DISPLAYCOLUMNGROUPDIV = new charParameter("@DISPLAYCOLUMNGROUPDIV", base.inputParameterList);
			        COLGRPDIVCOLOR = new intParameter("@COLGRPDIVCOLOR", base.inputParameterList);
			        COLGRPDIVBRUSHCOLOR = new intParameter("@COLGRPDIVBRUSHCOLOR", base.inputParameterList);
			        NODEDESCFONTFAMILY = new stringParameter("@NODEDESCFONTFAMILY", base.inputParameterList);
			        NODEDESCFONTSIZE = new floatParameter("@NODEDESCFONTSIZE", base.inputParameterList);
			        NODEDESCFONTSTYLE = new intParameter("@NODEDESCFONTSTYLE", base.inputParameterList);
			        NODEDESCTEXTEFFECTS = new intParameter("@NODEDESCTEXTEFFECTS", base.inputParameterList);
			        COLGRPHDRFONTFAMILY = new stringParameter("@COLGRPHDRFONTFAMILY", base.inputParameterList);
			        COLGRPHDRFONTSIZE = new floatParameter("@COLGRPHDRFONTSIZE", base.inputParameterList);
			        COLGRPHDRFONTSTYLE = new intParameter("@COLGRPHDRFONTSTYLE", base.inputParameterList);
			        COLGRPHDRTEXTEFFECTS = new intParameter("@COLGRPHDRTEXTEFFECTS", base.inputParameterList);
			        COLHDRFONTFAMILY = new stringParameter("@COLHDRFONTFAMILY", base.inputParameterList);
			        COLHDRFONTSIZE = new floatParameter("@COLHDRFONTSIZE", base.inputParameterList);
			        COLHDRFONTSTYLE = new intParameter("@COLHDRFONTSTYLE", base.inputParameterList);
			        COLHDRTEXTEFFECTS = new intParameter("@COLHDRTEXTEFFECTS", base.inputParameterList);
			        ROWHDRFONTFAMILY = new stringParameter("@ROWHDRFONTFAMILY", base.inputParameterList);
			        ROWHDRFONTSIZE = new floatParameter("@ROWHDRFONTSIZE", base.inputParameterList);
			        ROWHDRFONTSTYLE = new intParameter("@ROWHDRFONTSTYLE", base.inputParameterList);
			        DISPLAYONLYFONTFAMILY = new stringParameter("@DISPLAYONLYFONTFAMILY", base.inputParameterList);
			        DISPLAYONLYFONTSIZE = new floatParameter("@DISPLAYONLYFONTSIZE", base.inputParameterList);
			        DISPLAYONLYFONTSTYLE = new intParameter("@DISPLAYONLYFONTSTYLE", base.inputParameterList);
			        EDITABLEFONTFAMILY = new stringParameter("@EDITABLEFONTFAMILY", base.inputParameterList);
			        EDITABLEFONTSIZE = new floatParameter("@EDITABLEFONTSIZE", base.inputParameterList);
			        EDITABLEFONTSTYLE = new intParameter("@EDITABLEFONTSTYLE", base.inputParameterList);
			        INELIGSTRFONTFAMILY = new stringParameter("@INELIGSTRFONTFAMILY", base.inputParameterList);
			        INELIGSTRFONTSIZE = new floatParameter("@INELIGSTRFONTSIZE", base.inputParameterList);
			        INELIGSTRFONTSTYLE = new intParameter("@INELIGSTRFONTSTYLE", base.inputParameterList);
			        LOCKEDFONTFAMILY = new stringParameter("@LOCKEDFONTFAMILY", base.inputParameterList);
			        LOCKEDFONTSIZE = new floatParameter("@LOCKEDFONTSIZE", base.inputParameterList);
			        LOCKEDFONTSTYLE = new intParameter("@LOCKEDFONTSTYLE", base.inputParameterList);
			        NODEDESCFORECOLOR = new intParameter("@NODEDESCFORECOLOR", base.inputParameterList);
			        COLGRPHDRFORECOLOR = new intParameter("@COLGRPHDRFORECOLOR", base.inputParameterList);
			        COLHDRFORECOLOR = new intParameter("@COLHDRFORECOLOR", base.inputParameterList);
			        NEGATIVEFORECOLOR = new intParameter("@NEGATIVEFORECOLOR", base.inputParameterList);
			        NODEDESCBACKCOLOR = new intParameter("@NODEDESCBACKCOLOR", base.inputParameterList);
			        COLGRPHDRBACKCOLOR = new intParameter("@COLGRPHDRBACKCOLOR", base.inputParameterList);
			        COLHDRBACKCOLOR = new intParameter("@COLHDRBACKCOLOR", base.inputParameterList);
			        STRDETROWHEADERFORECOLOR = new intParameter("@STRDETROWHEADERFORECOLOR", base.inputParameterList);
			        STRDETFORECOLOR = new intParameter("@STRDETFORECOLOR", base.inputParameterList);
			        STRSETROWHEADERFORECOLOR = new intParameter("@STRSETROWHEADERFORECOLOR", base.inputParameterList);
			        STRSETFORECOLOR = new intParameter("@STRSETFORECOLOR", base.inputParameterList);
			        STRTOTROWHDRFORECOLOR = new intParameter("@STRTOTROWHDRFORECOLOR", base.inputParameterList);
			        STRTOTALFORECOLOR = new intParameter("@STRTOTALFORECOLOR", base.inputParameterList);
			        STRDETROWHEADERBACKCOLOR = new intParameter("@STRDETROWHEADERBACKCOLOR", base.inputParameterList);
			        STRDETROWHEADERALTCOLOR = new intParameter("@STRDETROWHEADERALTCOLOR", base.inputParameterList);
			        STRDETBACKCOLOR = new intParameter("@STRDETBACKCOLOR", base.inputParameterList);
			        STRDETALTCOLOR = new intParameter("@STRDETALTCOLOR", base.inputParameterList);
			        STRSETROWHEADERBACKCOLOR = new intParameter("@STRSETROWHEADERBACKCOLOR", base.inputParameterList);
			        STRSETROWHEADERALTCOLOR = new intParameter("@STRSETROWHEADERALTCOLOR", base.inputParameterList);
			        STRSETBACKCOLOR = new intParameter("@STRSETBACKCOLOR", base.inputParameterList);
			        STRSETALTCOLOR = new intParameter("@STRSETALTCOLOR", base.inputParameterList);
			        STRTOTROWHDRBACKCOLOR = new intParameter("@STRTOTROWHDRBACKCOLOR", base.inputParameterList);
			        STRTOTROWHDRALTCOLOR = new intParameter("@STRTOTROWHDRALTCOLOR", base.inputParameterList);
			        STRTOTALBACKCOLOR = new intParameter("@STRTOTALBACKCOLOR", base.inputParameterList);
			        STRTOTALALTCOLOR = new intParameter("@STRTOTALALTCOLOR", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? THEME_RID,
			                      string THEMENAME,
			                      int? VIEWSTYLE,
			                      int? CELLBORDERSTYLE,
			                      int? CELLBORDERCOLOR,
			                      int? DIVWIDTH,
			                      char? DISPLAYROWGROUPDIV,
			                      int? ROWGRPROWHDRDIVCOLOR,
			                      int? ROWGRPROWHDRDIVBRUSHCOLOR,
			                      int? ROWGRPDIVCOLOR,
			                      int? ROWGRPDIVBRUSHCOLOR,
			                      char? DISPLAYCOLUMNGROUPDIV,
			                      int? COLGRPDIVCOLOR,
			                      int? COLGRPDIVBRUSHCOLOR,
			                      string NODEDESCFONTFAMILY,
			                      double? NODEDESCFONTSIZE,
			                      int? NODEDESCFONTSTYLE,
			                      int? NODEDESCTEXTEFFECTS,
			                      string COLGRPHDRFONTFAMILY,
			                      double? COLGRPHDRFONTSIZE,
			                      int? COLGRPHDRFONTSTYLE,
			                      int? COLGRPHDRTEXTEFFECTS,
			                      string COLHDRFONTFAMILY,
			                      double? COLHDRFONTSIZE,
			                      int? COLHDRFONTSTYLE,
			                      int? COLHDRTEXTEFFECTS,
			                      string ROWHDRFONTFAMILY,
			                      double? ROWHDRFONTSIZE,
			                      int? ROWHDRFONTSTYLE,
			                      string DISPLAYONLYFONTFAMILY,
			                      double? DISPLAYONLYFONTSIZE,
			                      int? DISPLAYONLYFONTSTYLE,
			                      string EDITABLEFONTFAMILY,
			                      double? EDITABLEFONTSIZE,
			                      int? EDITABLEFONTSTYLE,
			                      string INELIGSTRFONTFAMILY,
			                      double? INELIGSTRFONTSIZE,
			                      int? INELIGSTRFONTSTYLE,
			                      string LOCKEDFONTFAMILY,
			                      double? LOCKEDFONTSIZE,
			                      int? LOCKEDFONTSTYLE,
			                      int? NODEDESCFORECOLOR,
			                      int? COLGRPHDRFORECOLOR,
			                      int? COLHDRFORECOLOR,
			                      int? NEGATIVEFORECOLOR,
			                      int? NODEDESCBACKCOLOR,
			                      int? COLGRPHDRBACKCOLOR,
			                      int? COLHDRBACKCOLOR,
			                      int? STRDETROWHEADERFORECOLOR,
			                      int? STRDETFORECOLOR,
			                      int? STRSETROWHEADERFORECOLOR,
			                      int? STRSETFORECOLOR,
			                      int? STRTOTROWHDRFORECOLOR,
			                      int? STRTOTALFORECOLOR,
			                      int? STRDETROWHEADERBACKCOLOR,
			                      int? STRDETROWHEADERALTCOLOR,
			                      int? STRDETBACKCOLOR,
			                      int? STRDETALTCOLOR,
			                      int? STRSETROWHEADERBACKCOLOR,
			                      int? STRSETROWHEADERALTCOLOR,
			                      int? STRSETBACKCOLOR,
			                      int? STRSETALTCOLOR,
			                      int? STRTOTROWHDRBACKCOLOR,
			                      int? STRTOTROWHDRALTCOLOR,
			                      int? STRTOTALBACKCOLOR,
			                      int? STRTOTALALTCOLOR
			                      )
			    {
                    lock (typeof(MID_THEME_UPDATE_def))
                    {
                        this.THEME_RID.SetValue(THEME_RID);
                        this.THEMENAME.SetValue(THEMENAME);
                        this.VIEWSTYLE.SetValue(VIEWSTYLE);
                        this.CELLBORDERSTYLE.SetValue(CELLBORDERSTYLE);
                        this.CELLBORDERCOLOR.SetValue(CELLBORDERCOLOR);
                        this.DIVWIDTH.SetValue(DIVWIDTH);
                        this.DISPLAYROWGROUPDIV.SetValue(DISPLAYROWGROUPDIV);
                        this.ROWGRPROWHDRDIVCOLOR.SetValue(ROWGRPROWHDRDIVCOLOR);
                        this.ROWGRPROWHDRDIVBRUSHCOLOR.SetValue(ROWGRPROWHDRDIVBRUSHCOLOR);
                        this.ROWGRPDIVCOLOR.SetValue(ROWGRPDIVCOLOR);
                        this.ROWGRPDIVBRUSHCOLOR.SetValue(ROWGRPDIVBRUSHCOLOR);
                        this.DISPLAYCOLUMNGROUPDIV.SetValue(DISPLAYCOLUMNGROUPDIV);
                        this.COLGRPDIVCOLOR.SetValue(COLGRPDIVCOLOR);
                        this.COLGRPDIVBRUSHCOLOR.SetValue(COLGRPDIVBRUSHCOLOR);
                        this.NODEDESCFONTFAMILY.SetValue(NODEDESCFONTFAMILY);
                        this.NODEDESCFONTSIZE.SetValue(NODEDESCFONTSIZE);
                        this.NODEDESCFONTSTYLE.SetValue(NODEDESCFONTSTYLE);
                        this.NODEDESCTEXTEFFECTS.SetValue(NODEDESCTEXTEFFECTS);
                        this.COLGRPHDRFONTFAMILY.SetValue(COLGRPHDRFONTFAMILY);
                        this.COLGRPHDRFONTSIZE.SetValue(COLGRPHDRFONTSIZE);
                        this.COLGRPHDRFONTSTYLE.SetValue(COLGRPHDRFONTSTYLE);
                        this.COLGRPHDRTEXTEFFECTS.SetValue(COLGRPHDRTEXTEFFECTS);
                        this.COLHDRFONTFAMILY.SetValue(COLHDRFONTFAMILY);
                        this.COLHDRFONTSIZE.SetValue(COLHDRFONTSIZE);
                        this.COLHDRFONTSTYLE.SetValue(COLHDRFONTSTYLE);
                        this.COLHDRTEXTEFFECTS.SetValue(COLHDRTEXTEFFECTS);
                        this.ROWHDRFONTFAMILY.SetValue(ROWHDRFONTFAMILY);
                        this.ROWHDRFONTSIZE.SetValue(ROWHDRFONTSIZE);
                        this.ROWHDRFONTSTYLE.SetValue(ROWHDRFONTSTYLE);
                        this.DISPLAYONLYFONTFAMILY.SetValue(DISPLAYONLYFONTFAMILY);
                        this.DISPLAYONLYFONTSIZE.SetValue(DISPLAYONLYFONTSIZE);
                        this.DISPLAYONLYFONTSTYLE.SetValue(DISPLAYONLYFONTSTYLE);
                        this.EDITABLEFONTFAMILY.SetValue(EDITABLEFONTFAMILY);
                        this.EDITABLEFONTSIZE.SetValue(EDITABLEFONTSIZE);
                        this.EDITABLEFONTSTYLE.SetValue(EDITABLEFONTSTYLE);
                        this.INELIGSTRFONTFAMILY.SetValue(INELIGSTRFONTFAMILY);
                        this.INELIGSTRFONTSIZE.SetValue(INELIGSTRFONTSIZE);
                        this.INELIGSTRFONTSTYLE.SetValue(INELIGSTRFONTSTYLE);
                        this.LOCKEDFONTFAMILY.SetValue(LOCKEDFONTFAMILY);
                        this.LOCKEDFONTSIZE.SetValue(LOCKEDFONTSIZE);
                        this.LOCKEDFONTSTYLE.SetValue(LOCKEDFONTSTYLE);
                        this.NODEDESCFORECOLOR.SetValue(NODEDESCFORECOLOR);
                        this.COLGRPHDRFORECOLOR.SetValue(COLGRPHDRFORECOLOR);
                        this.COLHDRFORECOLOR.SetValue(COLHDRFORECOLOR);
                        this.NEGATIVEFORECOLOR.SetValue(NEGATIVEFORECOLOR);
                        this.NODEDESCBACKCOLOR.SetValue(NODEDESCBACKCOLOR);
                        this.COLGRPHDRBACKCOLOR.SetValue(COLGRPHDRBACKCOLOR);
                        this.COLHDRBACKCOLOR.SetValue(COLHDRBACKCOLOR);
                        this.STRDETROWHEADERFORECOLOR.SetValue(STRDETROWHEADERFORECOLOR);
                        this.STRDETFORECOLOR.SetValue(STRDETFORECOLOR);
                        this.STRSETROWHEADERFORECOLOR.SetValue(STRSETROWHEADERFORECOLOR);
                        this.STRSETFORECOLOR.SetValue(STRSETFORECOLOR);
                        this.STRTOTROWHDRFORECOLOR.SetValue(STRTOTROWHDRFORECOLOR);
                        this.STRTOTALFORECOLOR.SetValue(STRTOTALFORECOLOR);
                        this.STRDETROWHEADERBACKCOLOR.SetValue(STRDETROWHEADERBACKCOLOR);
                        this.STRDETROWHEADERALTCOLOR.SetValue(STRDETROWHEADERALTCOLOR);
                        this.STRDETBACKCOLOR.SetValue(STRDETBACKCOLOR);
                        this.STRDETALTCOLOR.SetValue(STRDETALTCOLOR);
                        this.STRSETROWHEADERBACKCOLOR.SetValue(STRSETROWHEADERBACKCOLOR);
                        this.STRSETROWHEADERALTCOLOR.SetValue(STRSETROWHEADERALTCOLOR);
                        this.STRSETBACKCOLOR.SetValue(STRSETBACKCOLOR);
                        this.STRSETALTCOLOR.SetValue(STRSETALTCOLOR);
                        this.STRTOTROWHDRBACKCOLOR.SetValue(STRTOTROWHDRBACKCOLOR);
                        this.STRTOTROWHDRALTCOLOR.SetValue(STRTOTROWHDRALTCOLOR);
                        this.STRTOTALBACKCOLOR.SetValue(STRTOTALBACKCOLOR);
                        this.STRTOTALALTCOLOR.SetValue(STRTOTALALTCOLOR);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_THEME_INSERT_def SP_MID_THEME_INSERT = new SP_MID_THEME_INSERT_def();
            public class SP_MID_THEME_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_THEME_INSERT.SQL"

                private intParameter USERRID;
                private stringParameter THEMENAME;
                private intParameter VIEWSTYLE;
                private intParameter CELLBORDERSTYLE;
                private intParameter CELLBORDERCOLOR;
                private intParameter DIVWIDTH;
                private charParameter DISPLAYROWGROUPDIV;
                private intParameter ROWGRPROWHDRDIVCOLOR;
                private intParameter ROWGRPROWHDRDIVBRUSHCOLOR;
                private intParameter ROWGRPDIVCOLOR;
                private intParameter ROWGRPDIVBRUSHCOLOR;
                private charParameter DISPLAYCOLUMNGROUPDIV;
                private intParameter COLGRPDIVCOLOR;
                private intParameter COLGRPDIVBRUSHCOLOR;
                private stringParameter NODEDESCFONTFAMILY;
                private floatParameter NODEDESCFONTSIZE;
                private intParameter NODEDESCFONTSTYLE;
                private intParameter NODEDESCTEXTEFFECTS;
                private stringParameter COLGRPHDRFONTFAMILY;
                private floatParameter COLGRPHDRFONTSIZE;
                private intParameter COLGRPHDRFONTSTYLE;
                private intParameter COLGRPHDRTEXTEFFECTS;
                private stringParameter COLHDRFONTFAMILY;
                private floatParameter COLHDRFONTSIZE;
                private intParameter COLHDRFONTSTYLE;
                private intParameter COLHDRTEXTEFFECTS;
                private stringParameter ROWHDRFONTFAMILY;
                private floatParameter ROWHDRFONTSIZE;
                private intParameter ROWHDRFONTSTYLE;
                private stringParameter DISPLAYONLYFONTFAMILY;
                private floatParameter DISPLAYONLYFONTSIZE;
                private intParameter DISPLAYONLYFONTSTYLE;
                private stringParameter EDITABLEFONTFAMILY;
                private floatParameter EDITABLEFONTSIZE;
                private intParameter EDITABLEFONTSTYLE;
                private stringParameter INELIGSTRFONTFAMILY;
                private floatParameter INELIGSTRFONTSIZE;
                private intParameter INELIGSTRFONTSTYLE;
                private stringParameter LOCKEDFONTFAMILY;
                private floatParameter LOCKEDFONTSIZE;
                private intParameter LOCKEDFONTSTYLE;
                private intParameter NODEDESCFORECOLOR;
                private intParameter COLGRPHDRFORECOLOR;
                private intParameter COLHDRFORECOLOR;
                private intParameter NEGATIVEFORECOLOR;
                private intParameter NODEDESCBACKCOLOR;
                private intParameter COLGRPHDRBACKCOLOR;
                private intParameter COLHDRBACKCOLOR;
                private intParameter STRDETROWHEADERFORECOLOR;
                private intParameter STRDETFORECOLOR;
                private intParameter STRSETROWHEADERFORECOLOR;
                private intParameter STRSETFORECOLOR;
                private intParameter STRTOTROWHDRFORECOLOR;
                private intParameter STRTOTALFORECOLOR;
                private intParameter STRDETROWHEADERBACKCOLOR;
                private intParameter STRDETROWHEADERALTCOLOR;
                private intParameter STRDETBACKCOLOR;
                private intParameter STRDETALTCOLOR;
                private intParameter STRSETROWHEADERBACKCOLOR;
                private intParameter STRSETROWHEADERALTCOLOR;
                private intParameter STRSETBACKCOLOR;
                private intParameter STRSETALTCOLOR;
                private intParameter STRTOTROWHDRBACKCOLOR;
                private intParameter STRTOTROWHDRALTCOLOR;
                private intParameter STRTOTALBACKCOLOR;
                private intParameter STRTOTALALTCOLOR;
                private intParameter THEME_RID; //Declare Output Parameter

                public SP_MID_THEME_INSERT_def()
                {
                    base.procedureName = "SP_MID_THEME_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("THEME");
                    USERRID = new intParameter("@USERRID", base.inputParameterList);
                    THEMENAME = new stringParameter("@THEMENAME", base.inputParameterList);
                    VIEWSTYLE = new intParameter("@VIEWSTYLE", base.inputParameterList);
                    CELLBORDERSTYLE = new intParameter("@CELLBORDERSTYLE", base.inputParameterList);
                    CELLBORDERCOLOR = new intParameter("@CELLBORDERCOLOR", base.inputParameterList);
                    DIVWIDTH = new intParameter("@DIVWIDTH", base.inputParameterList);
                    DISPLAYROWGROUPDIV = new charParameter("@DISPLAYROWGROUPDIV", base.inputParameterList);
                    ROWGRPROWHDRDIVCOLOR = new intParameter("@ROWGRPROWHDRDIVCOLOR", base.inputParameterList);
                    ROWGRPROWHDRDIVBRUSHCOLOR = new intParameter("@ROWGRPROWHDRDIVBRUSHCOLOR", base.inputParameterList);
                    ROWGRPDIVCOLOR = new intParameter("@ROWGRPDIVCOLOR", base.inputParameterList);
                    ROWGRPDIVBRUSHCOLOR = new intParameter("@ROWGRPDIVBRUSHCOLOR", base.inputParameterList);
                    DISPLAYCOLUMNGROUPDIV = new charParameter("@DISPLAYCOLUMNGROUPDIV", base.inputParameterList);
                    COLGRPDIVCOLOR = new intParameter("@COLGRPDIVCOLOR", base.inputParameterList);
                    COLGRPDIVBRUSHCOLOR = new intParameter("@COLGRPDIVBRUSHCOLOR", base.inputParameterList);
                    NODEDESCFONTFAMILY = new stringParameter("@NODEDESCFONTFAMILY", base.inputParameterList);
                    NODEDESCFONTSIZE = new floatParameter("@NODEDESCFONTSIZE", base.inputParameterList);
                    NODEDESCFONTSTYLE = new intParameter("@NODEDESCFONTSTYLE", base.inputParameterList);
                    NODEDESCTEXTEFFECTS = new intParameter("@NODEDESCTEXTEFFECTS", base.inputParameterList);
                    COLGRPHDRFONTFAMILY = new stringParameter("@COLGRPHDRFONTFAMILY", base.inputParameterList);
                    COLGRPHDRFONTSIZE = new floatParameter("@COLGRPHDRFONTSIZE", base.inputParameterList);
                    COLGRPHDRFONTSTYLE = new intParameter("@COLGRPHDRFONTSTYLE", base.inputParameterList);
                    COLGRPHDRTEXTEFFECTS = new intParameter("@COLGRPHDRTEXTEFFECTS", base.inputParameterList);
                    COLHDRFONTFAMILY = new stringParameter("@COLHDRFONTFAMILY", base.inputParameterList);
                    COLHDRFONTSIZE = new floatParameter("@COLHDRFONTSIZE", base.inputParameterList);
                    COLHDRFONTSTYLE = new intParameter("@COLHDRFONTSTYLE", base.inputParameterList);
                    COLHDRTEXTEFFECTS = new intParameter("@COLHDRTEXTEFFECTS", base.inputParameterList);
                    ROWHDRFONTFAMILY = new stringParameter("@ROWHDRFONTFAMILY", base.inputParameterList);
                    ROWHDRFONTSIZE = new floatParameter("@ROWHDRFONTSIZE", base.inputParameterList);
                    ROWHDRFONTSTYLE = new intParameter("@ROWHDRFONTSTYLE", base.inputParameterList);
                    DISPLAYONLYFONTFAMILY = new stringParameter("@DISPLAYONLYFONTFAMILY", base.inputParameterList);
                    DISPLAYONLYFONTSIZE = new floatParameter("@DISPLAYONLYFONTSIZE", base.inputParameterList);
                    DISPLAYONLYFONTSTYLE = new intParameter("@DISPLAYONLYFONTSTYLE", base.inputParameterList);
                    EDITABLEFONTFAMILY = new stringParameter("@EDITABLEFONTFAMILY", base.inputParameterList);
                    EDITABLEFONTSIZE = new floatParameter("@EDITABLEFONTSIZE", base.inputParameterList);
                    EDITABLEFONTSTYLE = new intParameter("@EDITABLEFONTSTYLE", base.inputParameterList);
                    INELIGSTRFONTFAMILY = new stringParameter("@INELIGSTRFONTFAMILY", base.inputParameterList);
                    INELIGSTRFONTSIZE = new floatParameter("@INELIGSTRFONTSIZE", base.inputParameterList);
                    INELIGSTRFONTSTYLE = new intParameter("@INELIGSTRFONTSTYLE", base.inputParameterList);
                    LOCKEDFONTFAMILY = new stringParameter("@LOCKEDFONTFAMILY", base.inputParameterList);
                    LOCKEDFONTSIZE = new floatParameter("@LOCKEDFONTSIZE", base.inputParameterList);
                    LOCKEDFONTSTYLE = new intParameter("@LOCKEDFONTSTYLE", base.inputParameterList);
                    NODEDESCFORECOLOR = new intParameter("@NODEDESCFORECOLOR", base.inputParameterList);
                    COLGRPHDRFORECOLOR = new intParameter("@COLGRPHDRFORECOLOR", base.inputParameterList);
                    COLHDRFORECOLOR = new intParameter("@COLHDRFORECOLOR", base.inputParameterList);
                    NEGATIVEFORECOLOR = new intParameter("@NEGATIVEFORECOLOR", base.inputParameterList);
                    NODEDESCBACKCOLOR = new intParameter("@NODEDESCBACKCOLOR", base.inputParameterList);
                    COLGRPHDRBACKCOLOR = new intParameter("@COLGRPHDRBACKCOLOR", base.inputParameterList);
                    COLHDRBACKCOLOR = new intParameter("@COLHDRBACKCOLOR", base.inputParameterList);
                    STRDETROWHEADERFORECOLOR = new intParameter("@STRDETROWHEADERFORECOLOR", base.inputParameterList);
                    STRDETFORECOLOR = new intParameter("@STRDETFORECOLOR", base.inputParameterList);
                    STRSETROWHEADERFORECOLOR = new intParameter("@STRSETROWHEADERFORECOLOR", base.inputParameterList);
                    STRSETFORECOLOR = new intParameter("@STRSETFORECOLOR", base.inputParameterList);
                    STRTOTROWHDRFORECOLOR = new intParameter("@STRTOTROWHDRFORECOLOR", base.inputParameterList);
                    STRTOTALFORECOLOR = new intParameter("@STRTOTALFORECOLOR", base.inputParameterList);
                    STRDETROWHEADERBACKCOLOR = new intParameter("@STRDETROWHEADERBACKCOLOR", base.inputParameterList);
                    STRDETROWHEADERALTCOLOR = new intParameter("@STRDETROWHEADERALTCOLOR", base.inputParameterList);
                    STRDETBACKCOLOR = new intParameter("@STRDETBACKCOLOR", base.inputParameterList);
                    STRDETALTCOLOR = new intParameter("@STRDETALTCOLOR", base.inputParameterList);
                    STRSETROWHEADERBACKCOLOR = new intParameter("@STRSETROWHEADERBACKCOLOR", base.inputParameterList);
                    STRSETROWHEADERALTCOLOR = new intParameter("@STRSETROWHEADERALTCOLOR", base.inputParameterList);
                    STRSETBACKCOLOR = new intParameter("@STRSETBACKCOLOR", base.inputParameterList);
                    STRSETALTCOLOR = new intParameter("@STRSETALTCOLOR", base.inputParameterList);
                    STRTOTROWHDRBACKCOLOR = new intParameter("@STRTOTROWHDRBACKCOLOR", base.inputParameterList);
                    STRTOTROWHDRALTCOLOR = new intParameter("@STRTOTROWHDRALTCOLOR", base.inputParameterList);
                    STRTOTALBACKCOLOR = new intParameter("@STRTOTALBACKCOLOR", base.inputParameterList);
                    STRTOTALALTCOLOR = new intParameter("@STRTOTALALTCOLOR", base.inputParameterList);
                    THEME_RID = new intParameter("@THEME_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? USERRID,
                                              string THEMENAME,
                                              int? VIEWSTYLE,
                                              int? CELLBORDERSTYLE,
                                              int? CELLBORDERCOLOR,
                                              int? DIVWIDTH,
                                              char? DISPLAYROWGROUPDIV,
                                              int? ROWGRPROWHDRDIVCOLOR,
                                              int? ROWGRPROWHDRDIVBRUSHCOLOR,
                                              int? ROWGRPDIVCOLOR,
                                              int? ROWGRPDIVBRUSHCOLOR,
                                              char? DISPLAYCOLUMNGROUPDIV,
                                              int? COLGRPDIVCOLOR,
                                              int? COLGRPDIVBRUSHCOLOR,
                                              string NODEDESCFONTFAMILY,
                                              double? NODEDESCFONTSIZE,
                                              int? NODEDESCFONTSTYLE,
                                              int? NODEDESCTEXTEFFECTS,
                                              string COLGRPHDRFONTFAMILY,
                                              double? COLGRPHDRFONTSIZE,
                                              int? COLGRPHDRFONTSTYLE,
                                              int? COLGRPHDRTEXTEFFECTS,
                                              string COLHDRFONTFAMILY,
                                              double? COLHDRFONTSIZE,
                                              int? COLHDRFONTSTYLE,
                                              int? COLHDRTEXTEFFECTS,
                                              string ROWHDRFONTFAMILY,
                                              double? ROWHDRFONTSIZE,
                                              int? ROWHDRFONTSTYLE,
                                              string DISPLAYONLYFONTFAMILY,
                                              double? DISPLAYONLYFONTSIZE,
                                              int? DISPLAYONLYFONTSTYLE,
                                              string EDITABLEFONTFAMILY,
                                              double? EDITABLEFONTSIZE,
                                              int? EDITABLEFONTSTYLE,
                                              string INELIGSTRFONTFAMILY,
                                              double? INELIGSTRFONTSIZE,
                                              int? INELIGSTRFONTSTYLE,
                                              string LOCKEDFONTFAMILY,
                                              double? LOCKEDFONTSIZE,
                                              int? LOCKEDFONTSTYLE,
                                              int? NODEDESCFORECOLOR,
                                              int? COLGRPHDRFORECOLOR,
                                              int? COLHDRFORECOLOR,
                                              int? NEGATIVEFORECOLOR,
                                              int? NODEDESCBACKCOLOR,
                                              int? COLGRPHDRBACKCOLOR,
                                              int? COLHDRBACKCOLOR,
                                              int? STRDETROWHEADERFORECOLOR,
                                              int? STRDETFORECOLOR,
                                              int? STRSETROWHEADERFORECOLOR,
                                              int? STRSETFORECOLOR,
                                              int? STRTOTROWHDRFORECOLOR,
                                              int? STRTOTALFORECOLOR,
                                              int? STRDETROWHEADERBACKCOLOR,
                                              int? STRDETROWHEADERALTCOLOR,
                                              int? STRDETBACKCOLOR,
                                              int? STRDETALTCOLOR,
                                              int? STRSETROWHEADERBACKCOLOR,
                                              int? STRSETROWHEADERALTCOLOR,
                                              int? STRSETBACKCOLOR,
                                              int? STRSETALTCOLOR,
                                              int? STRTOTROWHDRBACKCOLOR,
                                              int? STRTOTROWHDRALTCOLOR,
                                              int? STRTOTALBACKCOLOR,
                                              int? STRTOTALALTCOLOR
                                              )
                {
                    lock (typeof(SP_MID_THEME_INSERT_def))
                    {
                        this.USERRID.SetValue(USERRID);
                        this.THEMENAME.SetValue(THEMENAME);
                        this.VIEWSTYLE.SetValue(VIEWSTYLE);
                        this.CELLBORDERSTYLE.SetValue(CELLBORDERSTYLE);
                        this.CELLBORDERCOLOR.SetValue(CELLBORDERCOLOR);
                        this.DIVWIDTH.SetValue(DIVWIDTH);
                        this.DISPLAYROWGROUPDIV.SetValue(DISPLAYROWGROUPDIV);
                        this.ROWGRPROWHDRDIVCOLOR.SetValue(ROWGRPROWHDRDIVCOLOR);
                        this.ROWGRPROWHDRDIVBRUSHCOLOR.SetValue(ROWGRPROWHDRDIVBRUSHCOLOR);
                        this.ROWGRPDIVCOLOR.SetValue(ROWGRPDIVCOLOR);
                        this.ROWGRPDIVBRUSHCOLOR.SetValue(ROWGRPDIVBRUSHCOLOR);
                        this.DISPLAYCOLUMNGROUPDIV.SetValue(DISPLAYCOLUMNGROUPDIV);
                        this.COLGRPDIVCOLOR.SetValue(COLGRPDIVCOLOR);
                        this.COLGRPDIVBRUSHCOLOR.SetValue(COLGRPDIVBRUSHCOLOR);
                        this.NODEDESCFONTFAMILY.SetValue(NODEDESCFONTFAMILY);
                        this.NODEDESCFONTSIZE.SetValue(NODEDESCFONTSIZE);
                        this.NODEDESCFONTSTYLE.SetValue(NODEDESCFONTSTYLE);
                        this.NODEDESCTEXTEFFECTS.SetValue(NODEDESCTEXTEFFECTS);
                        this.COLGRPHDRFONTFAMILY.SetValue(COLGRPHDRFONTFAMILY);
                        this.COLGRPHDRFONTSIZE.SetValue(COLGRPHDRFONTSIZE);
                        this.COLGRPHDRFONTSTYLE.SetValue(COLGRPHDRFONTSTYLE);
                        this.COLGRPHDRTEXTEFFECTS.SetValue(COLGRPHDRTEXTEFFECTS);
                        this.COLHDRFONTFAMILY.SetValue(COLHDRFONTFAMILY);
                        this.COLHDRFONTSIZE.SetValue(COLHDRFONTSIZE);
                        this.COLHDRFONTSTYLE.SetValue(COLHDRFONTSTYLE);
                        this.COLHDRTEXTEFFECTS.SetValue(COLHDRTEXTEFFECTS);
                        this.ROWHDRFONTFAMILY.SetValue(ROWHDRFONTFAMILY);
                        this.ROWHDRFONTSIZE.SetValue(ROWHDRFONTSIZE);
                        this.ROWHDRFONTSTYLE.SetValue(ROWHDRFONTSTYLE);
                        this.DISPLAYONLYFONTFAMILY.SetValue(DISPLAYONLYFONTFAMILY);
                        this.DISPLAYONLYFONTSIZE.SetValue(DISPLAYONLYFONTSIZE);
                        this.DISPLAYONLYFONTSTYLE.SetValue(DISPLAYONLYFONTSTYLE);
                        this.EDITABLEFONTFAMILY.SetValue(EDITABLEFONTFAMILY);
                        this.EDITABLEFONTSIZE.SetValue(EDITABLEFONTSIZE);
                        this.EDITABLEFONTSTYLE.SetValue(EDITABLEFONTSTYLE);
                        this.INELIGSTRFONTFAMILY.SetValue(INELIGSTRFONTFAMILY);
                        this.INELIGSTRFONTSIZE.SetValue(INELIGSTRFONTSIZE);
                        this.INELIGSTRFONTSTYLE.SetValue(INELIGSTRFONTSTYLE);
                        this.LOCKEDFONTFAMILY.SetValue(LOCKEDFONTFAMILY);
                        this.LOCKEDFONTSIZE.SetValue(LOCKEDFONTSIZE);
                        this.LOCKEDFONTSTYLE.SetValue(LOCKEDFONTSTYLE);
                        this.NODEDESCFORECOLOR.SetValue(NODEDESCFORECOLOR);
                        this.COLGRPHDRFORECOLOR.SetValue(COLGRPHDRFORECOLOR);
                        this.COLHDRFORECOLOR.SetValue(COLHDRFORECOLOR);
                        this.NEGATIVEFORECOLOR.SetValue(NEGATIVEFORECOLOR);
                        this.NODEDESCBACKCOLOR.SetValue(NODEDESCBACKCOLOR);
                        this.COLGRPHDRBACKCOLOR.SetValue(COLGRPHDRBACKCOLOR);
                        this.COLHDRBACKCOLOR.SetValue(COLHDRBACKCOLOR);
                        this.STRDETROWHEADERFORECOLOR.SetValue(STRDETROWHEADERFORECOLOR);
                        this.STRDETFORECOLOR.SetValue(STRDETFORECOLOR);
                        this.STRSETROWHEADERFORECOLOR.SetValue(STRSETROWHEADERFORECOLOR);
                        this.STRSETFORECOLOR.SetValue(STRSETFORECOLOR);
                        this.STRTOTROWHDRFORECOLOR.SetValue(STRTOTROWHDRFORECOLOR);
                        this.STRTOTALFORECOLOR.SetValue(STRTOTALFORECOLOR);
                        this.STRDETROWHEADERBACKCOLOR.SetValue(STRDETROWHEADERBACKCOLOR);
                        this.STRDETROWHEADERALTCOLOR.SetValue(STRDETROWHEADERALTCOLOR);
                        this.STRDETBACKCOLOR.SetValue(STRDETBACKCOLOR);
                        this.STRDETALTCOLOR.SetValue(STRDETALTCOLOR);
                        this.STRSETROWHEADERBACKCOLOR.SetValue(STRSETROWHEADERBACKCOLOR);
                        this.STRSETROWHEADERALTCOLOR.SetValue(STRSETROWHEADERALTCOLOR);
                        this.STRSETBACKCOLOR.SetValue(STRSETBACKCOLOR);
                        this.STRSETALTCOLOR.SetValue(STRSETALTCOLOR);
                        this.STRTOTROWHDRBACKCOLOR.SetValue(STRTOTROWHDRBACKCOLOR);
                        this.STRTOTROWHDRALTCOLOR.SetValue(STRTOTROWHDRALTCOLOR);
                        this.STRTOTALBACKCOLOR.SetValue(STRTOTALBACKCOLOR);
                        this.STRTOTALALTCOLOR.SetValue(STRTOTALALTCOLOR);
                        this.THEME_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_THEME_READ_THEME_RID_def MID_THEME_READ_THEME_RID = new MID_THEME_READ_THEME_RID_def();
            public class MID_THEME_READ_THEME_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_THEME_READ_THEME_RID.SQL"

                private stringParameter THEMENAME;
                private intParameter USER_RID;

                public MID_THEME_READ_THEME_RID_def()
                {
                    base.procedureName = "MID_THEME_READ_THEME_RID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("THEME");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    THEMENAME = new stringParameter("@THEMENAME", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, string THEME_NAME, int? USER_RID)
                {
                    lock (typeof(MID_THEME_READ_THEME_RID_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.THEMENAME.SetValue(THEME_NAME);
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);

                        if (dt.Rows.Count > 0)
                        {
                            return dt.Rows[0]["THEME_RID"];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                public DataTable Read(DatabaseAccess _dba, string THEME_NAME, int? USER_RID)
                {
                    lock (typeof(MID_THEME_READ_THEME_RID_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.THEMENAME.SetValue(THEME_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
