--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns Method Rollup Basis Detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_READ]
	@METHOD_RID int,
	@HN_RID int,
	@FV_RID int, 
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(HN_RID, @HN_RID) HN_RID,
		COALESCE(FV_RID, @FV_RID) FV_RID,
		COALESCE(CDR_RID, @CDR_RID) CDR_RID
    FROM METHOD_ROLLUP 
	WHERE METHOD_RID = @METHOD_RID

END
GO