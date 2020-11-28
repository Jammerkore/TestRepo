--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Inserts a OTS plan workflow step
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_STEP_OTSPLAN_INSERT]
		@WORKFLOW_RID int, 
		@STEP_NUMBER int, 
		@ACTION_METHOD_TYPE int,
        @METHOD_RID int, 
		@REVIEW_IND char, 
		@PCT_TOLERANCE float, 
		@STORE_FILTER_RID int, 
		@VARIABLE_NUMBER int, 
		@CALC_MODE varchar(50), 
		@BAL_IND char
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO WORKFLOW_STEP_OTSPLAN
		(
		WORKFLOW_RID, 
		STEP_NUMBER, 
		ACTION_METHOD_TYPE,
        METHOD_RID, 
		REVIEW_IND, 
		PCT_TOLERANCE, 
		STORE_FILTER_RID, 
		VARIABLE_NUMBER, 
		CALC_MODE, 
		BAL_IND
		)
    VALUES 
		(
		@WORKFLOW_RID, 
		@STEP_NUMBER, 
		@ACTION_METHOD_TYPE,
        @METHOD_RID, 
		@REVIEW_IND, 
		@PCT_TOLERANCE, 
		@STORE_FILTER_RID, 
		@VARIABLE_NUMBER, 
		@CALC_MODE, 
		@BAL_IND
		)

	SELECT @@ROWCOUNT;
END
GO
