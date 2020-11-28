--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a Assortment Properties Basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_BASIS_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_PROPERTIES_BASIS 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO