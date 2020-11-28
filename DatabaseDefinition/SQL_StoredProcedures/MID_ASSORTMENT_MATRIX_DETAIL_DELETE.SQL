--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes Assortment Properties
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_MATRIX_DETAIL_DELETE]
	@HDR_RID_LIST HDR_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_MATRIX_DETAIL 
	WHERE
	HDR_RID in (SELECT HDR_RID FROM @HDR_RID_LIST)
	 
	SELECT @@ROWCOUNT;
END
GO