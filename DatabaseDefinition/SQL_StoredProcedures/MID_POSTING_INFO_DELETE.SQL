--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a row from the posting info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_POSTING_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM POSTING_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
