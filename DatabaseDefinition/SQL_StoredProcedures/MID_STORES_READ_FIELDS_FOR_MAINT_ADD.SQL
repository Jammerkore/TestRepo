--dv =============================================
--dv Create date: 2/23/2015
--dv Description:	Get blank characteristics for a new store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_FIELDS_FOR_MAINT_ADD]
AS
BEGIN
	SET NOCOUNT ON;

	--Blank store characteristic values for the new store
	SELECT 
		scg.SCG_RID,
		scg.SCG_ID,
		scg.SCG_LIST_IND,
		scg.SCG_TYPE
	FROM STORE_CHAR_GROUP scg 
	ORDER BY scg.SCG_ID


END
GO