--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a row on the product hierarchy levels and header purge tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_DELETE]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE WITH (ROWLOCK)
    WHERE PH_RID = @PH_RID

	DELETE FROM PRODUCT_HIERARCHY_LEVELS WITH (ROWLOCK) 
    WHERE PH_RID = @PH_RID

	SELECT @@ROWCOUNT;

END
GO
