--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain weekly history for the provided time ids and node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE]
	@HN_RID INT,
	@TIME_LIST TIME_ID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_HISTORY_WEEK WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
	AND TIME_ID IN (SELECT TIME_ID FROM @TIME_LIST)

	SELECT @@ROWCOUNT;

END
GO