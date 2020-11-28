--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads from the product hierarchy levels header purge table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ]
	@PH_RID int,
	@PHL_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE 
    WHERE PH_RID = @PH_RID
    AND PHL_SEQUENCE = @PHL_SEQUENCE

END
GO
