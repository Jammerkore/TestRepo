--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads from the store load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STR_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM STR_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
