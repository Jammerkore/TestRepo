--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the node imo table based on the hierarchy rid and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_IMO_DELETE_FROM_STORE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM NODE_IMO 
    WHERE HN_RID = @HN_RID
          AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
