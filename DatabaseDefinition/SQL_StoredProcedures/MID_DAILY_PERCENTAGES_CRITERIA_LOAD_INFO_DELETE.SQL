--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row from the daily percentages criteria load info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM DAILY_PERCENTAGES_CRITERIA_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
