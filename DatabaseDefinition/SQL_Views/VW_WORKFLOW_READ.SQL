--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from a workflow.
--dv =============================================
CREATE VIEW [dbo].[VW_WORKFLOW_READ]
AS
	
	SELECT 
		w.WORKFLOW_RID,
		w.WORKFLOW_NAME,
		w.WORKFLOW_TYPE_ID,
		w.WORKFLOW_DESCRIPTION,
		COALESCE(w.STORE_FILTER_RID, - 1) STORE_FILTER_RID,
		w.WORKFLOW_USER_RID AS WORKFLOW_USER,
		w.WORKFLOW_OVERRIDE,
		ui.USER_RID AS WORKFLOW_USER_RID,
		ui.ITEM_TYPE,
		ui.OWNER_USER_RID
	FROM WORKFLOW w
	INNER JOIN USER_ITEM ui WITH (NOLOCK) ON ui.ITEM_RID = w.WORKFLOW_RID

GO
