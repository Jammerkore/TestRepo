--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes top X rows from the store intransit revision table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_INTRANSIT_REV_DELETE] 
	@COMMIT_LIMIT int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) 
	FROM STORE_INTRANSIT_REV WITH (ROWLOCK)  
	WHERE TIME_ID < (SELECT TOP 1 CURR_DATE_YYYYDDD FROM POSTING_DATE)
	
	SELECT @@ROWCOUNT;

END
GO