--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Deletes a folder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_DELETE]
	@FOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FOLDER 
	WHERE FOLDER_RID = @FOLDER_RID

	SELECT @@ROWCOUNT;

END
GO
