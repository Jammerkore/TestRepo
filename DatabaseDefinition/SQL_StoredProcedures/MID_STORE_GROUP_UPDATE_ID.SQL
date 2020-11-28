--dv =============================================
--dv Create date: 12/16/2013
--dv Description: Update store group data by ID.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_UPDATE_ID]
	@SG_RID INT,
	@SG_ID VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE STORE_GROUP
	SET SG_ID = @SG_ID
	WHERE SG_RID = @SG_RID

	SELECT @@ROWCOUNT
END
GO


