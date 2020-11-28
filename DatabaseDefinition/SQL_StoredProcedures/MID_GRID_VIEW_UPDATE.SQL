--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Updates the grid view
--dv History:		10/16/2014 TT#1313-MD -jsobek -Header Filters
--dv				01/08/2015 TT#1477-MD -jsobek -Header Filter Sort on Workspace
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_UPDATE] 
	@VIEW_RID int,
	@SHOW_DETAILS char(1),
	@GROUP_BY int,
	@GROUP_BY_SECONDARY int,
	@IS_SEQUENTIAL char(1),
	@WORKSPACE_FILTER_RID int,
	@USE_FILTER_SORTING bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE GRID_VIEW 
	SET
		SHOW_DETAILS = @SHOW_DETAILS,
		GROUP_BY = @GROUP_BY,
		GROUP_BY_SECONDARY = @GROUP_BY_SECONDARY,
		IS_SEQUENTIAL = @IS_SEQUENTIAL,
		WORKSPACE_FILTER_RID = @WORKSPACE_FILTER_RID,
		USE_FILTER_SORTING = @USE_FILTER_SORTING
    WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;

END
GO