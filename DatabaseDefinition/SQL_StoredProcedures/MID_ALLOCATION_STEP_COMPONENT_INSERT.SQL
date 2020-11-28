--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Insert values for allocation step component.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ALLOCATION_STEP_COMPONENT_INSERT]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT,
	@COMP_TYPE_ID INT,
	@COMP_CRIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO ALLOCATION_STEP_COMPONENT 
		(
		WORKFLOW_RID,
		STEP_NUMBER,
		COMP_TYPE_ID,
		COMP_CRIT_RID
		)
	VALUES 
		(
		@WORKFLOW_RID,
		@STEP_NUMBER,
		@COMP_TYPE_ID,
		@COMP_CRIT_RID
		)

	SELECT @@ROWCOUNT

END
GO




















