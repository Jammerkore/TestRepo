--dv =============================================
--dv Create date:	11/17/2020
--dv Description:	Reads plan view format for the view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_FORMAT_READ]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PLAN_VIEW_FORMAT 
	WHERE VIEW_RID = @VIEW_RID

END
GO