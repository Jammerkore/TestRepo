--dv =============================================
--dv Create date:	12/5/2014
--dv Description:	Deletes a row from the size curve load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_LOAD_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CURVE_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
