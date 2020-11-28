--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Inserts a folder join item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_JOIN_INSERT]
	@PARENT_FOLDER_RID int,
	@CHILD_ITEM_RID int, 
	@CHILD_ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO FOLDER_JOIN 
		(
		PARENT_FOLDER_RID, 
		CHILD_ITEM_RID, 
		CHILD_ITEM_TYPE
		)
	VALUES
		(
		@PARENT_FOLDER_RID, 
		@CHILD_ITEM_RID, 
		@CHILD_ITEM_TYPE
		)

	SELECT @@ROWCOUNT;

END
GO