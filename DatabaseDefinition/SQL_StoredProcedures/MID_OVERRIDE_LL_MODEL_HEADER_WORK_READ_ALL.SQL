--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads all records Header Work Override LL Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL]
	@OLL_RID int,
	@ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ollmh.*,
		COALESCE(ui.USER_RID, 5) as 'USER_RID',
		COALESCE(ui.OWNER_USER_RID, 5) as 'OWNER_USER_RID'
    FROM OVERRIDE_LL_MODEL_HEADER_WORK ollmh
		LEFT OUTER JOIN USER_ITEM ui ON ollmh.OLL_RID = ui.ITEM_RID AND ui.ITEM_TYPE = @ITEM_TYPE
    WHERE ollmh.OLL_RID = @OLL_RID
	ORDER BY ollmh.NAME

END
GO









