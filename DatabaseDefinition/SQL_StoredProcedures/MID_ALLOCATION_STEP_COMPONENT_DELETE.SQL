--dv =============================================
--dv Create date: 12/11/2013
--dv Description:	Deletes an allocation step component
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ALLOCATION_STEP_COMPONENT_DELETE]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ALLOCATION_STEP_COMPONENT
	WHERE WORKFLOW_RID = @WORKFLOW_RID
	AND STEP_NUMBER = @STEP_NUMBER

	SELECT @@ROWCOUNT

END
GO




















