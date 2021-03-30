--dv =============================================
--dv Create date:	3/26/2021
--dv Description:	Reads grid view splitter for the view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_SPLITTER_READ]
	@VIEW_RID int,
	@USER_RID int,
	@LAYOUT_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [VIEW_RID],
	[USER_RID],
	[LAYOUT_ID],
	[SPLITTER_TYPE_IND],
	[SPLITTER_SEQUENCE],
	[SPLITTER_PERCENTAGE]
	FROM [dbo].[GRID_VIEW_SPLITTER]
	WHERE [VIEW_RID] = @VIEW_RID 
	  AND [USER_RID] = @USER_RID
	  AND [LAYOUT_ID] = @LAYOUT_ID
	ORDER BY [SPLITTER_TYPE_IND], [SPLITTER_SEQUENCE]

END
GO