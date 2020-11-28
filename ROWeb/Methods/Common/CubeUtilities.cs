using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class CubeUtilities
    {
        public static readonly string[] CubePeriodTableNames = { "Year", "Season", "Quarter", "Month", "Week" };
        public static readonly string TotalTableName = "Total";
        public static readonly string DetailsTableName = "Details";
        public static readonly string INTERNAL_ROW_ID_COLUMN_NAME = "__internal_roweb_row_id__";
        public static readonly string sRowIDColumnName = "RowKey";
        public static readonly string sParentRowID = "ParentRowKey";
        public static readonly string sTimePeriodColName = "RowKeyDisplay";

        
        /// <summary>
        /// Get the details about the cube data available
        /// </summary>
        /// <returns>the data about the cube data available</returns>
        //RO-1074 Removed Logic for Get Metatdata - it will be called in the planning classes instead
        // public static ROCubeMetadata GetROCubeMetadata()
        //{
            //DataSet dsTransposed = TransposeDataSetTables(dsCubeData, firstVisibleColumn);
            //Dictionary<string, ROCubeTableMetaData> cubeValues = new Dictionary<string, ROCubeTableMetaData>();

            //foreach (DataTable dt in dsCubeData.Tables)
            //{
            //    bool bSetGroupNames;
            //    List <ROCubeColumnAttributes> columnAttributes = BuildColumnAttributeList(dt, out bSetGroupNames);
            //    string transposedTableName = GetTransposedTableName(dt.TableName);
            //    DataTable dtTransposed = dsTransposed.Tables[transposedTableName];
            //    HashSet<string> columnNames = new HashSet<string>();
            //    HashSet<string> transposedColumnNames = new HashSet<string>();

            //    for (int columnIndex = firstVisibleColumn; columnIndex < columnAttributes.Count; ++columnIndex)
            //    {
            //        if (bSetGroupNames)
            //        {
            //            columnNames.Add(columnAttributes[columnIndex].GroupName);
            //        }
            //        else
            //        {
            //            columnNames.Add(columnAttributes[columnIndex].Name);
            //        }
            //    }

            //    for (int columnIndex = firstTransposedVisibleColumn; columnIndex < dtTransposed.Columns.Count; ++columnIndex)
            //    {
            //        transposedColumnNames.Add(dtTransposed.Columns[columnIndex].ColumnName);
            //    }

            //    ROCubeTableMetaData tableMetadata = new ROCubeTableMetaData(dt, dtTransposed, columnNames, transposedColumnNames, bSetGroupNames);

            //    cubeValues.Add(dt.TableName, tableMetadata);
            //}

            //ROCubeTableMetaData firstMetadata = cubeValues.First<KeyValuePair<string, ROCubeTableMetaData>>().Value;
            //int variableCount = firstMetadata.columnNames.Count<string>();
            //int visibleCoumns = dsCubeData.Tables[firstMetadata.sTableName].Columns.Count - firstVisibleColumn;
            //int columnsPerVariable = visibleCoumns / variableCount;

            //return new ROCubeMetadata(eROReturnCode.Successful, null, metadataParams.ROInstanceID, 0, 0, null);
        //}

        private static void GetChildren(List<DataRow> selectedRows, int tableIndex, List<DataTable> periodTables)
        {
            if (tableIndex >= periodTables.Count)
            {
                return;
            }

            DataRow drParentRow = selectedRows[selectedRows.Count - 1];
            string parentRowKey = drParentRow[sRowIDColumnName].ToString();
            string sWhereClause = string.Format("{0} = '{1}'", sParentRowID, parentRowKey);
            DataRow[] childRows = periodTables[tableIndex].Select(sWhereClause);

            foreach(DataRow childRow in childRows)
            {
                selectedRows.Add(childRow);
                GetChildren(selectedRows, tableIndex + 1, periodTables);
            }
        }

        

        private static string ReplaceLineBreak(string sInput)
        {
            return sInput.Replace("\r\n", "__");
        }

    }
}
