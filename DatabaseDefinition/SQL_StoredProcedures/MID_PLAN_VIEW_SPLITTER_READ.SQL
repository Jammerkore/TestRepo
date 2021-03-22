--dv =============================================
--dv Create date:	3/19/2021
--dv Description:	Reads plan view splitter for the view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_SPLITTER_READ]
	@VIEW_RID int,
	@USER_RID int,
	@PLAN_SESSION_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [VIEW_RID],
	[USER_RID],
	[PLAN_SESSION_TYPE],
	[SPLITTER_TYPE_IND],
	[SPLITTER_SEQUENCE],
	[SPLITTER_PERCENTAGE]
	FROM [dbo].[PLAN_VIEW_SPLITTER]
	WHERE [VIEW_RID] = @VIEW_RID 
	  AND [USER_RID] = @USER_RID
	  AND [PLAN_SESSION_TYPE] = @PLAN_SESSION_TYPE
	ORDER BY [SPLITTER_TYPE_IND], [SPLITTER_SEQUENCE]

END
GO