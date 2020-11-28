--dv =============================================
--dv Create date:	5/8/2017
--dv Description:	Deletes header packs associations for the specified header RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_ASSOCIATION_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HEADER_PACK_ASSOCIATION WITH (ROWLOCK)
	WHERE HDR_RID = @HDR_RID

END
GO
