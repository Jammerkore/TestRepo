--dv =============================================
--dv Create date:	10/16/2014
--dv Description:	Reads the rid of the grid view filters for the given view
--dv History:		10/16/2014 TT#1313-MD -jsobek -Header Filters
--dv				01/08/2015 TT#1477-MD -jsobek -Header Filter Sort on Workspace
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(WORKSPACE_FILTER_RID, -1) AS WORKSPACE_FILTER_RID,
		USE_FILTER_SORTING
	FROM GRID_VIEW
	WHERE VIEW_RID = @VIEW_RID

END
GO	