--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Inserts a rollup method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_INSERT] 
	@METHOD_RID int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_ROLLUP
		(
		METHOD_RID, 
		HN_RID, 
		FV_RID,
		CDR_RID
		)
   VALUES
		(
		@METHOD_RID, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID
		)

	SELECT @@ROWCOUNT;
END
GO