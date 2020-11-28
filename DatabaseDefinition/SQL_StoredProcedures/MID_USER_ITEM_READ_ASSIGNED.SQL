--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads assigned to users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_READ_ASSIGNED]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		USER_RID 
	FROM USER_ITEM 
	WHERE OWNER_USER_RID = @USER_RID
		  AND USER_RID != @USER_RID

END
GO