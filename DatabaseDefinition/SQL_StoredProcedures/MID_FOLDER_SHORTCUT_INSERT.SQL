--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Inserts a folder shortcut
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_INSERT]
	@PARENT_FOLDER_RID int,
	@CHILD_SHORTCUT_RID int,
	@CHILD_SHORTCUT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO FOLDER_SHORTCUT 
		(
		PARENT_FOLDER_RID, 
		CHILD_SHORTCUT_RID, 
		CHILD_SHORTCUT_TYPE
		)
	VALUES
		(
		@PARENT_FOLDER_RID, 
		@CHILD_SHORTCUT_RID, 
		@CHILD_SHORTCUT_TYPE
		)
	
	SELECT @@ROWCOUNT;

END
GO