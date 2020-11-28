--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the stock eligibility entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_ELIGIBILITY_ENTRY_DELETE]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STOCK_ELIGIBILITY_ENTRY WITH (ROWLOCK) 
	WHERE EM_RID = @EM_RID

	SELECT @@ROWCOUNT;

END
GO
