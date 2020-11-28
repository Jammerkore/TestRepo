--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads a global lock methods group levels
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NoRID int = -1;

	SELECT
		COALESCE(SGL_RID, @NoRID) SGL_RID
    FROM METHOD_GLOBAL_LOCK_GRP_LVL 
	WHERE METHOD_RID = @METHOD_RID
	 
END
GO












