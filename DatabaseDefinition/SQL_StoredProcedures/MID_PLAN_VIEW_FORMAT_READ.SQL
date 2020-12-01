--dv =============================================
--dv Create date:	11/17/2020
--dv Description:	Reads plan view format for the view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_FORMAT_READ]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT VIEW_RID AS [VIEW_RID],
	PLAN_BASIS_TYPE AS [PLAN_BASIS_TYPE],
	VARIABLE_NUMBER AS [VARIABLE_NUMBER],
	coalesce (QUANTITY_VARIABLE_KEY, -1) AS [QUANTITY_VARIABLE_KEY],
	coalesce (TIME_PERIOD_TYPE, -1) AS [TIME_PERIOD_TYPE],
	coalesce (TIME_PERIOD_KEY, -1) AS [TIME_PERIOD_KEY],
	coalesce (VARIABLE_TOTAL_KEY, -1) AS [VARIABLE_TOTAL_KEY],
	WIDTH AS [WIDTH] 
	FROM [dbo].[PLAN_VIEW_FORMAT]
	WHERE VIEW_RID = @VIEW_RID

END
GO