--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header packs for the specified header RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HEADER_PACK 
	WHERE HDR_RID = @HDR_RID

END
GO
