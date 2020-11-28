--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header bulk color size data based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_SIZE_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		COLOR_CODE_RID, 
		HEADER_PACK_COLOR_SIZE.*
    FROM HEADER_PACK_COLOR_SIZE, 
		 HEADER_PACK_COLOR 
    WHERE HEADER_PACK_COLOR.HDR_PACK_RID IN (SELECT HDR_PACK_RID FROM HEADER_PACK WHERE HDR_RID = @HDR_RID)
          AND HEADER_PACK_COLOR_SIZE.HDR_PC_RID = HEADER_PACK_COLOR.HDR_PC_RID
    ORDER BY HEADER_PACK_COLOR_SIZE.SEQ

END
GO
