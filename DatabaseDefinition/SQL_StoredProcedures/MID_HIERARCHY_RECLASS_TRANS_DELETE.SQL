--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes hierarchy reclass transactions based on the transaction process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_RECLASS_TRANS_DELETE] 
	@TRANS_PROCESS_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIERARCHY_RECLASS_TRANS 
	WHERE TRANS_PROCESS_ID = @TRANS_PROCESS_ID
	
	SELECT @@ROWCOUNT;

END
GO