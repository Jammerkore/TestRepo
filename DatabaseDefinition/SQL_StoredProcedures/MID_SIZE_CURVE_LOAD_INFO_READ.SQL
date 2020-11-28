--dv =============================================
--dv Create date:	12/5/2014
--dv Description:	Reads a row from the size curve load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_CURVE_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
