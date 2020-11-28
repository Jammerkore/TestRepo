//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Logility.ROWebSharedTypes;

//namespace Logility.ROWeb
//{
//    class DataTypeCoordinatesMap
//    {
//        private eDataType _dataType;
//        private Dictionary<Object, int> rowCoordinatesMap;
//        private Dictionary<Object, int> columnCoordinatesMap;

//        public DataTypeCoordinatesMap(eDataType dataType, ROCells ROCells)
//        {
//            //int colCount = dt.Columns.Count;
//            //int rowCount = dt.Rows.Count;

//            //ROCells.ColumnNames

//            //_sTableName = dt.TableName;
//            //columnCoordinatesMap = new Dictionary<string, int>();
//            //for (int coldex = iAddedColumnCount; coldex < colCount; ++coldex)
//            //{
//            //    string sColumnName = dt.Columns[coldex].ColumnName;

//            //    columnCoordinatesMap.Add(sColumnName, coldex);
//            //}

//            _dataType = dataType;
//            int coldex = 0;
//            foreach (string columnName in ROCells.ColumnNames)
//            {
//                columnCoordinatesMap.Add(columnName, coldex);
//                ++coldex;
//            }

//            //rowCoordinatesMap = new Dictionary<Object, int>();
//            //for (int iRowIndex = 0; iRowIndex < rowCount; ++iRowIndex)
//            //{
//            //    int iRowID = Convert.ToInt32(dt.Rows[iRowIndex][CubeUtilities.INTERNAL_ROW_ID_COLUMN_NAME]);
//            //    int iCubeRowID = Convert.ToInt32(dt.Rows[iRowIndex]["RowIndex"]);

//            //    rowCoordinatesMap.Add(iRowID, iCubeRowID);
//            //}
//        }

//        //public int GetRowCoordinate(Object rowID)
//        //{
//        //    int iRowCoordinate;

//        //    if (!rowCoordinatesMap.TryGetValue(rowID, out iRowCoordinate))
//        //    {
//        //        string msg = string.Format("Could not find coordinate for row id {0} in table {1}", rowID.ToString(), _dataType.ToString());
//        //        throw new Exception(msg);
//        //    }

//        //    return iRowCoordinate;
//        //}

//        //public void AddRow(Object rowID)
//        //{
//        //    int iRowIndex = rowCoordinatesMap.Count;

//        //    rowCoordinatesMap.Add(rowID, iRowIndex);
//        //}

//        public int GetColumnCoordinate(string sColumnName)
//        {
//            int iColumnCoordinate;

//            if (!columnCoordinatesMap.TryGetValue(sColumnName, out iColumnCoordinate))
//            {
//                string msg = string.Format("Could not find coordinate for column {0} in table {1}", sColumnName, _dataType.ToString());

//                throw new Exception(msg);
//            }

//            return iColumnCoordinate;
//        }
//    }

//    public class CubeCoordinatesMap
//    {
//        private Dictionary<eDataType, DataTypeCoordinatesMap> _cubeCoordinatesMap;

//        public CubeCoordinatesMap(ROData ROData)
//        {
//            _cubeCoordinatesMap = new Dictionary<eDataType, DataTypeCoordinatesMap>();

//            eDataType dataType;
//            ROCells ROCells;
//            foreach (KeyValuePair<eDataType, ROCells> keyPair in ROData.Cells)
//            {
//                dataType = keyPair.Key;
//                ROCells = keyPair.Value;

//                DataTypeCoordinatesMap dataTypeMap = new DataTypeCoordinatesMap(dataType, ROCells);

//                _cubeCoordinatesMap.Add(dataType, dataTypeMap);
//            }
//        }

//        //public int GetRowCoordinate(object rowID)
//        //{
//        //    int rowCoordinate = -1;

//        //    foreach(DataTypeCoordinatesMap tableMap in _cubeCoordinatesMap.Values)
//        //    {
//        //        try
//        //        {
//        //            rowCoordinate = tableMap.GetRowCoordinate(rowID);
//        //            break;
//        //        }
//        //        catch {}
//        //    }

//        //    return rowCoordinate;
//        //}

//        public int GetColumnCoordinate(string sColumnName)
//        {
//            DataTypeCoordinatesMap dataTypeCoordinateMap = _cubeCoordinatesMap.Values.First();
//            int columnCoordinate = -1;

//            try
//            {
//                columnCoordinate = dataTypeCoordinateMap.GetColumnCoordinate(sColumnName);
//            }
//            catch { }

//            return columnCoordinate;
//        }

//        //public int GetRowCoordinate(eDataType dataType, Object rowID)
//        //{
//        //    DataTypeCoordinatesMap dataTypeCoordinateMap;

//        //    if (!_cubeCoordinatesMap.TryGetValue(dataType, out dataTypeCoordinateMap))
//        //    {
//        //        throw new Exception("Could not find coordinates for table " + dataType.ToString());
//        //    }

//        //    return dataTypeCoordinateMap.GetRowCoordinate(rowID);
//        //}

//        public int GetColumnCoordinate(eDataType dataType, string sColumnName)
//        {
//            DataTypeCoordinatesMap dataTypeCoordinateMap;

//            if (!_cubeCoordinatesMap.TryGetValue(dataType, out dataTypeCoordinateMap))
//            {
//                throw new Exception("Could not find coordinates for table " + dataType.ToString());
//            }

//            return dataTypeCoordinateMap.GetColumnCoordinate(sColumnName);
//        }
//    }
//}
