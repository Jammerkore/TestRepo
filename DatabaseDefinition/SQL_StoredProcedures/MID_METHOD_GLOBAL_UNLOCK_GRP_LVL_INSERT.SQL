--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Inserts a global unlock for group levels
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT] 
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_GLOBAL_UNLOCK_GRP_LVL
		(
		METHOD_RID, 
		SGL_RID
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID
		)

	SELECT @@ROWCOUNT;

END
GO