--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes a row from the priority shipping entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRIORITY_SHIPPING_ENTRY_DELETE]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PRIORITY_SHIPPING_ENTRY WITH (ROWLOCK)
	WHERE EM_RID = @EM_RID

	SELECT @@ROWCOUNT;

END
GO
