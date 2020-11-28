--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads all active forecast versions, sorted by version rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID] 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eForecastBlendType_None int = 802830;

	SELECT 
		FV_RID, 
		FV_ID, 
		DESCRIPTION, 
		PROTECT_HISTORY_IND, 
		ACTIVE_IND, 
		COALESCE(BLEND_TYPE, @eForecastBlendType_None) BLEND_TYPE,
		ACTUAL_FV_RID, 
		FORECAST_FV_RID,
		COALESCE(SIMILAR_STORE_IND,'1') SIMILAR_STORE_IND,
		COALESCE(CURRENT_BLEND_IND,'0') CURRENT_BLEND_IND 
	FROM FORECAST_VERSION 
	ORDER BY FV_RID

END
GO