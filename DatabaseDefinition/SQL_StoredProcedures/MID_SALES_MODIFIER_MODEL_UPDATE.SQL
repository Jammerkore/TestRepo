--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Updates a sales modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_UPDATE]
	@SLSMOD_RID int,
	@SLSMOD_ID varchar(50),
	@SLSMOD_DEFAULT float
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SALES_MODIFIER_MODEL WITH (ROWLOCK)
	SET 
		SLSMOD_ID = @SLSMOD_ID,
		SLSMOD_DEFAULT = @SLSMOD_DEFAULT
	WHERE SLSMOD_RID = @SLSMOD_RID

	SELECT @@ROWCOUNT;

END
GO
