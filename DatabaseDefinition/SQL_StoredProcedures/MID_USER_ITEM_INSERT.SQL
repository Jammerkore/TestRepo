--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Inserts a user item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ITEM_INSERT]
	@USER_RID int,
	@ITEM_TYPE int, 
	@ITEM_RID int,
	@OWNER_USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_ITEM 
		(
		USER_RID, 
		ITEM_TYPE, 
		ITEM_RID, 
		OWNER_USER_RID
		)
	VALUES
		(
		@USER_RID, 
		@ITEM_TYPE, 
		@ITEM_RID, 
		@OWNER_USER_RID
		)

	SELECT @@ROWCOUNT;

END
GO