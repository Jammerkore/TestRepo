--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads a computation model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_READ]
	@COMP_MODEL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COMP_MODEL_RID, 
		COMP_MODEL_ID, 
		CALC_MODE 
	FROM COMPUTATION_MODEL
	WHERE COMP_MODEL_RID = @COMP_MODEL_RID

END
GO