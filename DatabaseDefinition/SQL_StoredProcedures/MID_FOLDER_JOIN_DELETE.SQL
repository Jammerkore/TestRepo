--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Deletes a folder join item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_JOIN_DELETE]
	@CHILD_ITEM_TYPE int,
	@CHILD_ITEM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FOLDER_JOIN 
	WHERE CHILD_ITEM_TYPE = @CHILD_ITEM_TYPE
		  AND CHILD_ITEM_RID = @CHILD_ITEM_RID

	SELECT @@ROWCOUNT;

END
GO