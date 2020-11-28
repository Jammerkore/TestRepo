--dv =============================================
--dv Create date: 5/1/2015
--dv Description: Sets a store group inactive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_UPDATE_SET_INACTIVE] 
	@SG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_PLAN
	SET SG_RID = NULL
	WHERE SG_RID = @SG_RID

	-- Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
	UPDATE USER_ALLOCATION
	SET SG_RID = NULL
	WHERE SG_RID = @SG_RID
	-- End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

	UPDATE STORE_GROUP
	SET IS_ACTIVE = 0
	WHERE SG_RID = @SG_RID

	SELECT @@ROWCOUNT
END
GO


