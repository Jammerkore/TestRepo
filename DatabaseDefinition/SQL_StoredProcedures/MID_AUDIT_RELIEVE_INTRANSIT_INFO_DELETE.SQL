--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes rows from the audit relieve intransit info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM AUDIT_RELIEVE_INTRANSIT_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
