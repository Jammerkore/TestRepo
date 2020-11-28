--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row from the size curve generate info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GENERATE_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CURVE_GENERATE_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
