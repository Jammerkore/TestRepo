--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a row from the special request info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SPECIAL_REQUEST_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
