--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a row from the process audit header table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_HDR_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PROC_HDR 
    WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
