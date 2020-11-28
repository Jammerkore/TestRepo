--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads all records Header Work Override LL Model for authorized users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL]
	@OLL_RID int,
	@customOLL_rid int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ollmh.*,
		COALESCE(ui.OWNER_USER_RID, 5) as 'OWNER_USER_RID'
    FROM OVERRIDE_LL_MODEL_HEADER_WORK ollmh
		LEFT OUTER JOIN USER_ITEM ui ON ollmh.OLL_RID = ui.ITEM_RID AND ui.ITEM_TYPE = @OLL_RID
    WHERE ollmh.OLL_RID = @OLL_RID
		OR ui.USER_RID = 4
		OR ollmh.OLL_RID = @customOLL_rid
	ORDER BY ollmh.NAME

END
GO





