--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes a sales modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_DELETE]
	@SLSMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SALES_MODIFIER_MODEL_ENTRY WITH (ROWLOCK)
	WHERE SLSMOD_RID = @SLSMOD_RID

    DELETE FROM SALES_MODIFIER_MODEL WITH (ROWLOCK)
	WHERE SLSMOD_RID = @SLSMOD_RID

	SELECT @@ROWCOUNT;

END
GO
