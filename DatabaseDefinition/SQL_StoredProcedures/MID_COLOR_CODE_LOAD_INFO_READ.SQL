--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads a row from the color code load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM COLOR_CODE_LOAD_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
