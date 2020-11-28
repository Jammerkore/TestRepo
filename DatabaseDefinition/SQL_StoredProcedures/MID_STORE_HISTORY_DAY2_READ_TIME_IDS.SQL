--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the time ids for the node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_HISTORY_DAY2_READ_TIME_IDS]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		TIME_ID 
	FROM STORE_HISTORY_DAY2
	WHERE HN_RID = @HN_RID
	ORDER BY TIME_ID

END
GO