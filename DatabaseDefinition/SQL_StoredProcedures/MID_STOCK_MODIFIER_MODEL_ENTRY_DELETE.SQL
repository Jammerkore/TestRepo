--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes a row from the stock modifier model entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE]
	@STKMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STOCK_MODIFIER_MODEL_ENTRY WITH (ROWLOCK) 
	WHERE STKMOD_RID = @STKMOD_RID

	SELECT @@ROWCOUNT;

END
GO
