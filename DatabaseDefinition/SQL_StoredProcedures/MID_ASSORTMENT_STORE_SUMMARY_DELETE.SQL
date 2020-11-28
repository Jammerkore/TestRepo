--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a Assortment Store Summary
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_SUMMARY_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_STORE_SUMMARY 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO