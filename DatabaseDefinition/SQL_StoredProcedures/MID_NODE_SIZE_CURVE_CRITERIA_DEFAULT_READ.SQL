--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads size curve default criteria for the hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		NSCCD.HN_RID, 
		NSCCD.NSCCD_RID 
	FROM NODE_SIZE_CURVE_CRITERIA_DEFAULT NSCCD
	WHERE NSCCD.HN_RID = @HN_RID

END
GO
