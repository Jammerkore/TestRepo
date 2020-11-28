--dv =============================================
--dv Create date:	01/30/2014
--dv Description:	Returns all Header Override LL Model
--dv				where only user views are allowed 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW]
	@OLL_RID int,
	@USER_RID int,
	@customOLL_rid int,
	@ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ollmh.*,
		COALESCE(ui.USER_RID, 5) as 'USER_RID',
		COALESCE(ui.OWNER_USER_RID, 5) AS 'OWNER_USER_RID'
    FROM OVERRIDE_LL_MODEL_HEADER ollmh
		LEFT OUTER JOIN USER_ITEM ui ON ollmh.OLL_RID = ui.ITEM_RID 
		AND ui.ITEM_TYPE = @ITEM_TYPE
    WHERE ollmh.OLL_RID = @OLL_RID
		OR ui.USER_RID = @USER_RID
		OR ollmh.OLL_RID = @customOLL_rid
	ORDER BY ollmh.NAME

END
GO