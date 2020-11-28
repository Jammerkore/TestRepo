--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes a user item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_DELETE]
	@USER_RID int,
	@ITEM_TYPE int, 
	@ITEM_RID int,
	@OWNER_USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_ITEM 
	WHERE USER_RID = @USER_RID
		  AND ITEM_TYPE = @ITEM_TYPE 
		  AND ITEM_RID = @ITEM_RID 
		  AND OWNER_USER_RID = @OWNER_USER_RID

	SELECT @@ROWCOUNT;

END
GO