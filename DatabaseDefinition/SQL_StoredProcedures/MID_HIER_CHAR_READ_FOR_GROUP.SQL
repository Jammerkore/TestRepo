--dv =============================================
--dv Create date: 11/18/2014
--dv Description: Reads all possible product characteristic values from a product characteristic group
--dv Example:  Product characteristic group=Fabric
--dv		   Product characteristic values=Wool, Denim
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_READ_FOR_GROUP]
	@HCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HC.HC_ID AS CHAR_VALUE,
		HC.HC_RID 
	FROM HIER_CHAR HC WITH (NOLOCK)
	WHERE HC.HCG_RID=@HCG_RID
	ORDER BY CHAR_VALUE

END

GO


