--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads all computation groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		cg.CG_RID, 
		cg.PROCESS, 
		cg.COMP_MODEL_RID
	FROM COMPUTATION_GROUP cg

END
GO
