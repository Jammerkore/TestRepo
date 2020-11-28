--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the basis for the asssortment property
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_BASIS_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM ASSORTMENT_PROPERTIES_BASIS
	WHERE HDR_RID = @HDR_RID
	ORDER BY BASIS_SEQ

END
GO