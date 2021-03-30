--dv =============================================
--dv Create date:	10/29/2014
--dv Description:	Deletes a grid view
--dv History:		10/16/2014 TT#1313-MD -jsobek -Header Filters
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_DELETE] 
	@VIEW_RID INT
AS
BEGIN
	SET NOCOUNT ON

	DELETE USER_CURRENT_GRID_VIEW
	WITH (ROWLOCK)
	WHERE VIEW_RID = @VIEW_RID

	DELETE GRID_VIEW_DETAIL
	WITH (ROWLOCK)
	WHERE VIEW_RID = @VIEW_RID

	DELETE GRID_VIEW_SPLITTER
	WITH (ROWLOCK)
	WHERE VIEW_RID = @VIEW_RID

	DELETE GRID_VIEW
	WITH (ROWLOCK)
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;
END
GO


