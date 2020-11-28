--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a header color curve, aka header bulk color size need, by the bulk color rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR]
	@HDR_BC_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_BULK_COLOR_SIZE_NEED WITH (ROWLOCK) 
    WHERE HDR_BC_RID = @HDR_BC_RID

	SELECT @@ROWCOUNT;

END
GO
