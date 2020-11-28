--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a global lock method group level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT] 
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_GLOBAL_LOCK_GRP_LVL
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















