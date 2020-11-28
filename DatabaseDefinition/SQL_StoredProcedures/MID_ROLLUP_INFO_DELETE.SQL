--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row from the rollup info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ROLLUP_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
