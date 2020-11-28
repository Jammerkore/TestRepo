--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Updates the my workflow methods on the user option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS]
	@USER_RID int,
	@MY_WORKFLOWMETHODS varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_OPTIONS 
	SET
		MY_WORKFLOWMETHODS = @MY_WORKFLOWMETHODS
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT
END
GO

