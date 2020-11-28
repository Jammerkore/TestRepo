using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for SizeModelData.
	/// </summary>
	public partial class SizeModelData : DataLayer
	{
		private DataSet _ChildDataDS;

		public SizeModelData() : base()
		{
		}

		#region Size Constraint Model
		public DataTable SizeConstraintModel_Read()
		{
			try
			{	
                // MID Track #5240 - Size Curve Delete too slow; new db constraint requires null for 
                //                         SIZE_CURVE_GROUP_RID instead of -1
                return StoredProcedures.MID_SIZE_CONSTRAINT_MODEL_READ_ALL.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

		public DataTable SizeConstraintModel_Read(int sizeConstraintRid)
		{
			try
			{	// MID Track #5240 - Size Curve Delete too slow; new db constraint requires null for 
				//                         SIZE_CURVE_GROUP_RID instead of -1
		
                 DataSet dsValues = StoredProcedures.MID_SIZE_CONSTRAINT_MODELS_GET.ReadAsDataSet(_dba, SIZE_CONSTRAINT_RID: sizeConstraintRid);
                 return dsValues.Tables[0];
				// END MID Track #5240
			}
			catch
			{
				throw;
			}
		}

		// BEGION ANF Generic Size Constraint
       

        /// <summary>
        /// Currently, MID database use the default collation: latin1_general_cs_as
        /// the CS stands for case sensistive (AS stands for accent sensitive), which means by default all char, varchar, and text comparisons are case sensitive
        /// This would be the fastest method to call since this is the default for the database.
        /// </summary>
        /// <param name="sizeConstraintName"></param>
        /// <returns></returns>
        public DataTable SizeConstraintModel_FilterReadCaseSensitive(string sizeConstraintName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME.Read(_dba, SIZE_CONSTRAINT_NAME: sizeConstraintName);             
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Slower than the case sensitive method above - converting either using "UPPER" or "collate SQL_Latin1_General_CP1_CI_AS" requires a full table scan
        /// </summary>
        /// <param name="sizeConstraintName"></param>
        /// <returns></returns>
        public DataTable SizeConstraintModel_FilterReadCaseInsensitive(string sizeConstraintName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER.Read(_dba, SIZE_CONSTRAINT_NAME: sizeConstraintName);
            }
            catch
            {
                throw;
            }
        }

		public DataTable SizeConstraintModel_Read(string sizeConstraintName)
		{
			try
			{	
                // MID Track #5240 - Size Curve Delete too slow; new db constraint requires null for 
                //                         SIZE_CURVE_GROUP_RID instead of -1
                return StoredProcedures.MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT.Read(_dba, SIZE_CONSTRAINT_NAME: sizeConstraintName);
			}
			catch
			{
				throw;
			}
		}
		// END ANF Generic Size Constraint

		// begin MID Track 4372 Generic Size Constraints
		public int GetSizeConstraintModelRID(string aSizeConstraintName)
		{
			try
			{
                DataTable dt = SizeConstraintModel_Read(aSizeConstraintName);
				
				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["SIZE_CONSTRAINT_RID"]));
				}
				else
				{
					return Include.NoRID;
				} 
			}
			catch
			{
				throw;
			}
		}
		// end MID Track 4372 Generic Size Constraints



		public int SizeConstraintModel_Update_Insert(int sizeConstraintModelKey, string name, int sizeGroupKey, int sizeCurveGroupKey, int storeGroupKey, TransactionData td)
		{
			try
			{
                int? SIZECURVEGROUPRID_Nullable = null;
                if (sizeCurveGroupKey != -1) SIZECURVEGROUPRID_Nullable = sizeCurveGroupKey;

                return StoredProcedures.SP_MID_SZ_CONSTR_MODEL_UP_INS.InsertAndReturnRID(td.DBA,
                                                                                          RID: sizeConstraintModelKey,
                                                                                          NAME: name,
                                                                                          SIZEGROUPRID: sizeGroupKey,
                                                                                          SIZECURVEGROUPRID: SIZECURVEGROUPRID_Nullable,
                                                                                          SGRID: storeGroupKey
                                                                                          );
			}
			catch
			{
				throw;
			}
		}

		public void SizeConstraintModel_Delete(int sizeConstraintKey)
		{
			try
			{	
				_dba.OpenUpdateConnection();
                StoredProcedures.MID_SIZE_CONSTRAINT_MODEL_DELETE.Delete(_dba, SIZE_CONSTRAINT_RID: sizeConstraintKey);
				_dba.CommitData();
				_dba.CloseUpdateConnection();
			}
			catch
			{
				throw;
			}
		}

		public void SizeConstraintModel_Delete(int sizeConstraintKey, TransactionData td)
		{
			try
			{	
                StoredProcedures.MID_SIZE_CONSTRAINT_MODEL_DELETE.Delete(td.DBA, SIZE_CONSTRAINT_RID: sizeConstraintKey);
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Deletes data associated with the size constrant grid
		/// </summary>
		/// <param name="method_RID">ID of the method to delete</param>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <returns></returns>
		virtual public bool DeleteSizeConstraintChildren(int sizeConstraintKey, TransactionData td)
		{
			bool Successfull = true;
			try
			{
                //string SqlStatement = "DELETE FROM SIZE_CONSTRAINT_MINMAX WHERE SIZE_CONSTRAINT_RID = " + sizeConstraintKey.ToString(CultureInfo.CurrentUICulture);
                //td.ExecuteNonQuery(SqlStatement);
                StoredProcedures.MID_SIZE_CONSTRAINT_MINMAX_DELETE.Delete(td.DBA, SIZE_CONSTRAINT_RID: sizeConstraintKey);

				//SqlStatement = "DELETE FROM SIZE_CONSTRAINT_GRPLVL WHERE SIZE_CONSTRAINT_RID = " + sizeConstraintKey.ToString(CultureInfo.CurrentUICulture);
				//td.ExecuteNonQuery(SqlStatement);
                StoredProcedures.MID_SIZE_CONSTRAINT_GRPLVL_DELETE.Delete(td.DBA, SIZE_CONSTRAINT_RID: sizeConstraintKey);

				Successfull = true;
			}
			catch(Exception)
			{
				throw;
			}
			
			return Successfull;

		}

		/// <summary>
		/// Initializes the child data used in the datagrid for the Fill Size Holes Method.
		/// </summary>
		/// <param name="method_RID"></param>
		/// <returns></returns>
		// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		//virtual public DataSet GetChildData(int sizeConstraintRID, int sg_RID, bool getSizes)
        virtual public DataSet GetChildData(int sizeConstraintRID, int sg_RID, bool getSizes, int SG_Version)
		// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		{
			_ChildDataDS = MIDEnvironment.CreateDataSet();
			_ChildDataDS.EnforceConstraints = true;
			// BEGIN TT#827 - MD - stodd - performance
			// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
			//_ChildDataDS = SetupMinMaxData(sizeConstraintRID, sg_RID);
            _ChildDataDS = SetupMinMaxData(sizeConstraintRID, sg_RID, SG_Version);
			// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

			////GROUP LEVEL
			//_ChildDataDS.Tables.Add(SetupGroupLevelTable(sizeConstraintRID, sg_RID));
				
			////ALL COLOR
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.AllColor));

			//////COLOR
			////_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.Color));

			////			if (getSizes)
			////			{
			////ALL COLOR SIZE
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.AllColorSize));
			////COLOR SIZE
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.ColorSize));

			////COLOR SIZE DIMENSION
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.ColorSizeDimension));
			////COLOR
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.Color));

			////ALL COLOR SIZE DIMENSION
			//_ChildDataDS.Tables.Add(SetupMinMaxData(sizeConstraintRID, sg_RID, eSizeMethodRowType.AllColorSizeDimension));
			////			}
			// END TT#827 - MD - stodd - performance

			//CREATE THE RELATIONSHIPS FOR DATA HIERARCHY
			//BuildChildDataRelationships();
			CreatePrimaryKeys();
			CreateForeignKeys();
			CreateRelationships();

			//BuildChildDataRelationships(getSizes);

			//REMOVE RELATIONS IF SIZE IS NOT AVAILABLE
			if (!getSizes)
			{
				_ChildDataDS.Relations.Remove("ColorSize");
				_ChildDataDS.Relations.Remove("AllColorSize");
				_ChildDataDS.Relations.Remove("AllColorSizeDimension");
				_ChildDataDS.Relations.Remove("ColorSizeDimension");
			}


			return _ChildDataDS;

		}


		/// <summary>
		/// Creates primary keys on the size method constraint data.
		/// </summary>
		virtual public void CreatePrimaryKeys()
		{
			DataTable dt;

			dt = (DataTable)_ChildDataDS.Tables["SetLevel"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["AllColor"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["Color"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["AllColorSize"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"],
												 dt.Columns["SIZE_CODE_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["ColorSize"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"],
												 dt.Columns["SIZE_CODE_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["ColorSizeDimension"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"]};

            dt = (DataTable)_ChildDataDS.Tables["AllColorSizeDimension"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"]};
			

		}


		/// <summary>
		/// Creates foreign keys on the size method constraint data.
		/// </summary>
		virtual public void CreateForeignKeys()
		{
			//CREATE CONSTRAINTS
			ForeignKeyConstraint fkc;

			//SETALLCOLOR
			fkc = new ForeignKeyConstraint("SetAllColor", 
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 }, 
				new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"] 
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["AllColor"].Constraints.Add(fkc);

			//SETCOLOR
			fkc = new ForeignKeyConstraint("SetColor", 
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 }, 
				new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["Color"].Constraints.Add(fkc);

			//ALLCOLORSIZE
			fkc = new ForeignKeyConstraint("AllColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"]
								 }, 
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSize"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["AllColorSize"].Columns["DIMENSIONS_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["AllColorSize"].Constraints.Add(fkc);

			//COLORSIZE
			fkc = new ForeignKeyConstraint("ColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"]
								 }, 
				new DataColumn[] {	_ChildDataDS.Tables["ColorSize"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["ColorSize"].Columns["DIMENSIONS_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["ColorSize"].Constraints.Add(fkc);

			//COLORSIZEDIMENSION
			fkc = new ForeignKeyConstraint("ColorSizeDimension", 
				new DataColumn[] {	_ChildDataDS.Tables["Color"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["Color"].Columns["COLOR_CODE_RID"]
								 }, 
				new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["ColorSizeDimension"].Constraints.Add(fkc);

			//ALLCOLORSIZEDIMENSION
			fkc = new ForeignKeyConstraint("AllColorSizeDimension", 
				new DataColumn[] {	_ChildDataDS.Tables["AllColor"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColor"].Columns["COLOR_CODE_RID"]
								 }, 
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_ChildDataDS.Tables["AllColorSizeDimension"].Constraints.Add(fkc);
		}


		/// <summary>
		/// Creates relationships on the size method constraint data.
		/// </summary>
		virtual public void CreateRelationships()
		{

			//BUILD RELATIONS
			//SETALLCOLOR
			_ChildDataDS.Relations.Add(new DataRelation("SetAllColor",
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"]
								 }));
			//SETCOLOR
			_ChildDataDS.Relations.Add(new DataRelation("SetColor",
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"]
								 }));

			//ALLCOLORSIZEDIMENSION
			_ChildDataDS.Relations.Add(new DataRelation( "AllColorSizeDimension", 
				new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"], 
									 _ChildDataDS.Tables["AllColor"].Columns["COLOR_CODE_RID"] 
								 },
				new DataColumn[] { _ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"], 
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"] 
								 }));

			//COLORSIZEDIMENSION
			_ChildDataDS.Relations.Add(new DataRelation( "ColorSizeDimension", 
				new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["Color"].Columns["COLOR_CODE_RID"] 
								 },
				new DataColumn[] { _ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"] 
								 }));

			//ALLCOLORSIZE
			_ChildDataDS.Relations.Add(new DataRelation("AllColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"] 
								 },
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSize"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["AllColorSize"].Columns["DIMENSIONS_RID"] 
								 }));

			//COLORSIZE
			_ChildDataDS.Relations.Add(new DataRelation("ColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"] 
								 },
				new DataColumn[] {  _ChildDataDS.Tables["ColorSize"].Columns["SGL_RID"],
									 _ChildDataDS.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
									 _ChildDataDS.Tables["ColorSize"].Columns["DIMENSIONS_RID"] 
								 }));
		}


		/// <summary>
		/// Gets a dataSet from SIZE_MTH_CONST_MINMAX
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sg_RID"></param>
		/// <returns></returns>
		// BEGIN TT#827 - MD - stodd - performance
		// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		//virtual public DataSet SetupMinMaxData(int sizeConstraintRid, int sg_RID)
		virtual public DataSet SetupMinMaxData(int sizeConstraintRid, int sg_RID, int sg_Version)
		// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		{
			try
			{
				DataColumn column;
                DataSet dsMinmax = MIDEnvironment.CreateDataSet();
                dsMinmax = StoredProcedures.SP_MID_SIZE_CONST_GET_ALL_MINMAX.ReadAsDataSet(_dba,
                                                                                  SIZECONSTRAINTRID: sizeConstraintRid,
                                                                                  SGRID: sg_RID,
                                                                                  SG_VERSION: sg_Version  // TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                                                                                  );

				//column = dtMinmax.Columns["ROW_TYPE_ID"];
				//column.DefaultValue = RowTypeId;
                // Begin TT#3268 - JSmith - Error when creating new Size Constraint Model
                foreach (DataTable dt in dsMinmax.Tables)
                {
                    switch (dt.TableName)
                    {
                        case "AllColor":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.AllColor;
                            break;
                        case "Color":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.Color;
                            break;
                        case "AllColorSize":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.AllColorSize;
                            break;
                        case "AllColorSizeDimension":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.AllColorSizeDimension;
                            break;
                        case "ColorSize":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.ColorSize;
                            break;
                        case "ColorSizeDimension":
                            dt.Columns["ROW_TYPE_ID"].DefaultValue = eSizeMethodRowType.ColorSizeDimension;
                            break;
                    }
                }
                // End TT#3268 - JSmith - Error when creating new Size Constraint Model

				// For SetList table
				column = dsMinmax.Tables[0].Columns["BAND_DSC"];
				column.ReadOnly = true;
				// For AllColor table
				column = dsMinmax.Tables[1].Columns["BAND_DSC"];
				column.ReadOnly = true;

				// For all "size" tables
				for (int i = 2; i <= 5; i++)
				{
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = System.Type.GetType("System.Int32");
					dataColumn.ColumnName = "SIZE_SEQ";
					dataColumn.DefaultValue = 0;
					dsMinmax.Tables[i].Columns.Add(dataColumn);
					dsMinmax.Tables[i].DefaultView.Sort = "SIZE_SEQ";
				}

				return dsMinmax;
			}
			catch
			{
				throw;
			}
		}
		// END TT#827 - MD - stodd - performance

		/// <summary>
		/// Gets a datatable from SIZE_MTH_CONST_GRPLVL
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sg_RID"></param>
		/// <returns></returns>
        // Begin TT#5810 - JSmith - Size Constraint API failing
		//virtual public DataTable SetupGroupLevelTable(int sizeConstraintRid, int sg_RID)
		virtual public DataTable SetupGroupLevelTable(int sizeConstraintRid, int sg_RID, int SG_Version)
		// End TT#5810 - JSmith - Size Constraint API failing
		{
			try
			{
				DataColumn column;
				DataTable dtSetLevel = MIDEnvironment.CreateDataTable();
                dtSetLevel = StoredProcedures.SP_MID_SIZE_CONST_GET_GRPLVL.Read(_dba,
                                                                                SIZECONSTRAINTRID: sizeConstraintRid,
                                                                                SGRID: sg_RID,
                                                                                SG_VERSION: SG_Version  // TT#5810 - JSmith - Size Constraint API failing  
                                                                                );

				//Add columns to DataTable because Oracle does not support bit/boolean
				//data types.  Data will be retrieved as Char and be converted below.
				//=====================================================================
//				column = dtSetLevel.Columns.Add("FSH_IND", System.Type.GetType("System.Boolean"));
//				column.DefaultValue = true;
//
//				column = dtSetLevel.Columns.Add("FZ_IND", System.Type.GetType("System.Boolean"));
//				column.DefaultValue = false;
				//=====================================================================

				column = dtSetLevel.Columns["BAND_DSC"];
				column.ReadOnly = true;

				//Convert the char columns to boolean, set the default fill sequence.
//				foreach(DataRow dr in dtSetLevel.Rows)
//				{
//					dr["FSH_IND"] = Include.ConvertCharToBool(Convert.ToChar(dr["FILL_SIZE_HOLES_IND"]));
//					dr["FZ_IND"] = Include.ConvertCharToBool(Convert.ToChar(dr["FILL_ZEROS_IND"]));
//
////					switch (base.Method_Type_ID)
////					{
////						case eMethodType.BasisSizeAllocation:
//							dr["FILL_SEQUENCE"] = (dr["FILL_SEQUENCE"] != System.DBNull.Value) ? dr["FILL_SEQUENCE"] : (int)eBasisSizeSort.Descending;
////							break;
////						case eMethodType.FillSizeHolesAllocation:
////							dr["FILL_SEQUENCE"] = (dr["FILL_SEQUENCE"] != System.DBNull.Value) ? dr["FILL_SEQUENCE"] : (int)eFillSizeHolesSort.Ascending;
////							break;
////					}
//				}

				//COLUMNS THAT GET CONVERTED FROM CHAR TO BOOLEAN ARE NO LONGER NEEDED, TAKE 
				//THEM OFF THE TABLE.
//				dtSetLevel.Columns.Remove("FILL_SIZE_HOLES_IND");
//				dtSetLevel.Columns.Remove("FILL_ZEROS_IND");

				dtSetLevel.AcceptChanges();

				return dtSetLevel;
			}
			catch 
			{
				throw;
			}

		}


		/// <summary>
		/// Executes SP_MID_SIZE_CONST_MM_INSERT to insert constraint data.
		/// </summary>
		/// <param name="td"></param>
		/// <param name="inParams"></param>
		/// <returns></returns>
        virtual public bool ProcessMinMax(TransactionData td, int SIZECONSTRAINTRID, int SGLRID, int COLORCODERID, int SIZESRID, int SIZECODERID, int? MIN, int? MAX, int? MULT, int ROWTYPEID, int DIMENSIONS_RID)
		{
			bool Successfull = true;

			try
			{
                StoredProcedures.SP_MID_SIZE_CONST_MM_INSERT.Insert(td.DBA,
                                                                    SIZECONSTRAINTRID: SIZECONSTRAINTRID,
                                                                    SGLRID: SGLRID,
                                                                    COLORCODERID: COLORCODERID,
                                                                    SIZESRID: SIZESRID,
                                                                    SIZECODERID: SIZECODERID,
                                                                    MIN: MIN,
                                                                    MAX: MAX,
                                                                    MULT: MULT,
                                                                    ROWTYPEID: ROWTYPEID,
                                                                    DIMENSIONS_RID: DIMENSIONS_RID
                                                                    );
		
				Successfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}
			return Successfull;


		}
			

		/// <summary>
		/// Executes SP_MID_SIZE_CONST_GL_INSERT to insert constraint data.
		/// </summary>
		/// <param name="td"></param>
		/// <param name="inParams"></param>
		/// <returns></returns>
        virtual public bool ProcessGroupLevel(TransactionData td, int SIZECONSTRAINTRID, int SGLRID, int? MIN, int? MAX, int? MULT, int ROWTYPEID) //TT#1303-MD -jsobek -Size Constraints are ZERO on Save As
		{
			bool Successfull = true;

			try
			{
                StoredProcedures.SP_MID_SIZE_CONST_GL_INSERT.Insert(td.DBA,
                                                                 SIZECONSTRAINTRID: SIZECONSTRAINTRID,
                                                                 SGLRID: SGLRID,
                                                                 MIN: MIN,
                                                                 MAX: MAX,
                                                                 MULT: MULT,
                                                                 ROWTYPEID: ROWTYPEID
                                                                 );
			
				Successfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}
			return Successfull;


		}
		#endregion

		#region Size Alternate Model
		public DataTable SizeAlternateModel_Read()
		{
			try
			{
                return StoredProcedures.MID_SIZE_ALTERNATE_MODEL_READ_ALL.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

		public DataTable SizeAlternateModel_Read(int sizeAltRid)
		{
			try
			{
                return StoredProcedures.MID_SIZE_ALTERNATE_MODEL_READ.Read(_dba, SIZE_ALTERNATE_RID: sizeAltRid);
			}
			catch
			{
				throw;
			}
		}
		
		public DataTable SizeAlternateModel_Read(string sizeAltName)
		{
			try
			{
                return StoredProcedures.MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME.Read(_dba, SIZE_ALTERNATE_NAME: sizeAltName);
			}
			catch
			{
				throw;
			}
		}

		// BEGION ANF Generic Size Constraint

        public DataTable SizeAlternateModel_FilterReadCaseSensitive(string sizeAlternateName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME.Read(_dba, SIZE_ALTERNATE_NAME: sizeAlternateName);
            }
            catch
            {
                throw;
            }
        }
        public DataTable SizeAlternateModel_FilterReadCaseInsensitive(string sizeAlternateName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER.Read(_dba, SIZE_ALTERNATE_NAME: sizeAlternateName);
            }
            catch
            {
                throw;
            }
        }
		// END ANF Generic Size Constraint

		public void SizeAlternateModel_Delete(int sizeAltKey, TransactionData td)
		{
			try
			{
                StoredProcedures.MID_SIZE_ALTERNATE_MODEL_DELETE.Delete(td.DBA, SIZE_ALTERNATE_RID: sizeAltKey);
			}
			catch
			{
				throw;
			}
		}

		public int SizeAlternateModel_Insert(int sizeAltModelKey, string name, int sizeGroupKey, int altSizeGroupKey, TransactionData td)
		{
			try
			{
                return StoredProcedures.SP_MID_SZ_ALT_MODEL_UP_INS.InsertAndReturnRID(td.DBA,
                                                                               RID: sizeAltModelKey,
                                                                               NAME: name,
                                                                               PRIMSIZECURVERID: sizeGroupKey,
                                                                               ALTSIZECURVERID: altSizeGroupKey
                                                                               );
			}
			catch
			{
				throw;
			}
		}

		public DataTable SizeAltPrimarySize_Read(int sizeAltRid)
		{
			try
			{
                return StoredProcedures.MID_SIZE_ALT_PRIMARY_SIZE_READ.Read(_dba, SIZE_ALTERNATE_RID: sizeAltRid);
			}
			catch
			{
				throw;
			}
		}

		public DataTable SizeAltAlternateSize_Read(int sizeAltRid)
		{
			try
			{
                return StoredProcedures.MID_SIZE_ALT_ALTERNATE_SIZE_READ.Read(_dba, SIZE_ALTERNATE_RID: sizeAltRid);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteSizeAlternateChildren(int sizeAltKey, TransactionData td)
		{
			bool Successfull = true;
			try
			{
                StoredProcedures.MID_SIZE_ALT_ALTERNATE_SIZE_DELETE.Delete(td.DBA, SIZE_ALTERNATE_RID: sizeAltKey);

                StoredProcedures.MID_SIZE_ALT_PRIMARY_SIZE_DELETE.Delete(td.DBA, SIZE_ALTERNATE_RID: sizeAltKey);

				Successfull = true;
			}
			catch(Exception)
			{
				throw;
			}
			
			return Successfull;

		}

		public void SizeAltPrimarySize_insert(int sizeAltRid, int seq, int sizeRid, int dimRid, TransactionData td)
		{
			try
			{		
                int? SIZES_RID_Nullable = null;
                if (sizeRid != Include.NoRID) SIZES_RID_Nullable = sizeRid;
                int? DIMENSIONS_RID_Nullable = null;
                if (dimRid != Include.NoRID) DIMENSIONS_RID_Nullable = dimRid;

                StoredProcedures.MID_SIZE_ALT_PRIMARY_SIZE_INSERT.Insert(td.DBA,
                                                                         SIZE_ALTERNATE_RID: sizeAltRid,
                                                                         PRIMARY_SIZE_SEQ: seq,
                                                                         SIZES_RID: SIZES_RID_Nullable,
                                                                         DIMENSIONS_RID: DIMENSIONS_RID_Nullable
                                                                         );
			}
			catch 
			{
				throw;
			}
		}

		public void SizeAltAlternateSize_insert(int sizeAltRid, int primSeq, int altSeq, int sizeRid, int dimRid, TransactionData td)
		{
			try
			{		
                int? SIZES_RID_Nullable = null;
                if (sizeRid != Include.NoRID) SIZES_RID_Nullable = sizeRid;
                int? DIMENSIONS_RID_Nullable = null;
                if (dimRid != Include.NoRID) DIMENSIONS_RID_Nullable = dimRid;

                StoredProcedures.MID_SIZE_ALT_ALTERNATE_SIZE_INSERT.Insert(td.DBA,
                                                                           SIZE_ALTERNATE_RID: sizeAltRid,
                                                                           PRIMARY_SIZE_SEQ: primSeq,
                                                                           ALTERNATE_SIZE_SEQ: altSeq,
                                                                           SIZES_RID: SIZES_RID_Nullable,
                                                                           DIMENSIONS_RID: DIMENSIONS_RID_Nullable
                                                                           );
			}
			catch 
			{
				throw;
			}
		}
		#endregion


	}
}
