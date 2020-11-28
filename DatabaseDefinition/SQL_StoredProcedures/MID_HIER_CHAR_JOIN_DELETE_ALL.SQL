--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all rows on the hierarchy characteristic join table based on the node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIER_CHAR_JOIN WITH (ROWLOCK) 
	WHERE HN_RID=@HN_RID

	SELECT @@ROWCOUNT;

END
GO
