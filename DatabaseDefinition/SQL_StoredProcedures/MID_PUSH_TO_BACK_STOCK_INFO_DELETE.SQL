--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes rows from the push to back stock info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PUSH_TO_BACK_STOCK_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PUSH_TO_BACK_STOCK_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
