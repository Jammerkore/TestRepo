--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads all computation models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COMP_MODEL_RID, 
		COMP_MODEL_ID, 
		CALC_MODE 
	FROM COMPUTATION_MODEL

END
GO
