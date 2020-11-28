--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Updates the pack color code for a header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_UPDATE_COLOR] 
	@HDR_RID int,
	@OLD_COLOR_CODE_RID int,
	@NEW_COLOR_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HEADER_PACK_COLOR WITH (ROWLOCK) 
	SET 
		COLOR_CODE_RID = @NEW_COLOR_CODE_RID
    WHERE COLOR_CODE_RID = @OLD_COLOR_CODE_RID
          AND HDR_PACK_RID IN (
							   SELECT HDR_PACK_RID 
							   FROM HEADER_PACK
						       WHERE HDR_RID = @HDR_RID
							  )

	SELECT @@ROWCOUNT;

END
GO
