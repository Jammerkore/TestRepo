--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Reads OTS plans for the method provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OTS_PLAN_READ_FROM_METHOD] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PLAN_HN_RID, 
		PLAN_FV_RID, 
		CDR_RID, 
		CHAIN_FV_RID,
		BAL_SALES_IND, 
		BAL_STOCK_IND, 
		FORECAST_MOD_RID,
		HIGH_LEVEL_IND, 
		LOW_LEVELS_IND, 
		LOW_LEVEL_TYPE,
        LOW_LEVEL_SEQ, 
		LOW_LEVEL_OFFSET, 
		OLL_RID, 
		TREND_OPTIONS_IND, 
		TREND_OPTIONS_PLUG_CHAIN_WOS
    FROM OTS_PLAN 
	WHERE METHOD_RID = @METHOD_RID

END
GO