--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the push to back stock info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PUSH_TO_BACK_STOCK_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROCESS_RID, 
		COALESCE(HDRS_READ,0) HDRS_READ, 
		COALESCE(HDRS_WITH_ERRORS,0) HDRS_WITH_ERRORS,
		COALESCE(HDRS_PROCESSED,0) HDRS_PROCESSED,
		COALESCE(HDRS_SKIPPED,0) HDRS_SKIPPED
	FROM PUSH_TO_BACK_STOCK_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
