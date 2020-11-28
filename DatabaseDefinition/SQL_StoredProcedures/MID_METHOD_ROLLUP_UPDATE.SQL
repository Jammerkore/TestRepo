--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Updates a rollup method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_UPDATE] 
	@METHOD_RID int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_ROLLUP 
	SET
		HN_RID = @HN_RID,
		FV_RID = @FV_RID,
		CDR_RID = @CDR_RID
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO