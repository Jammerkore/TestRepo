--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Deletes a color code row based on the COLOR_CODE_RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_DELETE]
	@COLOR_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM COLOR_CODE
	WHERE COLOR_CODE_RID = @COLOR_CODE_RID

	SELECT @@ROWCOUNT;

END
GO
