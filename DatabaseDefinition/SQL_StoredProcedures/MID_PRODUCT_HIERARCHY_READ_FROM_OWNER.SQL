--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads product hierarchy rows based on the owner user rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_READ_FROM_OWNER] 
	@PH_OWNER int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PRODUCT_HIERARCHY
    WHERE PH_OWNER = @PH_OWNER

END
GO
