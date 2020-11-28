--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the sales eligibility entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_ELIGIBILITY_ENTRY_DELETE]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SALES_ELIGIBILITY_ENTRY WITH (ROWLOCK)
	WHERE EM_RID = @EM_RID

	SELECT @@ROWCOUNT;

END
GO
