--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a base node row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BASE_NODE_DELETE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM BASE_NODE WITH (ROWLOCK) 
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
