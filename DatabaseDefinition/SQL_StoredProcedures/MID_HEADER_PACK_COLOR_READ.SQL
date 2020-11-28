--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header pack colors based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HEADER_PACK_COLOR 
	WHERE HDR_PACK_RID IN (SELECT HDR_PACK_RID FROM HEADER_PACK WHERE HDR_RID = @HDR_RID)

END
GO
