--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads non-processed computation items based on process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_READ]
	@PROCESS int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ci.PROCESS, 
		ci.CG_RID, 
		ci.HN_RID, 
		ci.ITEM_TYPE, 
		ci.FV_RID, 
		ci.FISCAL_YEAR_WEEK,
		cm.CALC_MODE
	FROM COMPUTATION_ITEM ci, 
		 COMPUTATION_GROUP cg, 
		 COMPUTATION_MODEL cm 
	WHERE ci.PROCESS = @PROCESS
		  AND ci.CG_RID  = cg.CG_RID 
		  AND cg.COMP_MODEL_RID = cm.COMP_MODEL_RID  
		  AND ci.ITEM_PROCESSED IS NULL  
	ORDER BY ci.CG_RID, ci.ITEM_TYPE, ci.HN_RID, ci.FV_RID, ci.FISCAL_YEAR_WEEK

END
GO
