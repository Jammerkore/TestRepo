--dv =============================================
--dv Create date: 12/26/2013
--dv Description: Get all the store char group info and order by scg rid.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SCG_RID,
		SCG_ID,
		SCG_TYPE,
		SCG_LIST_IND
	FROM STORE_CHAR_GROUP
	ORDER BY SCG_ID

END
GO
