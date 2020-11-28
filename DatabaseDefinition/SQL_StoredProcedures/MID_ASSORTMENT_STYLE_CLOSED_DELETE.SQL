--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Deletes a closed Assortment Style
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STYLE_CLOSED_DELETE]
	@HDR_RID_LIST HDR_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_STYLE_CLOSED 
	WHERE
	HDR_RID in (SELECT HDR_RID FROM @HDR_RID_LIST)
	 
	SELECT @@ROWCOUNT;
END
GO