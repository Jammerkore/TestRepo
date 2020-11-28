--dv =============================================
--dv Create date: 12/16/2013
--dv Description: Update ID and User of Store Group.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_UPDATE_ID_AND_USER]
	@SG_RID INT,
	@SG_ID VARCHAR(50),
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE STORE_GROUP
	SET SG_ID = @SG_ID,
		USER_RID = @USER_RID
	WHERE SG_RID = @SG_RID

	SELECT @@ROWCOUNT
END
GO


