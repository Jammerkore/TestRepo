--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads from the special request info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SPECIAL_REQUEST_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
