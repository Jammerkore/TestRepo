--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain weekly history less than a purge date
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_HISTORY_WEEK_DELETE]
	@COMMIT_LIMIT INT,
	@TIME_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) chw WITH (TABLOCK) 
	FROM CHAIN_HISTORY_WEEK chw WITH (INDEX(CHAIN_HISTORY_WEEK_PK))
	WHERE TIME_ID < @TIME_ID

	SELECT @@ROWCOUNT;

END
GO