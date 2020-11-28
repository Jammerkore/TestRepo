--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads a folder join item for the given parent rid, child rid and child type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_JOIN_READ]
	@PARENT_FOLDER_RID int,
	@CHILD_ITEM_RID int,
	@CHILD_ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
    FROM FOLDER_JOIN fj
    WHERE fj.PARENT_FOLDER_RID = @PARENT_FOLDER_RID
    AND fj.CHILD_ITEM_RID = @CHILD_ITEM_RID
    AND fj.CHILD_ITEM_TYPE = @CHILD_ITEM_TYPE

END
GO