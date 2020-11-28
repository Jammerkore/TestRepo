--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Deletes a workflow, and deletes data from all its child tables
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_WORKFLOW_DELETE] 
	@WorkflowRID AS INT,
	@debug BIT = 0
AS
BEGIN
	IF (@debug = 0)
		SET NOCOUNT ON

	DELETE
	FROM [dbo].[WORKFLOW_STEP_OTSPLAN] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[TASK_ALLOCATE_DETAIL] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[TASK_FORECAST_DETAIL] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[WORKFLOW_CHANGE_HISTORY] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[USER_WORKFLOW_PREF] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[WORKFLOW_HISTORY] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[ALLOCATION_STEP_COMPONENT] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	DELETE
	FROM [dbo].[WORKFLOW_STEP_ALLOCATION] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID

	/* Begin TT#1873 - JSmith - Purge Failed with Severe Error */
	--update [dbo].[HEADER] with (rowlock) set WORKFLOW_RID = null where WORKFLOW_RID = @WorkflowRID
	UPDATE [dbo].[HEADER]
	WITH (ROWLOCK)

	SET WORKFLOW_RID = 1
	WHERE WORKFLOW_RID = @WorkflowRID

	/* End TT#1873 */
	DELETE
	FROM [dbo].[WORKFLOW] WITH (ROWLOCK)
	WHERE WORKFLOW_RID = @WorkflowRID
END
GO


