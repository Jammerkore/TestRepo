using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ReportData : DataLayer
    {
        protected static class StoredProcedures
        {

			public static MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME_def MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME = new MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME_def();
			public class MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME.SQL"

			
			    public MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME_def()
			    {
			        base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME";
			        base.procedureType = storedProcedureTypes.ScalarValue;
			        base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
			    }
			
			    public object ReadValues(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME_def))
                    {
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
			    }
			}

			public static MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME_def MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME = new MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME_def();
			public class MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME.SQL"

			
			    public MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME_def()
			    {
			        base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME";
			        base.procedureType = storedProcedureTypes.ScalarValue;
			        base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
			    }
			
			    public object ReadValues(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME_def))
                    {
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
			    }
			}



            public static MID_GET_FORECAST_METHODS_USED_def MID_GET_FORECAST_METHODS_USED = new MID_GET_FORECAST_METHODS_USED_def();
            public class MID_GET_FORECAST_METHODS_USED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_FORECAST_METHODS_USED.SQL"

			    private intParameter USER_RID;
                private intParameter INCLUDE_DESCRIPTION;
			
			    public MID_GET_FORECAST_METHODS_USED_def()
			    {
			        base.procedureName = "MID_GET_FORECAST_METHODS_USED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        INCLUDE_DESCRIPTION = new intParameter("@INCLUDE_DESCRIPTION", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? INCLUDE_DESCRIPTION
			                          )
			    {
                    lock (typeof(MID_GET_FORECAST_METHODS_USED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.INCLUDE_DESCRIPTION.SetValue(INCLUDE_DESCRIPTION);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_GET_FORECAST_METHOD_TYPES_USED_def MID_GET_FORECAST_METHOD_TYPES_USED = new MID_GET_FORECAST_METHOD_TYPES_USED_def();
			public class MID_GET_FORECAST_METHOD_TYPES_USED_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_FORECAST_METHOD_TYPES_USED.SQL"

                private intParameter USER_RID;

                public MID_GET_FORECAST_METHOD_TYPES_USED_def()
			    {
			        base.procedureName = "MID_GET_FORECAST_METHOD_TYPES_USED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_GET_FORECAST_METHOD_TYPES_USED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


            public static MID_GET_ALLOCATION_METHODS_USED_def MID_GET_ALLOCATION_METHODS_USED = new MID_GET_ALLOCATION_METHODS_USED_def();
            public class MID_GET_ALLOCATION_METHODS_USED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ALLOCATION_METHODS_USED.SQL"

                private intParameter USER_RID;
                private intParameter INCLUDE_DESCRIPTION;
			
			    public MID_GET_ALLOCATION_METHODS_USED_def()
			    {
			        base.procedureName = "MID_GET_ALLOCATION_METHODS_USED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        INCLUDE_DESCRIPTION = new intParameter("@INCLUDE_DESCRIPTION", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? INCLUDE_DESCRIPTION
			                          )
			    {
                    lock (typeof(MID_GET_ALLOCATION_METHODS_USED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.INCLUDE_DESCRIPTION.SetValue(INCLUDE_DESCRIPTION);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_GET_ALLOCATION_METHOD_TYPES_USED_def MID_GET_ALLOCATION_METHOD_TYPES_USED = new MID_GET_ALLOCATION_METHOD_TYPES_USED_def();
            public class MID_GET_ALLOCATION_METHOD_TYPES_USED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ALLOCATION_METHOD_TYPES_USED.SQL"

                private intParameter USER_RID;
			
			    public MID_GET_ALLOCATION_METHOD_TYPES_USED_def()
			    {
			        base.procedureName = "MID_GET_ALLOCATION_METHOD_TYPES_USED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_GET_ALLOCATION_METHOD_TYPES_USED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GET_FORECAST_ANALYSIS_def MID_GET_FORECAST_ANALYSIS = new MID_GET_FORECAST_ANALYSIS_def();
            public class MID_GET_FORECAST_ANALYSIS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_FORECAST_ANALYSIS.SQL"

                private intParameter RestrictToDescendantsOf_HN_RID;
                private intParameter RestrictToLowerLevelSequence;
                private intParameter UseDateRange;
                private datetimeParameter StartDate;
                private datetimeParameter EndDate;
                private intParameter UseForecastDateRange;
                private datetimeParameter ForecastStartDate;
                private datetimeParameter ForecastEndDate;
                private intParameter ResultLimit;
                private intParameter RestrictMethods;
                private stringParameter MethodRIDsToInclude;
                private intParameter RestrictUsers;
                private stringParameter UserRIDsToInclude;
                private intParameter RestrictStoreForecastVersions;
                private stringParameter StoreForecastVersionRIDsToInclude;
                private intParameter RestrictChainForecastVersions;
                private stringParameter ChainForecastVersionRIDsToInclude;
			
			    public MID_GET_FORECAST_ANALYSIS_def()
			    {
			        base.procedureName = "MID_GET_FORECAST_ANALYSIS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("AUDIT_FORECAST");
                    RestrictToDescendantsOf_HN_RID = new intParameter("@RestrictToDescendantsOf_HN_RID", base.inputParameterList);
                    RestrictToLowerLevelSequence = new intParameter("@RestrictToLowerLevelSequence", base.inputParameterList);
                    UseDateRange = new intParameter("@UseDateRange", base.inputParameterList);
                    StartDate = new datetimeParameter("@StartDate", base.inputParameterList);
                    EndDate = new datetimeParameter("@EndDate", base.inputParameterList);
                    UseForecastDateRange = new intParameter("@UseForecastDateRange", base.inputParameterList);
                    ForecastStartDate = new datetimeParameter("@ForecastStartDate", base.inputParameterList);
                    ForecastEndDate = new datetimeParameter("@ForecastEndDate", base.inputParameterList);
                    ResultLimit = new intParameter("@ResultLimit", base.inputParameterList);
                    RestrictMethods = new intParameter("@RestrictMethods", base.inputParameterList);
                    MethodRIDsToInclude = new stringParameter("@MethodRIDsToInclude", base.inputParameterList);
                    RestrictUsers = new intParameter("@RestrictUsers", base.inputParameterList);
                    UserRIDsToInclude = new stringParameter("@UserRIDsToInclude", base.inputParameterList);
                    RestrictStoreForecastVersions = new intParameter("@RestrictStoreForecastVersions", base.inputParameterList);
                    StoreForecastVersionRIDsToInclude = new stringParameter("@StoreForecastVersionRIDsToInclude", base.inputParameterList);
                    RestrictChainForecastVersions = new intParameter("@RestrictChainForecastVersions", base.inputParameterList);
                    ChainForecastVersionRIDsToInclude = new stringParameter("@ChainForecastVersionRIDsToInclude", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba,
                                      int? RestrictToDescendantsOf_HN_RID,
                                      int? RestrictToLowerLevelSequence,
                                      int? UseDateRange,
                                      DateTime? StartDate,
                                      DateTime? EndDate,
                                      int? UseForecastDateRange,
                                      DateTime? ForecastStartDate,
                                      DateTime? ForecastEndDate,
                                      int? ResultLimit,
                                      int? RestrictMethods,
                                      string MethodRIDsToInclude,
                                      int? RestrictUsers,
                                      string UserRIDsToInclude,
                                      int? RestrictStoreForecastVersions,
                                      string StoreForecastVersionRIDsToInclude,
                                      int? RestrictChainForecastVersions,
                                      string ChainForecastVersionRIDsToInclude
			                          )
			    {
                    lock (typeof(MID_GET_FORECAST_ANALYSIS_def))
                    {
                        this.RestrictToDescendantsOf_HN_RID.SetValue(RestrictToDescendantsOf_HN_RID);
                        this.RestrictToLowerLevelSequence.SetValue(RestrictToLowerLevelSequence);
                        this.UseDateRange.SetValue(UseDateRange);
                        this.StartDate.SetValue(StartDate);
                        this.EndDate.SetValue(EndDate);
                        this.UseForecastDateRange.SetValue(UseForecastDateRange);
                        this.ForecastStartDate.SetValue(ForecastStartDate);
                        this.ForecastEndDate.SetValue(ForecastEndDate);
                        this.ResultLimit.SetValue(ResultLimit);
                        this.RestrictMethods.SetValue(RestrictMethods);
                        this.MethodRIDsToInclude.SetValue(MethodRIDsToInclude);
                        this.RestrictUsers.SetValue(RestrictUsers);
                        this.UserRIDsToInclude.SetValue(UserRIDsToInclude);
                        this.RestrictStoreForecastVersions.SetValue(RestrictStoreForecastVersions);
                        this.StoreForecastVersionRIDsToInclude.SetValue(StoreForecastVersionRIDsToInclude);
                        this.RestrictChainForecastVersions.SetValue(RestrictChainForecastVersions);
                        this.ChainForecastVersionRIDsToInclude.SetValue(ChainForecastVersionRIDsToInclude);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}




            public static MID_GET_ALLOCATION_ANALYSIS_def MID_GET_ALLOCATION_ANALYSIS = new MID_GET_ALLOCATION_ANALYSIS_def();
            public class MID_GET_ALLOCATION_ANALYSIS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ALLOCATION_ANALYSIS.SQL"

                private intParameter RestrictToStyle_HN_RID;
                private intParameter UseDateRange;
                private datetimeParameter StartDate;
                private datetimeParameter EndDate;
                private intParameter ResultLimit;
                private intParameter RestrictMethods;
                private stringParameter MethodRIDsToInclude;
                private intParameter RestrictUsers;
                private stringParameter UserRIDsToInclude;
                private intParameter RestrictHeaders;
                private stringParameter HeaderRIDsToInclude;
                private intParameter Include_Receipt;
                private intParameter Include_ASN;
                private intParameter Include_Dummy;
                private intParameter Include_DropShip;
                private intParameter Include_MultiHeader;
                private intParameter Include_Reserve;
                private intParameter Include_WorkupTotalBuy;
                private intParameter Include_PurchaseOrder;
                private intParameter Include_Assortment;
                private intParameter Include_Placeholder;
                private intParameter Include_IMO;
                private intParameter Include_Master;  // TT#1966-MD - JSmith - DC Fulfillment
                private intParameter Include_ReceivedOutOfBalance;
                private intParameter Include_ReceivedInBalance;
                private intParameter Include_InUseByMultiHeader;
                private intParameter Include_PartialSizeOutOfBalance;
                private intParameter Include_PartialSizeInBalance;
                private intParameter Include_AllocatedOutOfBalance;
                private intParameter Include_AllocatedInBalance;
                private intParameter Include_SizesOutOfBalance;
                private intParameter Include_AllInBalance;
                private intParameter Include_Released;
                private intParameter Include_ReleaseApproved;
                private intParameter Include_AllocationStarted;
                private intParameter IncludeActionType_ActionUnassigned;
                private intParameter IncludeActionType_ApplyAPI_Workflow;
                private intParameter IncludeActionType_BackoutAllocation;
                private intParameter IncludeActionType_BackoutDetailPackAllocation;
                private intParameter IncludeActionType_BackoutSizeAllocation;
                private intParameter IncludeActionType_BackoutSizeIntransit;
                private intParameter IncludeActionType_BackoutStyleIntransit;
                private intParameter IncludeActionType_BalanceSizeBilaterally;
                private intParameter IncludeActionType_BalanceSizeNoSubs;
                private intParameter IncludeActionType_BalanceSizeWithConstraints;
                private intParameter IncludeActionType_BalanceSizeWithSubs;
                private intParameter IncludeActionType_BalanceStyleProportional;
                private intParameter IncludeActionType_BalanceToDC;
                private intParameter IncludeActionType_BreakoutSizesAsReceived;
                private intParameter IncludeActionType_BreakoutSizesAsReceivedWithConstraints;
                private intParameter IncludeActionType_ChargeIntransit;
                private intParameter IncludeActionType_ChargeSizeIntransit;
                private intParameter IncludeActionType_DeleteHeader;
                private intParameter IncludeActionType_ReapplyTotalAllocation;
                private intParameter IncludeActionType_Release;
                private intParameter IncludeActionType_RemoveAPI_Workflow;
                private intParameter IncludeActionType_Reset;
                private intParameter IncludeActionType_StyleNeed;
                private intParameter IncludeActionType_BalanceToVSW;     // TT#1334-MD - stodd - Balance to VSW Action

                public MID_GET_ALLOCATION_ANALYSIS_def()
                {
                    base.procedureName = "MID_GET_ALLOCATION_ANALYSIS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("AUDIT_HEADER");
                    RestrictToStyle_HN_RID = new intParameter("@RestrictToStyle_HN_RID", base.inputParameterList);
                    UseDateRange = new intParameter("@UseDateRange", base.inputParameterList);
                    StartDate = new datetimeParameter("@StartDate", base.inputParameterList);
                    EndDate = new datetimeParameter("@EndDate", base.inputParameterList);
                    ResultLimit = new intParameter("@ResultLimit", base.inputParameterList);
                    RestrictMethods = new intParameter("@RestrictMethods", base.inputParameterList);
                    MethodRIDsToInclude = new stringParameter("@MethodRIDsToInclude", base.inputParameterList);
                    RestrictUsers = new intParameter("@RestrictUsers", base.inputParameterList);
                    UserRIDsToInclude = new stringParameter("@UserRIDsToInclude", base.inputParameterList);
                    RestrictHeaders = new intParameter("@RestrictHeaders", base.inputParameterList);
                    HeaderRIDsToInclude = new stringParameter("@HeaderRIDsToInclude", base.inputParameterList);
                    Include_Receipt = new intParameter("@Include_Receipt", base.inputParameterList);
                    Include_ASN = new intParameter("@Include_ASN", base.inputParameterList);
                    Include_Dummy = new intParameter("@Include_Dummy", base.inputParameterList);
                    Include_DropShip = new intParameter("@Include_DropShip", base.inputParameterList);
                    Include_MultiHeader = new intParameter("@Include_MultiHeader", base.inputParameterList);
                    Include_Reserve = new intParameter("@Include_Reserve", base.inputParameterList);
                    Include_WorkupTotalBuy = new intParameter("@Include_WorkupTotalBuy", base.inputParameterList);
                    Include_PurchaseOrder = new intParameter("@Include_PurchaseOrder", base.inputParameterList);
                    Include_Assortment = new intParameter("@Include_Assortment", base.inputParameterList);
                    Include_Placeholder = new intParameter("@Include_Placeholder", base.inputParameterList);
                    Include_IMO = new intParameter("@Include_IMO", base.inputParameterList);
                    Include_Master = new intParameter("@Include_Master", base.inputParameterList);   // TT#1966-MD - JSmith - DC Fulfillment
                    Include_ReceivedOutOfBalance = new intParameter("@Include_ReceivedOutOfBalance", base.inputParameterList);
                    Include_ReceivedInBalance = new intParameter("@Include_ReceivedInBalance", base.inputParameterList);
                    Include_InUseByMultiHeader = new intParameter("@Include_InUseByMultiHeader", base.inputParameterList);
                    Include_PartialSizeOutOfBalance = new intParameter("@Include_PartialSizeOutOfBalance", base.inputParameterList);
                    Include_PartialSizeInBalance = new intParameter("@Include_PartialSizeInBalance", base.inputParameterList);
                    Include_AllocatedOutOfBalance = new intParameter("@Include_AllocatedOutOfBalance", base.inputParameterList);
                    Include_AllocatedInBalance = new intParameter("@Include_AllocatedInBalance", base.inputParameterList);
                    Include_SizesOutOfBalance = new intParameter("@Include_SizesOutOfBalance", base.inputParameterList);
                    Include_AllInBalance = new intParameter("@Include_AllInBalance", base.inputParameterList);
                    Include_Released = new intParameter("@Include_Released", base.inputParameterList);
                    Include_ReleaseApproved = new intParameter("@Include_ReleaseApproved", base.inputParameterList);
                    Include_AllocationStarted = new intParameter("@Include_AllocationStarted", base.inputParameterList);
                    IncludeActionType_ActionUnassigned = new intParameter("@IncludeActionType_ActionUnassigned", base.inputParameterList);
                    IncludeActionType_ApplyAPI_Workflow = new intParameter("@IncludeActionType_ApplyAPI_Workflow", base.inputParameterList);
                    IncludeActionType_BackoutAllocation = new intParameter("@IncludeActionType_BackoutAllocation", base.inputParameterList);
                    IncludeActionType_BackoutDetailPackAllocation = new intParameter("@IncludeActionType_BackoutDetailPackAllocation", base.inputParameterList);
                    IncludeActionType_BackoutSizeAllocation = new intParameter("@IncludeActionType_BackoutSizeAllocation", base.inputParameterList);
                    IncludeActionType_BackoutSizeIntransit = new intParameter("@IncludeActionType_BackoutSizeIntransit", base.inputParameterList);
                    IncludeActionType_BackoutStyleIntransit = new intParameter("@IncludeActionType_BackoutStyleIntransit", base.inputParameterList);
                    IncludeActionType_BalanceSizeBilaterally = new intParameter("@IncludeActionType_BalanceSizeBilaterally", base.inputParameterList);
                    IncludeActionType_BalanceSizeNoSubs = new intParameter("@IncludeActionType_BalanceSizeNoSubs", base.inputParameterList);
                    IncludeActionType_BalanceSizeWithConstraints = new intParameter("@IncludeActionType_BalanceSizeWithConstraints", base.inputParameterList);
                    IncludeActionType_BalanceSizeWithSubs = new intParameter("@IncludeActionType_BalanceSizeWithSubs", base.inputParameterList);
                    IncludeActionType_BalanceStyleProportional = new intParameter("@IncludeActionType_BalanceStyleProportional", base.inputParameterList);
                    IncludeActionType_BalanceToDC = new intParameter("@IncludeActionType_BalanceToDC", base.inputParameterList);
                    IncludeActionType_BreakoutSizesAsReceived = new intParameter("@IncludeActionType_BreakoutSizesAsReceived", base.inputParameterList);
                    IncludeActionType_BreakoutSizesAsReceivedWithConstraints = new intParameter("@IncludeActionType_BreakoutSizesAsReceivedWithConstraints", base.inputParameterList);
                    IncludeActionType_ChargeIntransit = new intParameter("@IncludeActionType_ChargeIntransit", base.inputParameterList);
                    IncludeActionType_ChargeSizeIntransit = new intParameter("@IncludeActionType_ChargeSizeIntransit", base.inputParameterList);
                    IncludeActionType_DeleteHeader = new intParameter("@IncludeActionType_DeleteHeader", base.inputParameterList);
                    IncludeActionType_ReapplyTotalAllocation = new intParameter("@IncludeActionType_ReapplyTotalAllocation", base.inputParameterList);
                    IncludeActionType_Release = new intParameter("@IncludeActionType_Release", base.inputParameterList);
                    IncludeActionType_RemoveAPI_Workflow = new intParameter("@IncludeActionType_RemoveAPI_Workflow", base.inputParameterList);
                    IncludeActionType_Reset = new intParameter("@IncludeActionType_Reset", base.inputParameterList);
                    IncludeActionType_StyleNeed = new intParameter("@IncludeActionType_StyleNeed", base.inputParameterList);
                    IncludeActionType_BalanceToVSW = new intParameter("@IncludeActionType_BalanceToVSW", base.inputParameterList);  // TT#1334-MD - stodd - Balance to VSW Action
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? RestrictToStyle_HN_RID,
                                      int? UseDateRange,
                                      DateTime? StartDate,
                                      DateTime? EndDate,
                                      int? ResultLimit,
                                      int? RestrictMethods,
                                      string MethodRIDsToInclude,
                                      int? RestrictUsers,
                                      string UserRIDsToInclude,
                                      int? RestrictHeaders,
                                      string HeaderRIDsToInclude,
                                      int? Include_Receipt,
                                      int? Include_ASN,
                                      int? Include_Dummy,
                                      int? Include_DropShip,
                                      int? Include_MultiHeader,
                                      int? Include_Reserve,
                                      int? Include_WorkupTotalBuy,
                                      int? Include_PurchaseOrder,
                                      int? Include_Assortment,
                                      int? Include_Placeholder,
                                      int? Include_IMO,
                                      int? Include_Master, // TT#1966-MD - JSmith - DC Fulfillment
                                      int? Include_ReceivedOutOfBalance,
                                      int? Include_ReceivedInBalance,
                                      int? Include_InUseByMultiHeader,
                                      int? Include_PartialSizeOutOfBalance,
                                      int? Include_PartialSizeInBalance,
                                      int? Include_AllocatedOutOfBalance,
                                      int? Include_AllocatedInBalance,
                                      int? Include_SizesOutOfBalance,
                                      int? Include_AllInBalance,
                                      int? Include_Released,
                                      int? Include_ReleaseApproved,
                                      int? Include_AllocationStarted,
                                      int? IncludeActionType_ActionUnassigned,
                                      int? IncludeActionType_ApplyAPI_Workflow,
                                      int? IncludeActionType_BackoutAllocation,
                                      int? IncludeActionType_BackoutDetailPackAllocation,
                                      int? IncludeActionType_BackoutSizeAllocation,
                                      int? IncludeActionType_BackoutSizeIntransit,
                                      int? IncludeActionType_BackoutStyleIntransit,
                                      int? IncludeActionType_BalanceSizeBilaterally,
                                      int? IncludeActionType_BalanceSizeNoSubs,
                                      int? IncludeActionType_BalanceSizeWithConstraints,
                                      int? IncludeActionType_BalanceSizeWithSubs,
                                      int? IncludeActionType_BalanceStyleProportional,
                                      int? IncludeActionType_BalanceToDC,
                                      int? IncludeActionType_BreakoutSizesAsReceived,
                                      int? IncludeActionType_BreakoutSizesAsReceivedWithConstraints,
                                      int? IncludeActionType_ChargeIntransit,
                                      int? IncludeActionType_ChargeSizeIntransit,
                                      int? IncludeActionType_DeleteHeader,
                                      int? IncludeActionType_ReapplyTotalAllocation,
                                      int? IncludeActionType_Release,
                                      int? IncludeActionType_RemoveAPI_Workflow,
                                      int? IncludeActionType_Reset,
                                      int? IncludeActionType_StyleNeed,
                                      int? IncludeActionType_BalanceToVSW   // TT#1334-MD - stodd - Balance to VSW Action
                                      )
                {
                    lock (typeof(MID_GET_ALLOCATION_ANALYSIS_def))
                    {
                        this.RestrictToStyle_HN_RID.SetValue(RestrictToStyle_HN_RID);
                        this.UseDateRange.SetValue(UseDateRange);
                        this.StartDate.SetValue(StartDate);
                        this.EndDate.SetValue(EndDate);
                        this.ResultLimit.SetValue(ResultLimit);
                        this.RestrictMethods.SetValue(RestrictMethods);
                        this.MethodRIDsToInclude.SetValue(MethodRIDsToInclude);
                        this.RestrictUsers.SetValue(RestrictUsers);
                        this.UserRIDsToInclude.SetValue(UserRIDsToInclude);
                        this.RestrictHeaders.SetValue(RestrictHeaders);
                        this.HeaderRIDsToInclude.SetValue(HeaderRIDsToInclude);
                        this.Include_Receipt.SetValue(Include_Receipt);
                        this.Include_ASN.SetValue(Include_ASN);
                        this.Include_Dummy.SetValue(Include_Dummy);
                        this.Include_DropShip.SetValue(Include_DropShip);
                        this.Include_MultiHeader.SetValue(Include_MultiHeader);
                        this.Include_Reserve.SetValue(Include_Reserve);
                        this.Include_WorkupTotalBuy.SetValue(Include_WorkupTotalBuy);
                        this.Include_PurchaseOrder.SetValue(Include_PurchaseOrder);
                        this.Include_Assortment.SetValue(Include_Assortment);
                        this.Include_Placeholder.SetValue(Include_Placeholder);
                        this.Include_IMO.SetValue(Include_IMO);
                        this.Include_Master.SetValue(Include_Master); // TT#1966-MD - JSmith - DC Fulfillment
                        this.Include_ReceivedOutOfBalance.SetValue(Include_ReceivedOutOfBalance);
                        this.Include_ReceivedInBalance.SetValue(Include_ReceivedInBalance);
                        this.Include_InUseByMultiHeader.SetValue(Include_InUseByMultiHeader);
                        this.Include_PartialSizeOutOfBalance.SetValue(Include_PartialSizeOutOfBalance);
                        this.Include_PartialSizeInBalance.SetValue(Include_PartialSizeInBalance);
                        this.Include_AllocatedOutOfBalance.SetValue(Include_AllocatedOutOfBalance);
                        this.Include_AllocatedInBalance.SetValue(Include_AllocatedInBalance);
                        this.Include_SizesOutOfBalance.SetValue(Include_SizesOutOfBalance);
                        this.Include_AllInBalance.SetValue(Include_AllInBalance);
                        this.Include_Released.SetValue(Include_Released);
                        this.Include_ReleaseApproved.SetValue(Include_ReleaseApproved);
                        this.Include_AllocationStarted.SetValue(Include_AllocationStarted);
                        this.IncludeActionType_ActionUnassigned.SetValue(IncludeActionType_ActionUnassigned);
                        this.IncludeActionType_ApplyAPI_Workflow.SetValue(IncludeActionType_ApplyAPI_Workflow);
                        this.IncludeActionType_BackoutAllocation.SetValue(IncludeActionType_BackoutAllocation);
                        this.IncludeActionType_BackoutDetailPackAllocation.SetValue(IncludeActionType_BackoutDetailPackAllocation);
                        this.IncludeActionType_BackoutSizeAllocation.SetValue(IncludeActionType_BackoutSizeAllocation);
                        this.IncludeActionType_BackoutSizeIntransit.SetValue(IncludeActionType_BackoutSizeIntransit);
                        this.IncludeActionType_BackoutStyleIntransit.SetValue(IncludeActionType_BackoutStyleIntransit);
                        this.IncludeActionType_BalanceSizeBilaterally.SetValue(IncludeActionType_BalanceSizeBilaterally);
                        this.IncludeActionType_BalanceSizeNoSubs.SetValue(IncludeActionType_BalanceSizeNoSubs);
                        this.IncludeActionType_BalanceSizeWithConstraints.SetValue(IncludeActionType_BalanceSizeWithConstraints);
                        this.IncludeActionType_BalanceSizeWithSubs.SetValue(IncludeActionType_BalanceSizeWithSubs);
                        this.IncludeActionType_BalanceStyleProportional.SetValue(IncludeActionType_BalanceStyleProportional);
                        this.IncludeActionType_BalanceToDC.SetValue(IncludeActionType_BalanceToDC);
                        this.IncludeActionType_BreakoutSizesAsReceived.SetValue(IncludeActionType_BreakoutSizesAsReceived);
                        this.IncludeActionType_BreakoutSizesAsReceivedWithConstraints.SetValue(IncludeActionType_BreakoutSizesAsReceivedWithConstraints);
                        this.IncludeActionType_ChargeIntransit.SetValue(IncludeActionType_ChargeIntransit);
                        this.IncludeActionType_ChargeSizeIntransit.SetValue(IncludeActionType_ChargeSizeIntransit);
                        this.IncludeActionType_DeleteHeader.SetValue(IncludeActionType_DeleteHeader);
                        this.IncludeActionType_ReapplyTotalAllocation.SetValue(IncludeActionType_ReapplyTotalAllocation);
                        this.IncludeActionType_Release.SetValue(IncludeActionType_Release);
                        this.IncludeActionType_RemoveAPI_Workflow.SetValue(IncludeActionType_RemoveAPI_Workflow);
                        this.IncludeActionType_Reset.SetValue(IncludeActionType_Reset);
                        this.IncludeActionType_StyleNeed.SetValue(IncludeActionType_StyleNeed);
                        this.IncludeActionType_BalanceToVSW.SetValue(IncludeActionType_BalanceToVSW);   // TT#1334-MD - stodd - Balance to VSW Action
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static MID_GET_ALLOCATION_BY_STORE_def MID_GET_ALLOCATION_BY_STORE = new MID_GET_ALLOCATION_BY_STORE_def();
            public class MID_GET_ALLOCATION_BY_STORE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ALLOCATION_BY_STORE.SQL"

                private intParameter ST_RID;
                private intParameter Include_Receipt;
                private intParameter Include_ASN;
                private intParameter Include_Dummy;
                private intParameter Include_DropShip;
                private intParameter Include_MultiHeader;
                private intParameter Include_Reserve;
                private intParameter Include_WorkupTotalBuy;
                private intParameter Include_PurchaseOrder;
                private intParameter Include_Assortment;
                private intParameter Include_Placeholder;
                private intParameter Include_IMO;
                private intParameter Include_Master;  // TT#1966-MD - JSmith - DC Fulfillment
                private intParameter Include_ReceivedOutOfBalance;
                private intParameter Include_ReceivedInBalance;
                private intParameter Include_InUseByMultiHeader;
                private intParameter Include_PartialSizeOutOfBalance;
                private intParameter Include_PartialSizeInBalance;
                private intParameter Include_AllocatedOutOfBalance;
                private intParameter Include_AllocatedInBalance;
                private intParameter Include_SizesOutOfBalance;
                private intParameter Include_AllInBalance;
                private intParameter Include_Released;
                private intParameter Include_ReleaseApproved;
                private intParameter Include_AllocationStarted;

                public MID_GET_ALLOCATION_BY_STORE_def()
                {
                    base.procedureName = "MID_GET_ALLOCATION_BY_STORE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GET_ALLOCATION_BY_STORE");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    Include_Receipt = new intParameter("@Include_Receipt", base.inputParameterList);
                    Include_ASN = new intParameter("@Include_ASN", base.inputParameterList);
                    Include_Dummy = new intParameter("@Include_Dummy", base.inputParameterList);
                    Include_DropShip = new intParameter("@Include_DropShip", base.inputParameterList);
                    Include_MultiHeader = new intParameter("@Include_MultiHeader", base.inputParameterList);
                    Include_Reserve = new intParameter("@Include_Reserve", base.inputParameterList);
                    Include_WorkupTotalBuy = new intParameter("@Include_WorkupTotalBuy", base.inputParameterList);
                    Include_PurchaseOrder = new intParameter("@Include_PurchaseOrder", base.inputParameterList);
                    Include_Assortment = new intParameter("@Include_Assortment", base.inputParameterList);
                    Include_Placeholder = new intParameter("@Include_Placeholder", base.inputParameterList);
                    Include_IMO = new intParameter("@Include_IMO", base.inputParameterList);
                    Include_Master = new intParameter("@Include_Master", base.inputParameterList);  // TT#1966-MD - JSmith - DC Fulfillment
                    Include_ReceivedOutOfBalance = new intParameter("@Include_ReceivedOutOfBalance", base.inputParameterList);
                    Include_ReceivedInBalance = new intParameter("@Include_ReceivedInBalance", base.inputParameterList);
                    Include_InUseByMultiHeader = new intParameter("@Include_InUseByMultiHeader", base.inputParameterList);
                    Include_PartialSizeOutOfBalance = new intParameter("@Include_PartialSizeOutOfBalance", base.inputParameterList);
                    Include_PartialSizeInBalance = new intParameter("@Include_PartialSizeInBalance", base.inputParameterList);
                    Include_AllocatedOutOfBalance = new intParameter("@Include_AllocatedOutOfBalance", base.inputParameterList);
                    Include_AllocatedInBalance = new intParameter("@Include_AllocatedInBalance", base.inputParameterList);
                    Include_SizesOutOfBalance = new intParameter("@Include_SizesOutOfBalance", base.inputParameterList);
                    Include_AllInBalance = new intParameter("@Include_AllInBalance", base.inputParameterList);
                    Include_Released = new intParameter("@Include_Released", base.inputParameterList);
                    Include_ReleaseApproved = new intParameter("@Include_ReleaseApproved", base.inputParameterList);
                    Include_AllocationStarted = new intParameter("@Include_AllocationStarted", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? ST_RID,
                                      int? Include_Receipt,
                                      int? Include_ASN,
                                      int? Include_Dummy,
                                      int? Include_DropShip,
                                      int? Include_MultiHeader,
                                      int? Include_Reserve,
                                      int? Include_WorkupTotalBuy,
                                      int? Include_PurchaseOrder,
                                      int? Include_Assortment,
                                      int? Include_Placeholder,
                                      int? Include_IMO,
                                      int? Include_Master,   // TT#1966-MD - JSmith - DC Fulfillment
                                      int? Include_ReceivedOutOfBalance,
                                      int? Include_ReceivedInBalance,
                                      int? Include_InUseByMultiHeader,
                                      int? Include_PartialSizeOutOfBalance,
                                      int? Include_PartialSizeInBalance,
                                      int? Include_AllocatedOutOfBalance,
                                      int? Include_AllocatedInBalance,
                                      int? Include_SizesOutOfBalance,
                                      int? Include_AllInBalance,
                                      int? Include_Released,
                                      int? Include_ReleaseApproved,
                                      int? Include_AllocationStarted
                                      )
                {
                    lock (typeof(MID_GET_ALLOCATION_BY_STORE_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.Include_Receipt.SetValue(Include_Receipt);
                        this.Include_ASN.SetValue(Include_ASN);
                        this.Include_Dummy.SetValue(Include_Dummy);
                        this.Include_DropShip.SetValue(Include_DropShip);
                        this.Include_MultiHeader.SetValue(Include_MultiHeader);
                        this.Include_Reserve.SetValue(Include_Reserve);
                        this.Include_WorkupTotalBuy.SetValue(Include_WorkupTotalBuy);
                        this.Include_PurchaseOrder.SetValue(Include_PurchaseOrder);
                        this.Include_Assortment.SetValue(Include_Assortment);
                        this.Include_Placeholder.SetValue(Include_Placeholder);
                        this.Include_IMO.SetValue(Include_IMO);
                        this.Include_Master.SetValue(Include_Master);  // TT#1966-MD - JSmith - DC Fulfillment
                        this.Include_ReceivedOutOfBalance.SetValue(Include_ReceivedOutOfBalance);
                        this.Include_ReceivedInBalance.SetValue(Include_ReceivedInBalance);
                        this.Include_InUseByMultiHeader.SetValue(Include_InUseByMultiHeader);
                        this.Include_PartialSizeOutOfBalance.SetValue(Include_PartialSizeOutOfBalance);
                        this.Include_PartialSizeInBalance.SetValue(Include_PartialSizeInBalance);
                        this.Include_AllocatedOutOfBalance.SetValue(Include_AllocatedOutOfBalance);
                        this.Include_AllocatedInBalance.SetValue(Include_AllocatedInBalance);
                        this.Include_SizesOutOfBalance.SetValue(Include_SizesOutOfBalance);
                        this.Include_AllInBalance.SetValue(Include_AllInBalance);
                        this.Include_Released.SetValue(Include_Released);
                        this.Include_ReleaseApproved.SetValue(Include_ReleaseApproved);
                        this.Include_AllocationStarted.SetValue(Include_AllocationStarted);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }



            public static SP_GET_USER_OPTIONS_REVIEW_REPORT_def SP_GET_USER_OPTIONS_REVIEW_REPORT = new SP_GET_USER_OPTIONS_REVIEW_REPORT_def();
            public class SP_GET_USER_OPTIONS_REVIEW_REPORT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_USER_OPTIONS_REVIEW_REPORT.SQL"

                private intParameter SELECTED_AUDIT_LOGGING_LEVEL;
                private charParameter SELECTED_FORECAST_MONITOR;
                private charParameter SELECTED_SALES_MONITOR;
                private charParameter SELECTED_DCFULFILLMENT_MONITOR;
			
			    public SP_GET_USER_OPTIONS_REVIEW_REPORT_def()
			    {
                    base.procedureName = "SP_GET_USER_OPTIONS_REVIEW_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_OPTIONS");
			        SELECTED_AUDIT_LOGGING_LEVEL = new intParameter("@SELECTED_AUDIT_LOGGING_LEVEL", base.inputParameterList);
			        SELECTED_FORECAST_MONITOR = new charParameter("@SELECTED_FORECAST_MONITOR", base.inputParameterList);
			        SELECTED_SALES_MONITOR = new charParameter("@SELECTED_SALES_MONITOR", base.inputParameterList);
                    SELECTED_DCFULFILLMENT_MONITOR = new charParameter("@SELECTED_DCFULFILLMENT_MONITOR", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_AUDIT_LOGGING_LEVEL,
			                          char? SELECTED_FORECAST_MONITOR,
			                          char? SELECTED_SALES_MONITOR,
                                      char? SELECTED_DCFULFILLMENT_MONITOR
			                          )
			    {
                    lock (typeof(SP_GET_USER_OPTIONS_REVIEW_REPORT_def))
                    {
                        this.SELECTED_AUDIT_LOGGING_LEVEL.SetValue(SELECTED_AUDIT_LOGGING_LEVEL);
                        this.SELECTED_FORECAST_MONITOR.SetValue(SELECTED_FORECAST_MONITOR);
                        this.SELECTED_SALES_MONITOR.SetValue(SELECTED_SALES_MONITOR);
                        this.SELECTED_DCFULFILLMENT_MONITOR.SetValue(SELECTED_DCFULFILLMENT_MONITOR);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static SIZE_CURVE_SIMILAR_STORES_REPORT_READ_def SIZE_CURVE_SIMILAR_STORES_REPORT_READ = new SIZE_CURVE_SIMILAR_STORES_REPORT_READ_def();
			public class SIZE_CURVE_SIMILAR_STORES_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_SIZE_CURVE_SIMILAR_STORES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public SIZE_CURVE_SIMILAR_STORES_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_SIZE_CURVE_SIMILAR_STORES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(SIZE_CURVE_SIMILAR_STORES_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_SIZE_CURVE_TOLERANCE_REPORT_def SP_GET_SIZE_CURVE_TOLERANCE_REPORT = new SP_GET_SIZE_CURVE_TOLERANCE_REPORT_def();
            public class SP_GET_SIZE_CURVE_TOLERANCE_REPORT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_SIZE_CURVE_TOLERANCE_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public SP_GET_SIZE_CURVE_TOLERANCE_REPORT_def()
			    {
                    base.procedureName = "SP_GET_SIZE_CURVE_TOLERANCE_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(SP_GET_SIZE_CURVE_TOLERANCE_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static CHAIN_SET_PERCENT_REPORT_READ_def CHAIN_SET_PERCENT_REPORT_READ = new CHAIN_SET_PERCENT_REPORT_READ_def();
			public class CHAIN_SET_PERCENT_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_CHAIN_SET_PERCENT_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private stringParameter STORE_ID;
                private stringParameter STORE_RID_LIST;
                private intParameter BEG_WEEK;
                private intParameter END_WEEK;
			
			    public CHAIN_SET_PERCENT_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_CHAIN_SET_PERCENT_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_GROUP_LEVEL");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
			        STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
			        BEG_WEEK = new intParameter("@BEG_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          string STORE_ID,
			                          string STORE_RID_LIST,
			                          int? BEG_WEEK,
			                          int? END_WEEK
			                          )
			    {
                    lock (typeof(CHAIN_SET_PERCENT_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.STORE_ID.SetValue(STORE_ID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        this.BEG_WEEK.SetValue(BEG_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static CHARACTERISTICS_REPORT_READ_def CHARACTERISTICS_REPORT_READ = new CHARACTERISTICS_REPORT_READ_def();
			public class CHARACTERISTICS_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_CHARACTERISTICS_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public CHARACTERISTICS_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_CHARACTERISTICS_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(CHARACTERISTICS_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static STOCK_MIN_MAX_REPORT_READ_def STOCK_MIN_MAX_REPORT_READ = new STOCK_MIN_MAX_REPORT_READ_def();
			public class STOCK_MIN_MAX_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_STOCK_MIN_MAX_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public STOCK_MIN_MAX_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_STOCK_MIN_MAX_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("NODE_STOCK_MIN_MAX");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(STOCK_MIN_MAX_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static VELOCITYGRADES_REPORT_READ_def VELOCITYGRADES_REPORT_READ = new VELOCITYGRADES_REPORT_READ_def();
			public class VELOCITYGRADES_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_VELOCITYGRADES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public VELOCITYGRADES_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_VELOCITYGRADES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("VELOCITY_GRADE");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(VELOCITYGRADES_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_STORE_GRADES_REPORT_def SP_GET_STORE_GRADES_REPORT = new SP_GET_STORE_GRADES_REPORT_def();
            public class SP_GET_STORE_GRADES_REPORT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_STORE_GRADES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private intParameter ShowStoreGrades;
                private intParameter ShowAllocationMinMax;
			
			    public SP_GET_STORE_GRADES_REPORT_def()
			    {
                    base.procedureName = "SP_GET_STORE_GRADES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_GRADES");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
                    ShowStoreGrades = new intParameter("@ShowStoreGrades", base.inputParameterList);
                    ShowAllocationMinMax = new intParameter("@ShowAllocationMinMax", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
                                      int? ShowStoreGrades,
                                      int? ShowAllocationMinMax
			                          )
			    {
                    lock (typeof(SP_GET_STORE_GRADES_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.ShowStoreGrades.SetValue(ShowStoreGrades);
                        this.ShowAllocationMinMax.SetValue(ShowAllocationMinMax);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_STORE_ELIGIBILITY_REPORT_def SP_GET_STORE_ELIGIBILITY_REPORT = new SP_GET_STORE_ELIGIBILITY_REPORT_def();
            public class SP_GET_STORE_ELIGIBILITY_REPORT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_STORE_ELIGIBILITY_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private stringParameter STORE_ID;
                private stringParameter STORE_RID_LIST;
                private intParameter ShowEligibility;
                private intParameter ShowModifiers;
                private intParameter ShowSimilarStore;
			
			    public SP_GET_STORE_ELIGIBILITY_REPORT_def()
			    {
			        base.procedureName = "SP_GET_STORE_ELIGIBILITY_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
			        STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
                    ShowEligibility = new intParameter("@ShowEligibility", base.inputParameterList);
                    ShowModifiers = new intParameter("@ShowModifiers", base.inputParameterList);
                    ShowSimilarStore = new intParameter("@ShowSimilarStore", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          string STORE_ID,
			                          string STORE_RID_LIST,
                                      int? ShowEligibility,
                                      int? ShowModifiers,
                                      int? ShowSimilarStore
			                          )
			    {
                    lock (typeof(SP_GET_STORE_ELIGIBILITY_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.STORE_ID.SetValue(STORE_ID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        this.ShowEligibility.SetValue(ShowEligibility);
                        this.ShowModifiers.SetValue(ShowModifiers);
                        this.ShowSimilarStore.SetValue(ShowSimilarStore);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static STORE_CAPACITY_REPORT_READ_def STORE_CAPACITY_REPORT_READ = new STORE_CAPACITY_REPORT_READ_def();
			public class STORE_CAPACITY_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_STORE_CAPACITY_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private stringParameter STORE_ID;
                private stringParameter STORE_RID_LIST;
			
			    public STORE_CAPACITY_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_STORE_CAPACITY_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
			        STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          string STORE_ID,
			                          string STORE_RID_LIST
			                          )
			    {
                    lock (typeof(STORE_CAPACITY_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.STORE_ID.SetValue(STORE_ID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static PURGE_DATES_REPORT_READ_def PURGE_DATES_REPORT_READ = new PURGE_DATES_REPORT_READ_def();
			public class PURGE_DATES_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_PURGE_DATES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public PURGE_DATES_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_PURGE_DATES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(PURGE_DATES_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_FORECAST_TYPE_REPORT_def SP_GET_FORECAST_TYPE_REPORT = new SP_GET_FORECAST_TYPE_REPORT_def();
            public class SP_GET_FORECAST_TYPE_REPORT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_FORECAST_TYPE_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public SP_GET_FORECAST_TYPE_REPORT_def()
			    {
                    base.procedureName = "SP_GET_FORECAST_TYPE_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE ");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(SP_GET_FORECAST_TYPE_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_FORECAST_LEVEL_REPORT_def SP_GET_FORECAST_LEVEL_REPORT = new SP_GET_FORECAST_LEVEL_REPORT_def();
            public class SP_GET_FORECAST_LEVEL_REPORT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_FORECAST_LEVEL_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private intParameter ShowForecastLevel;
			
			    public SP_GET_FORECAST_LEVEL_REPORT_def()
			    {
			        base.procedureName = "SP_GET_FORECAST_LEVEL_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE ");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
                    ShowForecastLevel = new intParameter("@ShowForecastLevel", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
                                      int? ShowForecastLevel
			                          )
			    {
                    lock (typeof(SP_GET_FORECAST_LEVEL_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.ShowForecastLevel.SetValue(ShowForecastLevel);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static DAILYPERCENTAGES_REPORT_READ_def DAILYPERCENTAGES_REPORT_READ = new DAILYPERCENTAGES_REPORT_READ_def();
			public class DAILYPERCENTAGES_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_DAILYPERCENTAGES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private stringParameter STORE_ID;
                private stringParameter STORE_RID_LIST;
			
			    public DAILYPERCENTAGES_REPORT_READ_def()
			    {
			        base.procedureName = "SP_DAILYPERCENTAGES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
			        STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          string STORE_ID,
			                          string STORE_RID_LIST
			                          )
			    {
                    lock (typeof(DAILYPERCENTAGES_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.STORE_ID.SetValue(STORE_ID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static FORECAST_AUDIT_MODIFYSALES_REPORT_READ_def FORECAST_AUDIT_MODIFYSALES_REPORT_READ = new FORECAST_AUDIT_MODIFYSALES_REPORT_READ_def();
			public class FORECAST_AUDIT_MODIFYSALES_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_FORECAST_AUDIT_MODIFYSALES_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private intParameter FV_RID;
                private intParameter USER_RID;
                private stringParameter TIME_RANGE_BEGIN;
                private stringParameter TIME_RANGE_END;
                private intParameter USER_GROUP_RID;
			
			    public FORECAST_AUDIT_MODIFYSALES_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_FORECAST_AUDIT_MODIFYSALES_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("AUDIT_FORECAST");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        TIME_RANGE_BEGIN = new stringParameter("@TIME_RANGE_BEGIN", base.inputParameterList);
			        TIME_RANGE_END = new stringParameter("@TIME_RANGE_END", base.inputParameterList);
			        USER_GROUP_RID = new intParameter("@USER_GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          int? FV_RID,
			                          int? USER_RID,
			                          string TIME_RANGE_BEGIN,
			                          string TIME_RANGE_END,
			                          int? USER_GROUP_RID
			                          )
			    {
                    lock (typeof(FORECAST_AUDIT_MODIFYSALES_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.FV_RID.SetValue(FV_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.TIME_RANGE_BEGIN.SetValue(TIME_RANGE_BEGIN);
                        this.TIME_RANGE_END.SetValue(TIME_RANGE_END);
                        this.USER_GROUP_RID.SetValue(USER_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT_def SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT = new SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT_def();
            public class SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private intParameter FV_RID;
                private intParameter USER_RID;
                private stringParameter TIME_RANGE_BEGIN;
                private stringParameter TIME_RANGE_END;
                private intParameter USER_GROUP_RID;
			
			    public SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT_def()
			    {
                    base.procedureName = "SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("AUDIT_FORECAST");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        TIME_RANGE_BEGIN = new stringParameter("@TIME_RANGE_BEGIN", base.inputParameterList);
			        TIME_RANGE_END = new stringParameter("@TIME_RANGE_END", base.inputParameterList);
			        USER_GROUP_RID = new intParameter("@USER_GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          int? FV_RID,
			                          int? USER_RID,
			                          string TIME_RANGE_BEGIN,
			                          string TIME_RANGE_END,
			                          int? USER_GROUP_RID
			                          )
			    {
                    lock (typeof(SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.FV_RID.SetValue(FV_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.TIME_RANGE_BEGIN.SetValue(TIME_RANGE_BEGIN);
                        this.TIME_RANGE_END.SetValue(TIME_RANGE_END);
                        this.USER_GROUP_RID.SetValue(USER_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}



            public static SP_GET_FORECAST_AUDIT_BY_MERCHANDISE_def SP_GET_FORECAST_AUDIT_BY_MERCHANDISE = new SP_GET_FORECAST_AUDIT_BY_MERCHANDISE_def();
            public class SP_GET_FORECAST_AUDIT_BY_MERCHANDISE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_FORECAST_AUDIT_BY_MERCHANDISE.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private intParameter FV_RID;
                private intParameter USER_RID;
                private stringParameter TIME_RANGE_BEGIN;
                private stringParameter TIME_RANGE_END;
                private intParameter USER_GROUP_RID;
                private stringParameter PROCESS_FROM_DATE;
                private stringParameter PROCESS_TO_DATE;
			
			    public SP_GET_FORECAST_AUDIT_BY_MERCHANDISE_def()
			    {
			        base.procedureName = "SP_GET_FORECAST_AUDIT_BY_MERCHANDISE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("AUDIT_FORECAST");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        TIME_RANGE_BEGIN = new stringParameter("@TIME_RANGE_BEGIN", base.inputParameterList);
			        TIME_RANGE_END = new stringParameter("@TIME_RANGE_END", base.inputParameterList);
			        USER_GROUP_RID = new intParameter("@USER_GROUP_RID", base.inputParameterList);
			        PROCESS_FROM_DATE = new stringParameter("@PROCESS_FROM_DATE", base.inputParameterList);
			        PROCESS_TO_DATE = new stringParameter("@PROCESS_TO_DATE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL,
			                          int? FV_RID,
			                          int? USER_RID,
			                          string TIME_RANGE_BEGIN,
			                          string TIME_RANGE_END,
			                          int? USER_GROUP_RID,
			                          string PROCESS_FROM_DATE,
			                          string PROCESS_TO_DATE
			                          )
			    {
                    lock (typeof(SP_GET_FORECAST_AUDIT_BY_MERCHANDISE_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.FV_RID.SetValue(FV_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.TIME_RANGE_BEGIN.SetValue(TIME_RANGE_BEGIN);
                        this.TIME_RANGE_END.SetValue(TIME_RANGE_END);
                        this.USER_GROUP_RID.SetValue(USER_GROUP_RID);
                        this.PROCESS_FROM_DATE.SetValue(PROCESS_FROM_DATE);
                        this.PROCESS_TO_DATE.SetValue(PROCESS_TO_DATE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static HEADER_AUDIT_REPORT_READ_def HEADER_AUDIT_REPORT_READ = new HEADER_AUDIT_REPORT_READ_def();
			public class HEADER_AUDIT_REPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_HEADER_AUDIT_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter PLAN_HNRID;
                private intParameter USER_RID;
                private intParameter USER_GROUP_RID;
                private stringParameter PROCESS_FROM_DATE;
                private stringParameter PROCESS_TO_DATE;
                private stringParameter HEADER_RID_LIST;
			
			    public HEADER_AUDIT_REPORT_READ_def()
			    {
			        base.procedureName = "SP_GET_HEADER_AUDIT_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PRODUCT_HIERARCHY");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        PLAN_HNRID = new intParameter("@PLAN_HNRID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_GROUP_RID = new intParameter("@USER_GROUP_RID", base.inputParameterList);
			        PROCESS_FROM_DATE = new stringParameter("@PROCESS_FROM_DATE", base.inputParameterList);
			        PROCESS_TO_DATE = new stringParameter("@PROCESS_TO_DATE", base.inputParameterList);
			        HEADER_RID_LIST = new stringParameter("@HEADER_RID_LIST", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? PLAN_HNRID,
			                          int? USER_RID,
			                          int? USER_GROUP_RID,
			                          string PROCESS_FROM_DATE,
			                          string PROCESS_TO_DATE,
			                          string HEADER_RID_LIST
			                          )
			    {
                    lock (typeof(HEADER_AUDIT_REPORT_READ_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.PLAN_HNRID.SetValue(PLAN_HNRID);
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_GROUP_RID.SetValue(USER_GROUP_RID);
                        this.PROCESS_FROM_DATE.SetValue(PROCESS_FROM_DATE);
                        this.PROCESS_TO_DATE.SetValue(PROCESS_TO_DATE);
                        this.HEADER_RID_LIST.SetValue(HEADER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_GET_SIZE_CURVE_CRITERIA_REPORT_def SP_GET_SIZE_CURVE_CRITERIA_REPORT = new SP_GET_SIZE_CURVE_CRITERIA_REPORT_def();
            public class SP_GET_SIZE_CURVE_CRITERIA_REPORT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_SIZE_CURVE_CRITERIA_REPORT.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
			
			    public SP_GET_SIZE_CURVE_CRITERIA_REPORT_def()
			    {
                    base.procedureName = "SP_GET_SIZE_CURVE_CRITERIA_REPORT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE ");
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SELECTED_NODE_RID,
			                          int? LOWER_LEVEL
			                          )
			    {
                    lock (typeof(SP_GET_SIZE_CURVE_CRITERIA_REPORT_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#4041 -jsobek -Node Properties Override report doesn't show decimals or FWOS Max Models
            //public static SP_GET_VSW_REPORT_def SP_GET_VSW_REPORT = new SP_GET_VSW_REPORT_def();
            //public class SP_GET_VSW_REPORT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_VSW_REPORT.SQL"

            //    public intParameter SELECTED_NODE_RID;
            //    public intParameter LOWER_LEVEL;
            //    public stringParameter STORE_ID;
            //    public stringParameter STORE_RID_LIST;
			
            //    public SP_GET_VSW_REPORT_def()
            //    {
            //        base.procedureName = "SP_GET_VSW_REPORT";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("STORES");
            //        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
            //        LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
            //        STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
            //        STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
            //    }
			
            //    public DataTable Read(DatabaseAccess _dba, 
            //                          int? SELECTED_NODE_RID,
            //                          int? LOWER_LEVEL,
            //                          string STORE_ID,
            //                          string STORE_RID_LIST
            //                          )
            //    {
            //        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
            //        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
            //        this.STORE_ID.SetValue(STORE_ID);
            //        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}
            public static MID_REPORT_READ_VSW_OVERRIDE_def MID_REPORT_READ_VSW_OVERRIDE = new MID_REPORT_READ_VSW_OVERRIDE_def();
            public class MID_REPORT_READ_VSW_OVERRIDE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_REPORT_READ_VSW_OVERRIDE.SQL"

                private intParameter SELECTED_NODE_RID;
                private intParameter LOWER_LEVEL;
                private stringParameter STORE_ID;
                private stringParameter STORE_RID_LIST;

                public MID_REPORT_READ_VSW_OVERRIDE_def()
                {
                    base.procedureName = "MID_REPORT_READ_VSW_OVERRIDE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORES");
                    SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
                    LOWER_LEVEL = new intParameter("@LOWER_LEVEL", base.inputParameterList);
                    STORE_ID = new stringParameter("@STORE_ID", base.inputParameterList);
                    STORE_RID_LIST = new stringParameter("@STORE_RID_LIST", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? SELECTED_NODE_RID,
                                      int? LOWER_LEVEL,
                                      string STORE_ID,
                                      string STORE_RID_LIST
                                      )
                {
                    lock (typeof(MID_REPORT_READ_VSW_OVERRIDE_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.LOWER_LEVEL.SetValue(LOWER_LEVEL);
                        this.STORE_ID.SetValue(STORE_ID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#4041 -jsobek -Node Properties Override report doesn't show decimals or FWOS Max Models

            public static SP_GET_REPORTS_def SP_GET_REPORTS = new SP_GET_REPORTS_def();
            public class SP_GET_REPORTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_GET_REPORTS.SQL"

                private intParameter ShowEligibility;
                private intParameter ShowModifiers;
                private intParameter ShowSimilarStore;
                private intParameter ShowStoreGrades;
                private intParameter ShowAllocationMinMax;
                private intParameter ShowVelocityGrades;
                private intParameter ShowCapacity;
                private intParameter ShowDailypercentages;
                private intParameter ShowPurgeCriteria;
                private intParameter ShowForecastLevel;
                private intParameter ShowForecastType;
                private intParameter ShowStockMinMax;
                private intParameter ShowCharacteristics;
                private intParameter ShowChainSetPercent;
                private intParameter ShowVSW;
                private intParameter ShowSizeCurvesCriteria;
                private intParameter ShowSizeCurvesTolerance;
                private intParameter ShowSizeCurvesSimilarStores;
			
			    public SP_GET_REPORTS_def()
			    {
                    base.procedureName = "SP_GET_REPORTS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GET_REPORTS");
			        ShowEligibility = new intParameter("@ShowEligibility", base.inputParameterList);
			        ShowModifiers = new intParameter("@ShowModifiers", base.inputParameterList);
			        ShowSimilarStore = new intParameter("@ShowSimilarStore", base.inputParameterList);
			        ShowStoreGrades = new intParameter("@ShowStoreGrades", base.inputParameterList);
			        ShowAllocationMinMax = new intParameter("@ShowAllocationMinMax", base.inputParameterList);
			        ShowVelocityGrades = new intParameter("@ShowVelocityGrades", base.inputParameterList);
			        ShowCapacity = new intParameter("@ShowCapacity", base.inputParameterList);
			        ShowDailypercentages = new intParameter("@ShowDailypercentages", base.inputParameterList);
			        ShowPurgeCriteria = new intParameter("@ShowPurgeCriteria", base.inputParameterList);
			        ShowForecastLevel = new intParameter("@ShowForecastLevel", base.inputParameterList);
			        ShowForecastType = new intParameter("@ShowForecastType", base.inputParameterList);
			        ShowStockMinMax = new intParameter("@ShowStockMinMax", base.inputParameterList);
			        ShowCharacteristics = new intParameter("@ShowCharacteristics", base.inputParameterList);
			        ShowChainSetPercent = new intParameter("@ShowChainSetPercent", base.inputParameterList);
			        ShowVSW = new intParameter("@ShowVSW", base.inputParameterList);
			        ShowSizeCurvesCriteria = new intParameter("@ShowSizeCurvesCriteria", base.inputParameterList);
			        ShowSizeCurvesTolerance = new intParameter("@ShowSizeCurvesTolerance", base.inputParameterList);
			        ShowSizeCurvesSimilarStores = new intParameter("@ShowSizeCurvesSimilarStores", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? ShowEligibility,
			                          int? ShowModifiers,
			                          int? ShowSimilarStore,
			                          int? ShowStoreGrades,
			                          int? ShowAllocationMinMax,
			                          int? ShowVelocityGrades,
			                          int? ShowCapacity,
			                          int? ShowDailypercentages,
			                          int? ShowPurgeCriteria,
			                          int? ShowForecastLevel,
			                          int? ShowForecastType,
			                          int? ShowStockMinMax,
			                          int? ShowCharacteristics,
			                          int? ShowChainSetPercent,
			                          int? ShowVSW,
			                          int? ShowSizeCurvesCriteria,
			                          int? ShowSizeCurvesTolerance,
			                          int? ShowSizeCurvesSimilarStores
			                          )
			    {
                    lock (typeof(SP_GET_REPORTS_def))
                    {
                        this.ShowEligibility.SetValue(ShowEligibility);
                        this.ShowModifiers.SetValue(ShowModifiers);
                        this.ShowSimilarStore.SetValue(ShowSimilarStore);
                        this.ShowStoreGrades.SetValue(ShowStoreGrades);
                        this.ShowAllocationMinMax.SetValue(ShowAllocationMinMax);
                        this.ShowVelocityGrades.SetValue(ShowVelocityGrades);
                        this.ShowCapacity.SetValue(ShowCapacity);
                        this.ShowDailypercentages.SetValue(ShowDailypercentages);
                        this.ShowPurgeCriteria.SetValue(ShowPurgeCriteria);
                        this.ShowForecastLevel.SetValue(ShowForecastLevel);
                        this.ShowForecastType.SetValue(ShowForecastType);
                        this.ShowStockMinMax.SetValue(ShowStockMinMax);
                        this.ShowCharacteristics.SetValue(ShowCharacteristics);
                        this.ShowChainSetPercent.SetValue(ShowChainSetPercent);
                        this.ShowVSW.SetValue(ShowVSW);
                        this.ShowSizeCurvesCriteria.SetValue(ShowSizeCurvesCriteria);
                        this.ShowSizeCurvesTolerance.SetValue(ShowSizeCurvesTolerance);
                        this.ShowSizeCurvesSimilarStores.SetValue(ShowSizeCurvesSimilarStores);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
