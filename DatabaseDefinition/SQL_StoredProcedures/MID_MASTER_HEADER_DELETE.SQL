--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Deletes a row in the master header table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_DELETE] 
	@SUBORD_HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE MASTER_HEADER WITH (ROWLOCK)
    WHERE SUBORD_HDR_RID = @SUBORD_HDR_RID

	SELECT @@ROWCOUNT;

END
GO
