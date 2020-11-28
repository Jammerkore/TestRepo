--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads from the product hierarchy levels table based on the product hierarchy rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_READ]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PRODUCT_HIERARCHY_LEVELS
	WHERE PH_RID = @PH_RID
	ORDER BY PHL_SEQUENCE 

END
GO
