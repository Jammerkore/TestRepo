--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads entries for a computation model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_ENTRY_READ]
	@COMP_MODEL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COMP_MODEL_RID, 
		COMP_MODEL_SEQUENCE, 
		COMP_TYPE, 
		FV_RID,
		CHANGE_VARIABLE, 
		PRODUCT_LEVEL 
	FROM COMPUTATION_MODEL_ENTRY
	WHERE COMP_MODEL_RID = @COMP_MODEL_RID

END
GO

