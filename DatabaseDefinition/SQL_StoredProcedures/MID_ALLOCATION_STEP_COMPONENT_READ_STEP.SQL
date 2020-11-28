--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Get workflow step components by workflow RID and Step Number.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ALLOCATION_STEP_COMPONENT_READ_STEP]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sc.WORKFLOW_RID,
		sc.STEP_NUMBER,
		sc.COMP_TYPE_ID,
		COALESCE(sc.COMP_CRIT_RID, - 1) COMP_CRIT_RID,
		COALESCE(cc.SIZE_CODE_RID, - 1) SIZE_CODE_RID,
		COALESCE(cc.COLOR_CODE_RID, - 1) COLOR_CODE_RID,
		COALESCE(cc.PACK_NAME, ' ') PACK_NAME
	FROM ALLOCATION_STEP_COMPONENT sc
	LEFT OUTER JOIN COMPONENT_CRITERIA cc ON sc.COMP_CRIT_RID = cc.COMP_CRIT_RID
	WHERE WORKFLOW_RID = @WORKFLOW_RID
		AND STEP_NUMBER = @STEP_NUMBER

END
GO





















