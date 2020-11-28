--dv =============================================
--dv Create date: 12/16/2013
--dv Description: Get all stores set for deletion.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_FOR_DELETION]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ST_RID,
		COALESCE(ST_ID, '') ST_ID,
		ACTIVE_IND,
		STORE_DELETE_IND
	FROM STORES
	WHERE STORE_DELETE_IND = '1'
	ORDER BY ST_RID

END
GO