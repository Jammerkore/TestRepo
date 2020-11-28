using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

using C1.Win.C1FlexGrid;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Windows
{
	public partial class MIDExport : MIDFormBase
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private bool _OKClicked = false;
		private string _fileName = null;
		private eExportType _exportType = eExportType.Excel;
		private bool _createDirectory = false;
		private string _includeCurrentLabel = null;
		private string _includeAllLabel = null;
		private bool _openExcel = false;
		private bool _showCurrentAll = true;
		private string _fileFilter = null;
		private bool _excelInstalled = false;

		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		public MIDExport(SessionAddressBlock aSAB, string aIncludeCurrentLabel, string aIncludeAllLabel,
			bool aShowCurrentAll)
			: base(aSAB)
		{
			InitializeComponent();
			_includeCurrentLabel = aIncludeCurrentLabel;
			_includeAllLabel = aIncludeAllLabel;
			_showCurrentAll = aShowCurrentAll;
			_excelInstalled = Include.IsExcelInstalled();
		}
		#endregion Constructors

		#region Properties
		//============
		// PROPERTIES
		//============

		public bool OKClicked
		{
			get { return _OKClicked; }
		}

		public string FileName
		{
			get { return _fileName; }
		}

		public eExportType ExportType
		{
			get { return _exportType; }
		}

		public eExportData ExportData
		{
			get 
			{
				if (radAll.Checked)
				{
					return eExportData.All;
				}
				else
				{
					return eExportData.Current;
				}
			}
		}

		public bool CreateDirectory
		{
			get { return _createDirectory; }
		}

		public bool OpenExcel
		{
			get { return _openExcel; }
		}

		public bool IncludeFormatting
		{
			get { return chkIncludeFormatting.Checked; }
		}

		#endregion Properties

		private void MIDExport_Load(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				SetText();
				Format_Title(eDataState.None, eMIDTextCode.frm_Export, null);
				if (_excelInstalled)
				{
					radExcel.Checked = true;
				}
				else
				{
					radExcel.Enabled = false;
					radFile.Checked = true;
				}
				radCurrent.Checked = true;
				chkIncludeFormatting.Checked = false;
				if (!_showCurrentAll)
				{
					//gbxInclude.Visible = false;
					//Height -= gbxInclude.Height;
					radAll.Visible = false;
					radCurrent.Visible = false;
					int remove = radAll.Bottom - radCurrent.Top;
					gbxInclude.Height -= remove;
					Height -= remove;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void SetText()
		{
			try
			{
				btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				btnFileName.Text = MIDText.GetTextOnly(eMIDTextCode.menu_File_SaveAs);
				radExcel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Excel);
				radFile.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_File);
				lblFileName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Name) + ":";
				radAll.Text = _includeAllLabel;
				radCurrent.Text = _includeCurrentLabel;
				gbxLocation.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Location);
				gbxInclude.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Include);
				chkIncludeFormatting.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeFormatting);
			}
			catch 
			{
				throw;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			//Begin Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
			DirectoryInfo dirInfo;

			//End Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
			try
			{
				if (radFile.Checked)
				{
					_fileName = txtFileName.Text;
					if (_fileName == null ||
						_fileName.Trim().Length == 0)
					{
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileNameRequired), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					else
					{
						//Begin Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
						dirInfo = new DirectoryInfo(_fileName);
						if (dirInfo.Exists)
						{
							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileNameIsDirectory), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						//End Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
						int index = _fileName.LastIndexOf('\\');
						// Begin TT#703 - stodd - export abending
						//=====================================================
						// If they've entered or navigated to a directory, we use it.
						// If not, we try to use the export default file path.
						// If that's not valid, we use the current directory.
						//=====================================================
						string directory = string.Empty;
						if (index >= 0)
						{
							directory = _fileName.Substring(0, index);
						}
						else
						{
							directory = MIDConfigurationManager.AppSettings["Export_FilePath"];

							if (directory == null || directory.Trim().Length == 0)
							{
								directory = Directory.GetCurrentDirectory();
							}
							else 
							{
								directory = directory.Trim();
							}
							_fileName = directory + "\\" + _fileName;
						}
						// End TT#703
						string file = _fileName.Substring(index + 1, _fileName.Length - index - 1);
						if (file == null ||
						file.Trim().Length == 0)
						{
							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileNameRequired), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						//Begin Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
						//DirectoryInfo dirInfo = new DirectoryInfo(directory);
						dirInfo = new DirectoryInfo(directory);
						//End Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException.
						if (!dirInfo.Exists)
						{
							if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DirectoryNotExists), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
							{
								return;
							}
							_createDirectory = true;
							try
							{
								Directory.CreateDirectory(directory);
							}
							catch
							{
								string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorCreatingDirectory);
								message = message.Replace("{0}", directory);
								MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									return;
							}

						}
						else
						{
							FileInfo fileInfo = new FileInfo(_fileName);
							if (fileInfo.Exists)
							{
								string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileAlreadyExists);
								message = message.Replace("{0}", file);
								if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
								{
									return;
								}
							}
						}
					}
				}
				_OKClicked = true;
				Close();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public void AddFileFilter(eExportFilterType aExportFilterType)
		{
			try
			{
				string fileName = string.Empty;
				string fileExtension = string.Empty;
				
				switch (aExportFilterType)
				{
					case eExportFilterType.Excel:
						if (!_excelInstalled)
						{
							return;
						}
						fileName = "Microsoft Office Excel Workbook (*.xls)";
						fileExtension = "*.xls"; 
						break;
					case eExportFilterType.CSV:
						fileName = "CSV (Comma delimited)";
						fileExtension = "*.csv"; 
						break;
					case eExportFilterType.XML:
						fileName = "XML Data";
						fileExtension = "*.xml";
						break;
					case eExportFilterType.All:
						fileName = "All files (*.*)";
						fileExtension = "*.*"; 
						break;
					default:
						return;
				}

				if (_fileFilter != null)
				{
					_fileFilter += "|";
				}
				_fileFilter += fileName + "|" + fileExtension;
			}
			catch
			{
				throw;
			}
		}

		private void btnFileName_Click(object sender, EventArgs e)
		{
			try
			{
				if (_excelInstalled)
				{
					sfdFileName.DefaultExt = "xls";
					sfdFileName.FileName = "file1.xls";
				}
				else
				{
					sfdFileName.DefaultExt = "csv";
					sfdFileName.FileName = "file1.csv";
				}
				sfdFileName.Filter = _fileFilter;
				sfdFileName.FilterIndex = 1;
				sfdFileName.RestoreDirectory = true;
				sfdFileName.CheckFileExists = false;
				sfdFileName.CheckPathExists = false;
				sfdFileName.OverwritePrompt = false;

				if (sfdFileName.ShowDialog() == DialogResult.OK)
				{
					txtFileName.Text = sfdFileName.FileName;
					_fileName = sfdFileName.FileName;
					chkIncludeFormatting.Enabled = false;
					_exportType = eExportType.CSV;
					if (txtFileName.Text != null)
					{
						// only allow formatting for Excel export
						if (txtFileName.Text.Substring(txtFileName.Text.Length - 4).ToUpper() == ".XLS")
						{
							chkIncludeFormatting.Enabled = true;
							_exportType = eExportType.Excel;
						}
						else if (txtFileName.Text.Substring(txtFileName.Text.Length - 4).ToUpper() == ".XML")
						{
							chkIncludeFormatting.Enabled = false;
							chkIncludeFormatting.Checked = false;
							_exportType = eExportType.XML;
						}
						else
						{
							chkIncludeFormatting.Checked = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void radExcel_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				txtFileName.Enabled = false;
				btnFileName.Enabled = false;
				chkIncludeFormatting.Enabled = true;
				_exportType = eExportType.Excel;
				_openExcel = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void radFile_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				txtFileName.Enabled = true;
				btnFileName.Enabled = true;
				chkIncludeFormatting.Enabled = false;
				_openExcel = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	}

	public enum eExportFilterType
	{
		Excel,
		CSV,
		XML,
		All
	}

	public enum eExportData
	{
		Current,
		All
	}

	public enum eExportDataType
	{
		RowHeading,
		ColumnHeading,
		Value
	}

	abstract public class MIDExportFile
	{
		//=======
		// FIELDS
		//=======

		protected string _fileName;
		protected bool _includeFormatting;
		protected eExportType _exportType;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instace of MIDExportFile.
		/// </summary>
		/// <param name="aFileName">
		/// The name of the extract file.
		/// </param>
		/// <param name="aIncludeFormatting">
		/// A flag identifying if the file is to include the current formatting.
		/// </param>
		/// <param name="aExportType">
		/// The eExportType value identifying the type of file being exported
		/// </param>

		public MIDExportFile(string aFileName, bool aIncludeFormatting, eExportType aExportType)
		{
			_fileName = aFileName;
			_includeFormatting = aIncludeFormatting;
			_exportType = aExportType;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the name of the extract file.
		/// </summary>

		public string FileName
		{
			get
			{
				return _fileName;
			}
		}

		/// <summary>
		/// Gets the flag identifying if the export file is to include formatting.
		/// </summary>

		public bool IncludeFormatting
		{
			get
			{
				return _includeFormatting;
			}
		}

		/// <summary>
		/// Gets the eExportType value identifying the type of file being exported.
		/// </summary>

		public eExportType ExportType
		{
			get
			{
				return _exportType;
			}
		}

		//========
		// METHODS
		//========
		abstract public void OpenFile();

		abstract public void SetNumberRowsColumns(int aNumberOfRows, int aNumberOfColumns);

		abstract public void AddStyle(string aStyleName, CellStyle aCellStyle);

		abstract public void NoMoreStyles();

		abstract public void AddGrouping(string aGroupingName, int aNumberOfColumns);

		abstract public void WriteGrouping();

		abstract public void AddRow();

		abstract public void WriteRow();

		abstract public void AddValue(string aValue, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle);

		abstract public void AddValue(string aValue, bool aIsValueNumeric, bool aIsValueNegative, int aNumberOfDecimals, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle);

		abstract public void WriteFile();

		abstract public void ApplyCellFormatting(int aTopRow, int aLeftCol,
			int aBottomRow, int aRightCol, string aStyle);

	}


	public class MIDExportFlexGridToXML : MIDExportFile
	{
		//=======
		// FIELDS
		//=======
		private TextWriter _tw = null;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instace of MIDExportFlexGridToXML class to export data in XML format.
		/// </summary>
		/// <param name="aFileName">
		/// The name of the extract file.
		/// </param>
		/// <param name="aIncludeFormatting">
		/// A flag identifying if the file is to include the current formatting.
		/// </param>

		public MIDExportFlexGridToXML(string aFileName, bool aIncludeFormatting)
			: base(aFileName, aIncludeFormatting, eExportType.XML)
		{
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void OpenFile()
		{
			try
			{
				// =================
				// Open the document
				// =================
				_tw = new StreamWriter(FileName);

				// ======================
				// Write the root element
				// ======================
				_tw.WriteLine(@"<?xml version=""1.0""?>");
				_tw.WriteLine(@"<ss:Workbook xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"">");
				_tw.WriteLine(@"	<ss:Styles>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""colheading"">");
				//_tw.WriteLine(@"			<ss:Font ss:Color=""#FFFFFF""/>");
				//_tw.WriteLine(@"			<ss:Interior ss:Color=""#000000"" ss:Pattern=""Solid""/>");
				_tw.WriteLine(@"			<ss:Font ss:Color=""White""/>");
				_tw.WriteLine(@"			<ss:Interior ss:Color=""Black"" ss:Pattern=""Solid""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""rowheading"">");
				//_tw.WriteLine(@"			<ss:Font ss:Color=""#FFFFFF""/>");
				//_tw.WriteLine(@"			<ss:Interior ss:Color=""#0080FF"" ss:Pattern=""Solid""/>");
				_tw.WriteLine(@"			<ss:Font ss:Color=""White""/>");
				_tw.WriteLine(@"			<ss:Interior ss:Color=""Blue"" ss:Pattern=""Solid""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""negative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""1decimal"">");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.0""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""1decimalnegative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.0""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""2decimal"">");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.00""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""2decimalnegative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.00""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""3decimal"">");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""3decimalnegative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""4decimal"">");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.0000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""4decimalnegative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.0000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""5decimal"">");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.00000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				_tw.WriteLine(@"		<ss:Style ss:ID=""5decimalnegative"">");
				_tw.WriteLine(@"			<ss:Font ss:Color=""Red""/>");
				_tw.WriteLine(@"			<ss:NumberFormat  ss:Format=""#,##0.00000""/>");
				_tw.WriteLine(@"		</ss:Style>");
				//_tw.WriteLine(@"	</ss:Styles>");

			}
			catch
			{
				throw;
			}
		}

		override public void SetNumberRowsColumns(int aNumberOfRows, int aNumberOfColumns)
		{
			// nothing to do for this file type
		}

		override public void AddStyle(string aStyleName, CellStyle aCellStyle)
		{
			try
			{
				bool centerValue = false;
				switch (aCellStyle.TextAlign)
				{
					case TextAlignEnum.CenterBottom:
					case TextAlignEnum.CenterCenter:
					case TextAlignEnum.CenterTop:
						centerValue = true;
						break;
				}

				string rgb = aCellStyle.ForeColor.ToArgb().ToString("X").Substring(2,6);
				_tw.WriteLine(@"		<ss:Style ss:ID=""" + aStyleName + @""">");
				if (centerValue)
				{
					_tw.WriteLine(@"		    <ss:Alignment ss:Horizontal=""Center""/>");
				}
				_tw.WriteLine(@"			<ss:Font ss:Color=""#" + rgb + @"""/>");
				rgb = aCellStyle.BackColor.ToArgb().ToString("X").Substring(2, 6);
				_tw.WriteLine(@"			<ss:Interior ss:Color=""#" + rgb + @""" ss:Pattern=""Solid""/>");
				_tw.WriteLine(@"		</ss:Style>");
			}
			catch
			{
				throw;
			}
		}

		override public void NoMoreStyles()
		{
			try
			{
				_tw.WriteLine(@"	</ss:Styles>");
			}
			catch
			{
			}
		}

		override public void AddGrouping(string aGroupingName, int aNumberOfColumns)
		{
			// ==============================
			// Write the Grouping element
			// ==============================
            // Begin Track #5486 - JSmith - Export fails
            //_tw.WriteLine(@"    <ss:Worksheet ss:Name=""" + aGroupingName + @""">");
            _tw.WriteLine(@"    <ss:Worksheet ss:Name=""" + Include.FormatExcelSheetName(aGroupingName) + @""">");
            // End Track #5486

			// ==============================
			// Write the table element
			// ==============================
			_tw.WriteLine(@"        <ss:Table>");

			// ==============================
			// Write the column sizes
			// ==============================
			_tw.WriteLine(@"            <ss:Column ss:AutoFitWidth=""0"" ss:Width=""125""/>");
			for (int i = 0; i < aNumberOfColumns; i++)
			{
				_tw.WriteLine(@"            <ss:Column ss:AutoFitWidth=""0"" ss:Width=""50""/>");
			}
		}

		override public void WriteGrouping()
		{
			// ==============================
			// Close the Table element
			// ==============================
			_tw.WriteLine(@"        </ss:Table>");

			// ==============================
			// Close the Grouping element
			// ==============================
			_tw.WriteLine(@"    </ss:Worksheet>");
		}

		override public void AddRow()
		{
			// ==============================
			// Write the Grouping element
			// ==============================
			_tw.WriteLine(@"            <ss:Row>");
		}

		override public void WriteRow()
		{
			// ==============================
			// Close the Row element
			// ==============================
			_tw.WriteLine(@"            </ss:Row>");
		}

		override public void AddValue(string aValue, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			try
			{
				bool isNegative = false;
				bool isNumeric = false;
				int numberOfDecimals = 0;
				// add style information
				if (aExportDataType == eExportDataType.Value)
				{
					try
					{
						double number = Convert.ToDouble(aValue);
						isNumeric = true;
						if (number < 0)
						{
							isNegative = true;
						}
						int decimalPos = aValue.LastIndexOf('.');
						if (decimalPos > -1)
						{
							numberOfDecimals = aValue.Length - decimalPos - 1;
						}
					}
					catch
					{
						//not a number so replace invalid XML characters
						aValue = aValue.Replace(@"&", @"&amp;");
						aValue = aValue.Replace(@"'", @"&apos;");
						aValue = aValue.Replace(@"""", @"&quot;");
						aValue = aValue.Replace(@"<", @"&lt;");
						aValue = aValue.Replace(@">", @"&gt;");
					}
				}
				AddValue(aValue, isNumeric, isNegative, numberOfDecimals, aExportDataType, aTextStyle, aNegativeStyle);
			}
			catch
			{
				throw;
			}
		}

		override public void AddValue(string aValue, bool aIsValueNumeric, bool aIsValueNegative, int aNumberOfDecimals, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			try
			{
				// override styles if no formatting
				if (aExportDataType == eExportDataType.Value &&
					!IncludeFormatting)
				{
					aTextStyle = null;
					aNegativeStyle = null;
				}
				string type = "String";
				// add style information
				switch (aExportDataType)
				{
					case eExportDataType.Value:
						if (aIsValueNumeric)
						{
							type = "Number";
						}

						// ==============================
						// Write the Cell element
						// ==============================
						if (aIsValueNegative)
						{
							if (aNumberOfDecimals == 0)
							{
								_tw.WriteLine(@"                <ss:Cell ss:StyleID=""negative"">");
							}
							else
							{
								_tw.WriteLine(@"                <ss:Cell ss:StyleID=""" + aNumberOfDecimals.ToString() + @"decimalnegative"">");
							}
						}
						else
						{
							if (aNumberOfDecimals == 0)
							{
								_tw.WriteLine(@"                <ss:Cell>");
							}
							else
							{
								_tw.WriteLine(@"                <ss:Cell ss:StyleID=""" + aNumberOfDecimals.ToString() + @"decimal"">");
							}
						}
						break;
					case eExportDataType.RowHeading:
						if (aTextStyle == null)
						{
							_tw.WriteLine(@"                <ss:Cell ss:StyleID=""rowheading"">");
						}
						else
						{
							_tw.WriteLine(@"                <ss:Cell ss:StyleID=""" + aTextStyle + @""">");
						}
						break;
					case eExportDataType.ColumnHeading:
						if (aTextStyle == null)
						{
							_tw.WriteLine(@"                <ss:Cell ss:StyleID=""colheading"">");
						}
						else
						{
							_tw.WriteLine(@"                <ss:Cell ss:StyleID=""" + aTextStyle + @""">");
						}
						break;
					default:
						if (aTextStyle == null)
						{
							_tw.WriteLine(@"                <ss:Cell>");
						}
						else
						{
							_tw.WriteLine(@"                <ss:Cell ss:StyleID=""" + aTextStyle + @""">");
						}
						break;
				}

				// ==============================
				// Write the Data element
				// ==============================

				_tw.WriteLine(@"                    <ss:Data ss:Type=""" + type + @""">" + aValue + "</ss:Data>");

				// ==============================
				// Close the Cell element
				// ==============================
				_tw.WriteLine(@"                </ss:Cell>");
			}
			catch
			{
				throw;
			}
		}

		override public void WriteFile()
		{
			try
			{
				if (_tw != null)
				{
					// ======================
					// Close the root Workbook element
					// ======================
					_tw.WriteLine(@"</ss:Workbook>");

					// ===================================
					// Flush the buffer and close the file
					// ===================================
					_tw.Flush();

					_tw.Close();
				}
			}
			catch
			{
				throw;
			}
		}

		override public void ApplyCellFormatting(int aTopRow, int aLeftCol,
			int aBottomRow, int aRightCol, string aStyle)
		{
			try
			{
				// nothing to do for this file type
			}
			catch
			{
				throw;
			}
		}
	}

	public class MIDExportFlexGridToCSV : MIDExportFile
	{
		//=======
		// FIELDS
		//=======
		private TextWriter _tw = null;
		private string _line = null;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instace of MIDExportFlexGridToXML class to export data in XML format.
		/// </summary>
		/// <param name="aFileName">
		/// The name of the extract file.
		/// </param>
		/// <param name="aIncludeFormatting">
		/// A flag identifying if the file is to include the current formatting.
		/// This is always false for this type of file.
		/// </param>

		public MIDExportFlexGridToCSV(string aFileName, bool aIncludeFormatting)
			: base(aFileName, false, eExportType.CSV)
		{
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		override public void OpenFile()
		{
			try
			{
				// =================
				// Open the document
				// =================
				_tw = new StreamWriter(FileName);
			}
			catch
			{
				throw;
			}
		}

		override public void SetNumberRowsColumns(int aNumberOfRows, int aNumberOfColumns)
		{
			// nothing to do for this file type
		}
		override public void AddStyle(string aStyleName, CellStyle aCellStyle)
		{
			// nothing to do for this file type
		}

		override public void NoMoreStyles()
		{
			// nothing to do for this file type
		}

		override public void AddGrouping(string aGroupingName, int aNumberOfColumns)
		{
			// nothing to do for this file type
		}

		override public void WriteGrouping()
		{
			// add blank line after grouping
			_tw.WriteLine(" ");
		}

		override public void AddRow()
		{
			// nothing to do for this file type
		}

		override public void WriteRow()
		{
			_tw.WriteLine(_line);
			_line = null;
		}

		override public void AddValue(string aValue, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			try
			{
				// format values do not matter for this format so default
				AddValue(aValue, false, false, 0, aExportDataType, aTextStyle, aNegativeStyle);
			}
			catch
			{
				throw;
			}
		}

		override public void AddValue(string aValue, bool aIsValueNumeric, bool aIsValueNegative, int aNumberOfDecimals, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			if (_line != null)
			{
				_line += ",";
			}
			// remove control characters
			aValue = aValue.Replace("\r\n", " ");
			// make sure there are not commas in the value
			_line += aValue.Replace(",",null);
		}

		override public void WriteFile()
		{
			try
			{
				if (_tw != null)
				{
					// ===================================
					// Flush the buffer and close the file
					// ===================================
					_tw.Flush();

					_tw.Close();
				}
			}
			catch
			{
				throw;
			}
		}

		override public void ApplyCellFormatting(int aTopRow, int aLeftCol,
			int aBottomRow, int aRightCol, string aStyle)
		{
			try
			{
				// nothing to do for this file type
			}
			catch
			{
				throw;
			}
		}
	}

	public class MIDExportFlexGridToExcel : MIDExportFile
	{
		//=======
		// FIELDS
		//=======
		private C1FlexGrid _exportGrid = null;
		private string _currentGrouping = null;
		private int _row = 0;
		private int _col = 0;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instace of MIDExportFlexGridToXML class to export data in XML format.
		/// </summary>
		/// <param name="aFileName">
		/// The name of the extract file.
		/// </param>
		/// <param name="aIncludeFormatting">
		/// A flag identifying if the file is to include the current formatting.
		/// This is always false for this type of file.
		/// </param>

		public MIDExportFlexGridToExcel(string aFileName, bool aIncludeFormatting)
			: base(aFileName, aIncludeFormatting, eExportType.Excel)
		{
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		override public void OpenFile()
		{
			try
			{
				// nothing to do for this file type
			}
			catch
			{
				throw;
			}
		}

		override public void SetNumberRowsColumns(int aNumberOfRows, int aNumberOfColumns)
		{
			try
			{
				_exportGrid.Rows.Count = aNumberOfRows;
				_exportGrid.Cols.Count = aNumberOfColumns;
			}
			catch
			{
				throw;
			}
		}

		override public void AddStyle(string aStyleName, CellStyle aCellStyle)
		{
			try
			{
				_exportGrid.Styles.Add(aStyleName, aCellStyle);
			}
			catch
			{
				throw;
			}
		}

		override public void NoMoreStyles()
		{
			// nothing to do for this file type
		}

		override public void AddGrouping(string aGroupingName, int aNumberOfColumns)
		{
			try
			{
				_exportGrid = new C1FlexGrid();
				_exportGrid.Cols.Fixed = 0;
				_exportGrid.Rows.Fixed = 0;
				_currentGrouping = aGroupingName;
				_row = 0;
				_col = 0;
			}
			catch
			{
				throw;
			}
		}

		override public void WriteGrouping()
		{
			//Begin Track #5666 - JScott - Export to Excel when Formatting checked receive and error.
			//_exportGrid.SaveExcel(FileName, _currentGrouping, FileFlags.AsDisplayed);
			_exportGrid.SaveExcel(FileName, Include.FormatExcelSheetName(_currentGrouping), FileFlags.AsDisplayed);
			//End Track #5666 - JScott - Export to Excel when Formatting checked receive and error.
		}

		override public void AddRow()
		{
			// nothing to do for this file type
		}

		override public void WriteRow()
		{
			++_row;
			_col = 0;
		}

		override public void AddValue(string aValue, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			try
			{
				bool isNegative = false;
				bool isNumeric = false;
				int numberOfDecimals = 0;
				// add style information
				if (aExportDataType == eExportDataType.Value)
				{
					try
					{
						double number = Convert.ToDouble(aValue);
						isNumeric = true;
						if (number < 0)
						{
							isNegative = true;
						}
						int decimalPos = aValue.LastIndexOf('.');
						if (decimalPos > -1)
						{
							numberOfDecimals = aValue.Length - decimalPos - 1;
						}
					}
					catch
					{
						//not a number
					}
				}
				AddValue(aValue, isNumeric, isNegative, numberOfDecimals, aExportDataType, aTextStyle, aNegativeStyle);
			}
			catch
			{
				throw;
			}
		}

		override public void AddValue(string aValue, bool aIsValueNumeric, bool aIsValueNegative, int aNumberOfDecimals, eExportDataType aExportDataType, string aTextStyle, string aNegativeStyle)
		{
			try
			{
				_exportGrid[_row, _col] = aValue;
				if (aExportDataType == eExportDataType.Value)
				{
					if (aIsValueNegative)
					{
						ApplyCellFormatting(_row, _col, _row, _col, aNegativeStyle);
					}
					else if (aTextStyle != null)
					{
						ApplyCellFormatting(_row, _col, _row, _col, aTextStyle);
					}

				}
				else if (aTextStyle != null)
				{
					ApplyCellFormatting(_row, _col, _row, _col, aTextStyle);
				}

				++_col;
			}
			catch
			{
				throw;
			}
		}

		override public void WriteFile()
		{
			try
			{
				// nothing to do for this file type
			}
			catch
			{
				throw;
			}
		}

		override public void ApplyCellFormatting(int aTopRow, int aLeftCol,
			int aBottomRow, int aRightCol, string aStyle)
		{
			try
			{
				CellRange cellRange;
				cellRange = _exportGrid.GetCellRange(aTopRow, aLeftCol, aBottomRow, aRightCol);
				cellRange.Style = _exportGrid.Styles[aStyle];
			}
			catch
			{
				throw;
			}
		}
	}
}
