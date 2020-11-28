--dv =============================================
--dv Create date: 12/16/2013
--dv Description: Get all store removal analysis data.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_REMOVAL_ANALYSIS_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT sra.*,
		srl.TABLE_NAME
	FROM STORE_REMOVAL_ANALYSIS sra
	INNER JOIN STORE_REMOVAL_TABLE_LIST srl ON sra.TABLE_LIST_ID = srl.TABLE_LIST_ID
	ORDER BY srl.DELETE_SEQUENCE

END
GO