--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads size curve detail name based on size curve detail rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME]
	@NSCCD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(CURVE_NAME, '') CURVE_NAME
    FROM NODE_SIZE_CURVE_CRITERIA_DETAIL
    WHERE NSCCD_RID = @NSCCD_RID

END
GO
