--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Updates the bulk color code for a header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_UPDATE_COLOR] 
	@HDR_RID int,
	@OLD_COLOR_CODE_RID int,
	@NEW_COLOR_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HEADER_BULK_COLOR WITH (ROWLOCK) 
	SET 
		COLOR_CODE_RID = @NEW_COLOR_CODE_RID
    WHERE HDR_RID = @HDR_RID
          AND COLOR_CODE_RID = @OLD_COLOR_CODE_RID 

	SELECT @@ROWCOUNT;

END
GO
