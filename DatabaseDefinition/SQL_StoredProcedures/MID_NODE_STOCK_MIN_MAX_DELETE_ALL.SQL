--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all stock min max rows based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_STOCK_MIN_MAX_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM NODE_STOCK_MIN_MAX WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
