--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes size sell thrus based on the hierarchy node
--dv For Review:	Missing row lock??
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_SELLTHRU_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM NODE_SIZE_SELLTHRU 
    WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
