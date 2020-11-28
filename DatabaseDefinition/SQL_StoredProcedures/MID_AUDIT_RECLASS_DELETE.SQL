--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes rows from the audit reclass table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_RECLASS_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM AUDIT_RECLASS 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
