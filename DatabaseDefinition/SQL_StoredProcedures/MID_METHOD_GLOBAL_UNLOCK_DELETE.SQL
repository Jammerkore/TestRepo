--dv =============================================
--dv Create date:	2/5/2014
--dv Description:	Deletes a Method global unlock
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_UNLOCK_DELETE]
	@METHOD_RID int

AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GLOBAL_UNLOCK 
	WHERE
	METHOD_RID = @METHOD_RID 

	SELECT @@ROWCOUNT;
END
GO