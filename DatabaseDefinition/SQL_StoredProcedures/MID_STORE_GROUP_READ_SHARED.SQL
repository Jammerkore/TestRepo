--dv =============================================
--dv Create date: 12/16/2013
--dv Description:	Get shared store group data.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_SHARED]
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @StoreGroup AS INT = 38;

	SELECT sg.SG_RID,
		sg.SG_ID,
		sg.SG_DYNAMIC_GROUP_IND,
		ui.USER_RID,
		ui.OWNER_USER_RID
	FROM STORE_GROUP sg
	INNER JOIN USER_ITEM ui ON sg.SG_RID = ui.ITEM_RID
	WHERE ui.USER_RID = @USER_RID
		AND ui.ITEM_TYPE = @StoreGroup
		AND sg.IS_ACTIVE=1
	ORDER BY sg.SG_RID ASC

END
GO


