--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header bulk color sizes based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COLOR_CODE_RID,
        HEADER_BULK_COLOR_SIZE.*
    FROM HEADER_BULK_COLOR_SIZE, HEADER_BULK_COLOR
    WHERE HEADER_BULK_COLOR_SIZE.HDR_RID = @HDR_RID
          AND HEADER_BULK_COLOR_SIZE.HDR_BC_RID = HEADER_BULK_COLOR.HDR_BC_RID
    ORDER BY HEADER_BULK_COLOR.HDR_RID, HEADER_BULK_COLOR.SEQ, HEADER_BULK_COLOR_SIZE.SEQ

END
GO
