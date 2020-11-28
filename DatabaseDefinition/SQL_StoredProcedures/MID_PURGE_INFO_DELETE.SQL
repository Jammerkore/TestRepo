--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row from the purge info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PURGE_INFO 
	WHERE PROCESS_RID = @PROCESS_RID
		
	SELECT @@ROWCOUNT;
		
END
GO
