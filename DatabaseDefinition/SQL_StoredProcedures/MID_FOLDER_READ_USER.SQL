--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads the user for the given folder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_READ_USER]
	@FOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_RID 
	FROM FOLDER 
	WHERE FOLDER_RID = @FOLDER_RID

END
GO
