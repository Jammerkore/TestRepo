--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes store history for the provided time ids and node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_HISTORY_WEEK4_DELETE]
	@HN_RID INT,
	@TIME_LIST TIME_ID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_HISTORY_WEEK4 WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
	AND TIME_ID IN (SELECT TIME_ID FROM @TIME_LIST)

	SELECT @@ROWCOUNT;

END
GO