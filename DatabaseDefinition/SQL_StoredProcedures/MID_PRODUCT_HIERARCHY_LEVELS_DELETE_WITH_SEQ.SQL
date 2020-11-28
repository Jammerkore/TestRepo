--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a row on the product hierarchy levels and header purge tables based on product hierarchy rid and product hierarchy level sequence
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ]
	@PH_RID int,
	@PHL_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE WITH (ROWLOCK) 
    WHERE PH_RID = @PH_RID
		  AND PHL_SEQUENCE = @PHL_SEQUENCE

	DELETE FROM PRODUCT_HIERARCHY_LEVELS WITH (ROWLOCK) 
    WHERE PH_RID = @PH_RID
		  AND PHL_SEQUENCE = @PHL_SEQUENCE

	SELECT @@ROWCOUNT;

END
GO
