--dv Create date:	9/30/2013
--dv Description:	Gets assortment properties for a header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER] 
	@headerRID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[UDF_GET_ASSORTMENT_PROPERTIES_FOR_HEADER](@headerRID);
END
GO
