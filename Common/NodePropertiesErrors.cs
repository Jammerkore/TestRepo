using System;
using System.Collections;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Common;

namespace MIDRetail.Common
{
    /// <summary>
	/// Contains the information about the eligibility for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
    public class StoreEligibilityErrors : MIDObject
    {
        bool _simStoreErr;
        bool _typeErr;
        int _storeRID;
        string _type;
        string _message;
        string _dataString; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
        StoreEligibilityList _sel; 

        public StoreEligibilityErrors()
        {
            _simStoreErr = false;
            _typeErr = false;
            _storeRID = Include.NoRID;
            _type = string.Empty;
            _message = string.Empty;
            _sel = new StoreEligibilityList(eProfileType.StoreEligibility);
        }

        /// <summary>
        /// Gets or sets the store RID on where the error occoured.
        /// </summary>
        public bool simStoreErr
        {
            get { return _simStoreErr; }
            set { _simStoreErr = value; }
        }

        /// <summary>
        /// Gets or sets the store RID on where the error occoured.
        /// </summary>
        public bool typeErr
        {
            get { return _typeErr; }
            set { _typeErr = value; }
        }

        /// <summary>
        /// Gets or sets the store RID on where the error occoured.
        /// </summary>
        public int storeRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }
        /// <summary>
        /// Gets or sets the modifier type for the store.
        /// i.e (Stock Modifier, Sales Modifier, FWOS Modifier...)
        /// </summary>
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Gets or sets the name of the message for the error.
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
		//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
        /// <summary>
        /// Gets or sets the store ID for the error.
        /// </summary>
        public string dataString
        {
            get { return _dataString; }
            set { _dataString = value; }
        }
		//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
		
        public StoreEligibilityList sel
        {
            get { return _sel; }
            set { _sel = value; }
        }
    }
    
    public class IMOErrors : MIDObject
    {
        bool _error;
        int _storeRID;
        string _message;
        string _type;
        IMOProfileList _ProfList;

        public IMOErrors()
        {
            _error = false;
            _storeRID = Include.NoRID;
            _message = string.Empty;
            _type = string.Empty;
            _ProfList = new IMOProfileList(eProfileType.IMO);
        }

        public bool Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public int StoreRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        public IMOProfileList ProfList
        {
            get { return _ProfList; }
            set { _ProfList = value; }
        }
    }

    public class IMODataSet : MIDObject
    {

        public DataSet Reservation_Define(System.Data.DataSet _imoDataSet)
        {
            try
            {
                _imoDataSet = MIDEnvironment.CreateDataSet("reservationDataSet");

                DataTable setTable = _imoDataSet.Tables.Add("Sets");

                DataColumn dataColumn;
                //Create Columns and rows for datatable

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Reservation Store";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Min Ship Qty";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Pct Pack Threshold";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                // BEGIN TT#2225 - gtaylor - ANF VSW
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);
                // END TT#2225 - gtaylor - ANF VSW

                //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Max RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);
                //END TT#108 - MD - DOConnell - FWOS Max Model

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Push to Backstock";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                //make set ID the primary key
                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                PrimaryKeyColumn[0] = setTable.Columns["SetID"];
                setTable.PrimaryKey = PrimaryKeyColumn;

                DataTable storeTable = _imoDataSet.Tables.Add("Stores");

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Updated";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Store RID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = true;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Store ID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Reservation Store";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Min Ship Qty";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Int32");
                //dataColumn.ColumnName = "Min Ship Qty Inherited RID";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Pct Pack Threshold";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Int32");
                //dataColumn.ColumnName = "Pct Pack Threshold Inherited RID";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Int32");
                //dataColumn.ColumnName = "Item Max Inherited RID";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                // BEGIN TT#2225 - gtaylor - ANF VSW
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);
                // END TT#2225 - gtaylor - ANF VSW

                //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Max RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);
                //END TT#108 - MD - DOConnell - FWOS Max Model

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Push to Backstock";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Int32");
                //dataColumn.ColumnName = "Push to Backstock Inherited RID";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                _imoDataSet.Relations.Add("Stores",
                    _imoDataSet.Tables["Sets"].Columns["SetID"],
                    _imoDataSet.Tables["Stores"].Columns["SetID"]);
            }
            catch
            {
                throw;
            }
            return _imoDataSet;
        }
    }

}