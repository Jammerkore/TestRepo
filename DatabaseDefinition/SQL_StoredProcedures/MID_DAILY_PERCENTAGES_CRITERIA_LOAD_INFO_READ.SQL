--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads a row from the daily percentages criteria load info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM DAILY_PERCENTAGES_CRITERIA_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
