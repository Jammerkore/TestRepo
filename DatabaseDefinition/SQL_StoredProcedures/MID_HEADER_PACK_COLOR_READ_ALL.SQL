--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header pack colors
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		HEADER_PACK_COLOR.* 
	FROM HEADER_PACK_COLOR,
         HEADER_PACK 
	WHERE HEADER_PACK.HDR_PACK_RID = HEADER_PACK_COLOR.HDR_PACK_RID
    ORDER BY HEADER_PACK_COLOR.HDR_PACK_RID, HEADER_PACK_COLOR.SEQ

END
GO
