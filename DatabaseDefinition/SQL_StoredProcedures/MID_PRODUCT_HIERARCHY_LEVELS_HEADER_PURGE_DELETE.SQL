--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Deletes a row on the product hierarchy header purge table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE]
	@PH_RID int,
	@PHL_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE WITH (ROWLOCK) 
    WHERE PH_RID = @PH_RID
		  AND PHL_SEQUENCE = @PHL_SEQUENCE
	
	SELECT @@ROWCOUNT;

END
GO
