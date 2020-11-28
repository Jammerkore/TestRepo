--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Updates a folder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_UPDATE]
	@FOLDER_RID int,
	@USER_RID int,
	@FOLDER_ID varchar(100),
	@FOLDER_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FOLDER 
	SET 
		USER_RID = @USER_RID, 
		FOLDER_ID = @FOLDER_ID, 
		FOLDER_TYPE = @FOLDER_TYPE 
	WHERE FOLDER_RID = @FOLDER_RID

	SELECT @@ROWCOUNT;

END
GO
