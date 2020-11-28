--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a row in the color node table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_DELETE]
	@HN_RID int,
	@COLOR_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM COLOR_NODE WITH (ROWLOCK)  
	WHERE HN_RID = @HN_RID
		  AND COLOR_CODE_RID = @COLOR_CODE_RID

	SELECT @@ROWCOUNT;

END
GO
