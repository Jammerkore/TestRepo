--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads the modify sales method sell thrus
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_SELL_THRU_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM METHOD_MOD_SALES_SELL_THRU
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SELL_THRU DESC

END
GO

