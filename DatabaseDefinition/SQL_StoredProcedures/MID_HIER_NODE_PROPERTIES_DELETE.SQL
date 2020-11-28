--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a row from the hierarchy node properties table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_PROPERTIES_DELETE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM HIER_NODE_PROPERTIES WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
