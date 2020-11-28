--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads users assigned to me
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_READ_ASSIGNED_TO_ME]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		OWNER_USER_RID 
	FROM USER_ITEM 
	where USER_RID = @USER_RID
		  AND USER_RID != OWNER_USER_RID

END
GO