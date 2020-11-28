--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Renames a folder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_UPDATE_ID]
	@FOLDER_RID int,
	@FOLDER_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FOLDER WITH (ROWLOCK)
	SET 
		FOLDER_ID = @FOLDER_ID 
	WHERE FOLDER_RID = @FOLDER_RID

	SELECT @@ROWCOUNT;

END
GO

