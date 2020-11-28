--dv =============================================
--dv Create date: 2/23/2015
--dv Description:	Get all stores profile data.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_FOR_MAINT]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ST_RID,
		COALESCE(ST_ID, '') AS 'Id',
		COALESCE(STORE_NAME, '') AS 'Name',
		COALESCE(STORE_DESC, '') AS 'Description',
		CASE WHEN ACTIVE_IND = '1' THEN 'Y' ELSE 'N' END AS 'Active',
		CASE WHEN STORE_DELETE_IND = '1' THEN 'Y' ELSE 'N' END AS 'Marked for Deletion'
	FROM STORES
	ORDER BY ST_ID

END
GO