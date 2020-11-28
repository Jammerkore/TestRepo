--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes rows for chain plan set percentages based on the hierarchy node and fiscal week
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_SET_DELETE]
	@HN_RID int,
	@FISCAL_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_SET_PERCENT_SET 
    WHERE HN_RID = @HN_RID
          AND TIME_ID = (SELECT FIRST_DAY_OF_WEEK FROM FISCAL_WEEKS WHERE FISCAL_WEEK = @FISCAL_WEEK)

	SELECT @@ROWCOUNT;

END
GO
