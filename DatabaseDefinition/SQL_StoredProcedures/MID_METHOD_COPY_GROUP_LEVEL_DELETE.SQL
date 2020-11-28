--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Deletes rows from the method copy group level table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_GROUP_LEVEL_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_COPY_GROUP_LEVEL
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT
END
GO