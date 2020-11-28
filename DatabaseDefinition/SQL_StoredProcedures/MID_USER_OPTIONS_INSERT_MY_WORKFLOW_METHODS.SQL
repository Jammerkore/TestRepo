--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Inserts a new row into the user option table, filling in my workflow methods
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS]
	@USER_RID int,
	@MY_WORKFLOWMETHODS varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_OPTIONS
		(
		USER_RID, 
		MY_WORKFLOWMETHODS
		) 
	VALUES 
		(
		@USER_RID, 
		@MY_WORKFLOWMETHODS
		)

	SELECT @@ROWCOUNT
END
GO
