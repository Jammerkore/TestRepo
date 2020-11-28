--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Deletes a Store Assortment Eligibility
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_STORE_ELIGIBILITY 
	WHERE
	HDR_RID = @HDR_RID
	 
	SELECT @@ROWCOUNT;
END
GO