--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes a user item based on item type and item rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM]
	@ITEM_TYPE int, 
	@ITEM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_ITEM 
	WHERE ITEM_TYPE = @ITEM_TYPE 
		  AND ITEM_RID = @ITEM_RID

	SELECT @@ROWCOUNT;

END
GO