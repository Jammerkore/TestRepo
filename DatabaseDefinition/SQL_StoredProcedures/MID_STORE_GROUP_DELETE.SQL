--dv =============================================
--dv Create date:	4/3/2017
--dv Description:	Deletes store groups
--dv TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_DELETE]
	@SG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_PLAN
	SET SG_RID = NULL
	WHERE SG_RID = @SG_RID

	UPDATE USER_ALLOCATION
	SET SG_RID = NULL
	WHERE SG_RID = @SG_RID

	DELETE FROM STORE_GROUP_JOIN_HISTORY WITH (ROWLOCK) WHERE SG_RID=@SG_RID
	DELETE FROM STORE_GROUP_LEVEL_RESULTS_HISTORY WITH (ROWLOCK) WHERE SGL_RID IN (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WHERE sgl.SG_RID=@SG_RID)

	-- Delete  store groups and levels

	--Delete user items for inactive store groups
	DELETE 
	FROM USER_ITEM WITH (ROWLOCK)
	WHERE ITEM_TYPE = 38 --Store Group 
		  AND ITEM_RID = @SG_RID

	--Delete levels for inactive store groups
	DELETE FROM STORE_GROUP_LEVEL_RESULTS WITH (ROWLOCK)
	WHERE STORE_GROUP_LEVEL_RESULTS.SGL_RID IN (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WHERE sgl.SG_RID = @SG_RID)

	--Delete join entries for inactive store groups
	DELETE FROM STORE_GROUP_JOIN WITH (ROWLOCK)
	WHERE STORE_GROUP_JOIN.SG_RID = @SG_RID

	DELETE FROM STORE_GROUP_LEVEL WITH (ROWLOCK) WHERE SG_RID = @SG_RID

	--Delete the filters for inactive store groups
	DELETE FROM FILTER WITH (ROWLOCK)
	WHERE FILTER_RID IN 
	(
		SELECT FILTER_RID FROM STORE_GROUP WITH (ROWLOCK)
		WHERE SG_RID = @SG_RID
	)

	--Delete inactive store groups
	DELETE FROM STORE_GROUP WITH (ROWLOCK)
	WHERE SG_RID = @SG_RID

	SELECT @@ROWCOUNT;
END
GO
