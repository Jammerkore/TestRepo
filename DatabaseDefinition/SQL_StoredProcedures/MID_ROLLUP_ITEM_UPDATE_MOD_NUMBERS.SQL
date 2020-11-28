--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Updates rollup items mod numbers
--dv Modified date:	2/27/2017
--dv                TT#5485 - JSmith - Performance
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NumberOfTables INT = 10;

    SELECT @NumberOfTables=STORE_TABLE_COUNT FROM SYSTEM_OPTIONS

	UPDATE ROLLUP_ITEM 
	SET HN_MOD = HN_RID % @NumberOfTables 
	WHERE HN_MOD IS NULL

	SELECT @@ROWCOUNT;
END
GO