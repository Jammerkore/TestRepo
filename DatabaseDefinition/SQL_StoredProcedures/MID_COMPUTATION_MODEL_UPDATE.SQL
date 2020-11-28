--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Updates a computation model row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_MODEL_UPDATE]
	@COMP_MODEL_RID int,
	@COMP_MODEL_ID varchar(50),
	@CALC_MODE varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE COMPUTATION_MODEL
	SET
		COMP_MODEL_ID = @COMP_MODEL_ID,
	    CALC_MODE = @CALC_MODE
	WHERE COMP_MODEL_RID = @COMP_MODEL_RID

	SELECT @@ROWCOUNT;

END
GO
