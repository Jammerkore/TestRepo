--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from the method copy forecast table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_FORECAST_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NoRID int = -1;
	DECLARE @UndefinedCalendarDateRange int = 1;
	DECLARE @Undefined int = -1;
	DECLARE @eFromLevelsType_HierarchyLevel int = 1;
	DECLARE @eToLevelsType_HierarchyLevel int = 1;
	
	SELECT 
		COALESCE(HN_RID, @NoRID) HN_RID,
        COALESCE(FV_RID, @NoRID) FV_RID,
        COALESCE(CDR_RID, @UndefinedCalendarDateRange) CDR_RID,
        COALESCE(PLAN_TYPE, @Undefined) PLAN_TYPE,
        COALESCE(STORE_FILTER_RID, @NoRID) STORE_FILTER_RID,
        MULTI_LEVEL_IND,
        COALESCE(FROM_LEVEL_TYPE, @eFromLevelsType_HierarchyLevel) FROM_LEVEL_TYPE,
        COALESCE(FROM_LEVEL_OFFSET, 0) FROM_LEVEL_OFFSET,
        COALESCE(FROM_LEVEL_SEQ, 0) FROM_LEVEL_SEQ,
        COALESCE(TO_LEVEL_TYPE, @eToLevelsType_HierarchyLevel) TO_LEVEL_TYPE,
        COALESCE(TO_LEVEL_OFFSET, 0) TO_LEVEL_OFFSET,
        COALESCE(TO_LEVEL_SEQ, 0) TO_LEVEL_SEQ,
        COALESCE(PREINIT_VALUES_IND, '0') PREINIT_VALUES_IND,
        COALESCE(OLL_RID, @NoRID) OLL_RID
	FROM METHOD_COPY_FORECAST 
	WHERE METHOD_RID = @METHOD_RID

END
GO