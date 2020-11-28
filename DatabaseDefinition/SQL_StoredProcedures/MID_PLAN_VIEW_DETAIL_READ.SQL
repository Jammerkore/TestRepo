--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads plan view details for the view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_DETAIL_READ]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PLAN_VIEW_DETAIL 
	WHERE VIEW_RID = @VIEW_RID
	ORDER BY VIEW_RID, AXIS, AXIS_SEQUENCE

END
GO