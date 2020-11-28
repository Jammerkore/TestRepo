--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads plan views for the user list provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_READ_FROM_USER_LIST]
	@USER_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_PlanView INT = 58;
	
	SELECT 
		pv.VIEW_RID, 
		ui.USER_RID, 
		pv.VIEW_ID, 
		pv.GROUPBY_TYPE,
		ui.ITEM_TYPE, 
		ui.OWNER_USER_RID
	FROM PLAN_VIEW pv
		 INNER JOIN USER_ITEM ui ON ui.ITEM_RID = pv.VIEW_RID
	WHERE ui.USER_RID IN (SELECT USER_RID FROM @USER_LIST)
		  AND ui.ITEM_TYPE = @eProfileType_PlanView
	ORDER BY VIEW_ID

END
GO