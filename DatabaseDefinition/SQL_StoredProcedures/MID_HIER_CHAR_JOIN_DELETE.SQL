--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row on the hierarchy characteristic join table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_DELETE]
	@HN_RID int,
	@HC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIER_CHAR_JOIN WITH (ROWLOCK) 
    WHERE HN_RID = @HN_RID
          AND HC_RID = @HC_RID
	
	SELECT @@ROWCOUNT;

END
GO
