--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Updates a tasklist name and user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASKLIST_UPDATE_NAME_AND_USER]
	@TASKLIST_RID int,
	@TASKLIST_NAME varchar(50),
	@USER_RID int,
	@LAST_MODIFIED_BY_USER_RID int,
	@LAST_MODIFIED_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE TASKLIST 
	SET 
		TASKLIST_NAME = @TASKLIST_NAME, 
		USER_RID = @USER_RID, 
		LAST_MODIFIED_BY_USER_RID = @LAST_MODIFIED_BY_USER_RID, 
		LAST_MODIFIED_DATETIME = @LAST_MODIFIED_DATETIME
    WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO