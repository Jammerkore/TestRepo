--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the time ids for the node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_HISTORY_DAY1_READ_TIME_IDS]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		TIME_ID 
	FROM STORE_HISTORY_DAY1
	WHERE HN_RID = @HN_RID
	ORDER BY TIME_ID

END
GO