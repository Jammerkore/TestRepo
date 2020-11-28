--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes Assortment Properties
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_PROPERTIES 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO