--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads assigned and owned folders
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_READ_ASSIGNED_AND_OWNED]
	@FOLDER_TYPE int,
	@ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		f.FOLDER_RID, 
		ui.USER_RID, 
		f.FOLDER_ID, 
		f.FOLDER_TYPE,
		ui.ITEM_TYPE, 
		ui.OWNER_USER_RID
	FROM FOLDER f
    INNER JOIN USER_ITEM ui ON ui.ITEM_RID = f.FOLDER_RID AND ui.ITEM_TYPE = f.FOLDER_TYPE
	WHERE FOLDER_TYPE = @FOLDER_TYPE
		  AND ui.ITEM_TYPE = @ITEM_TYPE
	ORDER BY FOLDER_ID

END
GO