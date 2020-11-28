--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Inserts a computation model entry
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_ENTRY_INSERT]
	@COMP_MODEL_RID int, 
	@COMP_MODEL_SEQUENCE int, 
	@COMP_TYPE int, 
	@FV_RID int, 
	@CHANGE_VARIABLE int, 
	@PRODUCT_LEVEL int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO COMPUTATION_MODEL_ENTRY
		(
		COMP_MODEL_RID, 
		COMP_MODEL_SEQUENCE, 
		COMP_TYPE, 
		FV_RID, 
		CHANGE_VARIABLE, 
		PRODUCT_LEVEL
		)
	VALUES
		(
		@COMP_MODEL_RID, 
		@COMP_MODEL_SEQUENCE, 
		@COMP_TYPE, 
		@FV_RID, 
		@CHANGE_VARIABLE, 
		@PRODUCT_LEVEL
		)

	SELECT @@ROWCOUNT;

END
GO
