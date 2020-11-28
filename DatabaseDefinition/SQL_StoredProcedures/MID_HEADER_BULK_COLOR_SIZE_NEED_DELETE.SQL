--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a header color curve aka header bulk color size need
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_BULK_COLOR_SIZE_NEED WITH (ROWLOCK) 
    WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;

END
GO
