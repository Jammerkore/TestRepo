--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads a computation model from the id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_READ_FROM_ID]
	@COMP_MODEL_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COMP_MODEL_RID, 
		COMP_MODEL_ID, 
		CALC_MODE 
	FROM COMPUTATION_MODEL
	WHERE COMP_MODEL_ID = @COMP_MODEL_ID

END
GO
