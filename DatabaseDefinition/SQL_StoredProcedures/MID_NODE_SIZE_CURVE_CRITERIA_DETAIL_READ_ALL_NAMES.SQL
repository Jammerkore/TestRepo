--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads all size curve detail names
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		NSCCD_RID, 
		COALESCE(CURVE_NAME, '') CURVE_NAME
    FROM NODE_SIZE_CURVE_CRITERIA_DETAIL
    WHERE CURVE_NAME IS NOT NULL AND RTRIM(LTRIM(CURVE_NAME)) > '' 
    ORDER BY CURVE_NAME

END
GO
