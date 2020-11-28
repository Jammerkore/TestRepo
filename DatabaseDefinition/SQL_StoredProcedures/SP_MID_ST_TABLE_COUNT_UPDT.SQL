--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure to set the STORE_TABLE_COUNT column on the SYSTEM_OPTIONS table.
--dv				The record id, SYSTEM_OPTION_RID, must be customized by installation 
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_ST_TABLE_COUNT_UPDT] 
	@NEW_TABLE_COUNT INT
AS
BEGIN

	UPDATE SYSTEM_OPTIONS
	SET STORE_TABLE_COUNT = @NEW_TABLE_COUNT
	WHERE SYSTEM_OPTION_RID = 1

END
GO


