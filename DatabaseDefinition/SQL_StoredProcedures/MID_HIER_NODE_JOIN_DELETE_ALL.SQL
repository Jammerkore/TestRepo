--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all rows from the hierarchy join table based on the hn rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_JOIN_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIER_NODE_JOIN WITH (ROWLOCK) 
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
