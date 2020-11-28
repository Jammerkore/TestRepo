--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header store grades for the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_STORE_GRADE_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		hsg.HDR_RID, 
		val.SGL_RID, 
		hsg.GRADE_CODE, 
		hsg.BOUNDARY, 
		val.MINIMUM_STOCK,
        val.MAXIMUM_STOCK, 
		val.MINIMUM_AD, 
		val.MINIMUM_COLOR, 
		val.MAXIMUM_COLOR, 
		COALESCE(val.SHIP_UP_TO,0) AS SHIP_UP_TO
	FROM HEADER_STORE_GRADE hsg 
		 LEFT OUTER JOIN HEADER_STORE_GRADE_VALUES val ON hsg.HDR_RID = val.HDR_RID AND hsg.BOUNDARY = val.BOUNDARY 
    WHERE hsg.HDR_RID = @HDR_RID 
    ORDER BY val.SGL_RID, hsg.BOUNDARY DESC

END
GO
