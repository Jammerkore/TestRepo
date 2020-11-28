--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Deletes Assortment view Details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_DETAIL_DELETE]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ASSORTMENT_VIEW_DETAIL
	WHERE
	VIEW_RID = @VIEW_RID
	 
	SELECT @@ROWCOUNT;
END
GO