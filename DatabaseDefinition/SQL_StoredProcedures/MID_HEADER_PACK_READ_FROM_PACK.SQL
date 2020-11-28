--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Reads header packs based on the header pack rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ_FROM_PACK]
	@HDR_PACK_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM HEADER_PACK 
	WHERE HDR_PACK_RID = @HDR_PACK_RID

	SELECT @@ROWCOUNT;

END
GO
