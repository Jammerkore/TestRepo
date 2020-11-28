--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes top X rows from the store imo revision table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_IMO_REV_DELETE] 
	@COMMIT_LIMIT int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) 
	FROM STORE_IMO_REV WITH (ROWLOCK) 
	WHERE FLASHBACK < (SELECT TOP 1 POSTING_DATE FROM POSTING_DATE)
	
	SELECT @@ROWCOUNT;

END
GO