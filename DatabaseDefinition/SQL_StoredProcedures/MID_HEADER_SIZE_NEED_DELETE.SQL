--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a header detail curve aka header size need
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_SIZE_NEED_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_SIZE_NEED WITH (ROWLOCK) 
    WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;

END
GO
