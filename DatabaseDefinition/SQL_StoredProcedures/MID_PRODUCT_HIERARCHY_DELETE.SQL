--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes from the product hierarchy, hierarchy join, mid enqueue and user item tables for the specified PH_RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_DELETE]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ROWS_DELETED int;

	DECLARE @PROFILE_HIERARCHY int
	SET @PROFILE_HIERARCHY = 26

	DELETE FROM HIER_NODE_JOIN WITH (ROWLOCK) 
	WHERE PH_RID = @PH_RID

	DELETE FROM MID_ENQUEUE WITH (ROWLOCK) 
	WHERE PH_RID = @PH_RID

	DELETE FROM PRODUCT_HIERARCHY WITH (ROWLOCK) 
	WHERE PH_RID = @PH_RID

	SET @ROWS_DELETED=@@ROWCOUNT;

	DELETE FROM USER_ITEM 
	WHERE ITEM_TYPE = @PROFILE_HIERARCHY 
		  AND ITEM_RID = @PH_RID

	SELECT @ROWS_DELETED;

END
GO
