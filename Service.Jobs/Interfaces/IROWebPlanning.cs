using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

using Logility.ROWebSharedTypes; // TT#1156-MD CTeegarden - make cube functionality generic

namespace Logility.ROServices
{
    /// <summary>
    /// Provide Merchandise Planning interfaces to the RO application
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebPlanning
    {
        /// <summary>
        /// Get Filter Controls Data
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="screenID">Screen containing filter controls</param>
        /// <returns>a DataSet containing tables to be bound to that screen's controls</returns>
        [OperationContract]
        DataSet GetFilterControlsData(string sUserID, string sEnvironment, eScreenID screenID);

        /// <summary>
        /// Get Hierarchy Child Nodes
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="screenID">Screen containing filter controls</param>
        /// <param name="sNodeAddress">The address of the parent node</param>
        /// <returns>a DataTable containing a row each for each child node</returns>
        [OperationContract]
        DataTable GetHierarchyChildNodes(string sUserID, string sEnvironment, eScreenID screenID, string sNodeAddress);

		// BEGIN TT#1156-MD CTeegarden - make cube functionality generic
        /// <summary>
        /// Open Chain Function
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="openParams">The details needed to open the correct data</param>
        /// <returns>Instance ID of the function</returns>
        [OperationContract]
        long OpenCubeFunction(string sUserID, string sEnvironment, CubeOpenParams openParams);
		//long OpenChainFunction(string sUserID, string sEnvironment, string sView, string sNodeID, string sVersion, string sFromWeekYYYYWW, string sToWeekYYYYWW);

        /// <summary>
        ///  Close Cube Function
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        [OperationContract]
        void CloseCubeFunction(string sUserID, string sEnvironment, long lInstanceID);

        /// <summary>
        /// Get the details about the cube data available
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        /// <param name="metadataParams">the information needed to determine table dimensions, etc.</param>
        /// <returns>the data about the cube data available</returns>
        [OperationContract]
        CubeMetadata GetCubeMetadata(string sUserID, string sEnvironment, long lInstanceID, CubeGetMetadataParams metadataParams);

        /// <summary>
        /// Get Cube Data
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        /// <param name="getDataParams">Specifies the data to get from the cube group and the desired orientation</param>
        /// <returns>Dataset with chain values</returns>
        [OperationContract]
        DataSet GetCubeData(string sUserID, string sEnvironment, long lInstanceID, CubeGetDataParams getDataParams);
		//DataSet GetChainData(string sUserID, string sEnvironment, long lInstanceID);
		
        /// <summary>
        /// Request period values for Chain Ladder
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        /// <param name="periodChangeParams">The data to use in determining what tables to return</param>
        /// <returns>updated info about the cube data available</returns>
        [OperationContract]
        CubeMetadata HandlePeriodChange(string sUserID, string sEnvironment, long lInstanceID, CubePeriodChangeParams periodChangeParams);
        //DataSet HandlePeriodChange(string sUserID, string sEnvironment, long lInstanceID, bool showYears, bool showSeasons, bool showQuarters, bool showMonths, bool showWeeks);

        /// <summary>
        /// Send changed values to the cubes
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        /// <param name="cellChanges">The list of changed cell values</param>
        [OperationContract]
        void CellValuesChanged(string sUserID, string sEnvironment, long lInstanceID, List<GridCellChange> cellChanges);
        //void CellValueChanged(string sUserID, string sEnvironment, long lInstanceID, int iRowIndex, int iColumnIndex, double dNewValue);
		
        /// <summary>
        /// Recompute the cubes after applying changes
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        /// <param name="getDataParams">Specifies the data to get from the cube group and the desired orientation</param>
        /// <returns>Dataset with new values</returns>
        [OperationContract]
        DataSet RecomputeCubes(string sUserID, string sEnvironment, long lInstanceID, CubeGetDataParams getDataParams);
        //DataSet RecomputeCubes(string sUserID, string sEnvironment, long lInstanceID);
		
        /// <summary>
        /// Undo the most recent recompute of the cube group
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        [OperationContract]
        void UndoLastRecompute(string sUserID, string sEnvironment, long lInstanceID);
		// END TT#1156-MD CTeegarden - make cube functionality generic

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="lInstanceID">The ID of the function</param>
        [OperationContract]
        void SaveCubeGroup(string sUserID, string sEnvironment, long lInstanceID);
    }
}
