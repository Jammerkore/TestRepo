--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the intransit time ids for the node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_INTRANSIT_READ_TIME_IDS]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		TIME_ID 
	FROM STORE_INTRANSIT
	WHERE HN_RID = @HN_RID
	ORDER BY TIME_ID

END
GO