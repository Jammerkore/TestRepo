--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads a folder for the given folder rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_READ_FROM_RID]
	@FOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM FOLDER
	WHERE FOLDER_RID = @FOLDER_RID

END
GO