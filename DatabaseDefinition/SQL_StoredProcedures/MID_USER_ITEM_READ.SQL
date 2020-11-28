--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads user items for the user and owner
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_READ]
	@USER_RID int,
	@OWNER_USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM USER_ITEM 
	WHERE USER_RID = @USER_RID
		  AND OWNER_USER_RID = @OWNER_USER_RID

END
GO