using System;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Generic class to communicate database parameter information to specific databases.
	/// </summary>
	public class MIDDbParameter
	{
		private string _paramName;
		private object _paramValue;
		private eDbType _dBType;
		private eParameterDirection _dBDirection;
		private int _size;
		private string _paramFieldName;
        private string _paramTypeName; //TT#827-MD -jsobek -Allocation Reviews Performance

		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		public MIDDbParameter ()
		{
		}

		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		/// <param name="param_Name"></param>
		/// <param name="param_Value"></param>
		public MIDDbParameter (string param_Name, object param_Value)
		{
			_paramName = param_Name;
			_paramValue = param_Value;
		}
		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		/// <param name="param_Name"></param>
		/// <param name="param_Value"></param>
		/// <param name="dB_Type"></param>
		public MIDDbParameter (string param_Name, object param_Value, eDbType dB_Type)
		{
			_paramName = param_Name;
			_paramValue = param_Value;
			_dBType = dB_Type;
		}
		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		/// <param name="param_Name"></param>
		/// <param name="param_Value"></param>
		/// <param name="dB_Type"></param>
		/// <param name="direction"></param>
		public MIDDbParameter (string param_Name, object param_Value, eDbType dB_Type, eParameterDirection direction)
		{
			_paramName = param_Name;
			_paramValue = param_Value;
			_dBType = dB_Type;
			_dBDirection = direction;
		}

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        /// <summary>
        /// Creates a new instance of MIDDbParameter
        /// </summary>
        /// <param name="param_Name"></param>
        /// <param name="param_Value"></param>
        /// <param name="dB_Type"></param>
        /// <param name="direction"></param>
        /// <param name="typeName">Used with structured parameters</param>
        public MIDDbParameter(string param_Name, object param_Value, eDbType dB_Type, eParameterDirection direction, string typeName)
        {
            _paramName = param_Name;
            _paramValue = param_Value;
            _dBType = dB_Type;
            _dBDirection = direction;
            _paramTypeName = typeName;
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		/// <param name="param_Name"></param>
		/// <param name="dB_Type"></param>
		/// <param name="size"></param>
		/// <param name="field_Name"></param>
		public MIDDbParameter (string param_Name, eDbType dB_Type, int size, string field_Name)
		{
			_paramName = param_Name;
			_dBType = dB_Type;
			_size = size;
			_paramFieldName = field_Name;
		}

		/// <summary>
		/// Creates a new instance of MIDDbParameter
		/// </summary>
		/// <param name="param_Name"></param>
		/// <param name="dB_Type"></param>
		/// <param name="size"></param>
		/// <param name="field_Name"></param>
		/// <param name="direction"></param>
		public MIDDbParameter (string param_Name, eDbType dB_Type, int size, string field_Name, eParameterDirection direction)
		{
			_paramName = param_Name;
			_dBType = dB_Type;
			_size = size;
			_paramFieldName = field_Name;
			_dBDirection = direction;
		}

		/// <summary>
		/// Gets or sets the name of the database parameter.
		/// </summary>
		public string ParameterName 
		{
			get { return _paramName ; }
			set { _paramName = value; }
		}
		/// <summary>
		/// Gets or sets the value of the database parameter.
		/// </summary>
		public object Value 
		{
			get { return _paramValue ; }
			set { _paramValue = value; }
		}
		/// <summary>
		/// Gets or sets the type of the database parameter.
		/// </summary>
		public eDbType DbType 
		{
			get { return _dBType ; }
			set { _dBType = value; }
		}
		/// <summary>
		/// Gets or sets the direction of the database parameter.
		/// </summary>
		public eParameterDirection Direction
		{
			get { return _dBDirection ; }
			set { _dBDirection = value; }
		}
		/// <summary>
		/// Gets or sets the size of the value in the database parameter.
		/// </summary>
		public int Size
		{
			get { return _size ; }
			set { _size = value; }
		}
		/// <summary>
		/// Gets or sets the parameter field name of the database parameter.
		/// </summary>
		public string ParamFieldName
		{
			get { return _paramFieldName ; }
			set { _paramFieldName = value; }
		}
        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        /// <summary>
        /// Gets or sets the parameter type name of the database parameter.
        /// Used with structured parameters
        /// </summary>
        public string TypeName
        {
            get { return _paramTypeName; }
            set { _paramTypeName = value; }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance
	}
}
