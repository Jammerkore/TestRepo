using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for CrystalHelper.
	/// </summary>
	public class CrystalHelper : System.ComponentModel.Component
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CrystalHelper(System.ComponentModel.IContainer container)
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			container.Add(this);
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public CrystalHelper()
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary> 
		/// Overrides dispose to clean up the component list.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();

		}
		#endregion
	
		public string[] m_ParaterFileds = new string[ 1001 ]; 
		public string m_ReportFileName; 
        
		public string[] ParameterFields 
		{ 
			get 
			{ 
				string[] parameterFieldsReturn = null;
				parameterFieldsReturn = m_ParaterFileds; 
				return parameterFieldsReturn;
			} 
			set 
			{ 
				m_ParaterFileds = value; 
			} 
		} 
        
		public string ReportFileName 
		{ 
			get 
			{ 
				string reportFileNameReturn = null;
				reportFileNameReturn = m_ReportFileName; 
				return reportFileNameReturn;
			} 
            
			set 
			{ 
				m_ReportFileName = value; 
			} 
		} 
        
		public CrystalDecisions.Shared.ParameterFields ParamFields = new CrystalDecisions.Shared.ParameterFields(); 
        
		public CrystalDecisions.CrystalReports.Engine.ReportDocument CryParamSet() 
		{ 
			CrystalDecisions.CrystalReports.Engine.ReportDocument crySetReturn = null;
			int Para = 0; 
			CrystalDecisions.CrystalReports.Engine.ReportDocument crReportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument(); 
			crReportDocument.Load( ReportFileName ); 
            
			CrystalDecisions.Shared.ParameterField ParamField = new CrystalDecisions.Shared.ParameterField(); 
			CrystalDecisions.Shared.ParameterDiscreteValue DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
            
			CrystalDecisions.CrystalReports.Engine.DataDefinition transTemp0 = crReportDocument.DataDefinition;
			string[] ArrValues = null; 
			int ParamRow = 0; 
			for ( Para=0; Para<=transTemp0.ParameterFields.Count - 1; Para++ ) 
			{ 
                
				for ( ParamRow=0; ParamRow<=Information.UBound( ParameterFields, 1 ); ParamRow++ ) 
				{ 
					ArrValues = Strings.Split( ParameterFields[ ParamRow ], ";", -1, (Microsoft.VisualBasic.CompareMethod)(0) ); 
					if ( transTemp0.ParameterFields[ Para ].Name == ArrValues[ 0 ] ) 
					{ 
						//  Set name of parameter field, this must match a parameter in report.
						ParamField.ParameterFieldName = transTemp0.ParameterFields[ Para ].Name; 
                        
						switch ( transTemp0.ParameterFields[ Para ].ParameterValueKind ) 
						{
							case CrystalDecisions.Shared.ParameterValueKind.BooleanParameter:
								// "Boolean"
								DiscreteVal.Value = bool.Parse( ArrValues[ 1 ] ); 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.CurrencyParameter:
								// "Currency"
								DiscreteVal.Value = ArrValues[ 1 ]; 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.DateParameter:
								// "Date"
								DiscreteVal.Value = DateTime.Parse( ArrValues[ 1 ] ); 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter:
								// "DateTime"
								DiscreteVal.Value = DateTime.Parse( ArrValues[ 1 ] ); 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.NumberParameter:
								// "Number"
								DiscreteVal.Value = int.Parse( ArrValues[ 1 ] ); 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.StringParameter:
								// "String"
								DiscreteVal.Value = ArrValues[ 1 ]; 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
							case CrystalDecisions.Shared.ParameterValueKind.TimeParameter:
								// "Time"
								DiscreteVal.Value = DateTime.Parse( ArrValues[ 1 ] ); 
								ParamField.CurrentValues.Add( DiscreteVal ); 
                                
								//  Add parameter to parameter fields collection.
								ParamFields.Add( ParamField ); 
								DiscreteVal = new CrystalDecisions.Shared.ParameterDiscreteValue(); 
								ParamField = new CrystalDecisions.Shared.ParameterField(); 
								break;
						}
                        
					} 
				} 
			}            
			crySetReturn = crReportDocument; 
			return crySetReturn;
		} 
        	
	
	}
}
