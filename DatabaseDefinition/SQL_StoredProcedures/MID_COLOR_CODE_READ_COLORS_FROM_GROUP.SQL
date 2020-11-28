--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns color codes based on group name passed in
--dv				If @COLOR_CODE_GROUP is null, returns all colors
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_READ_COLORS_FROM_GROUP]
	@COLOR_CODE_GROUP varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM COLOR_CODE 
	WHERE COLOR_CODE_RID > 0
	AND ((@COLOR_CODE_GROUP IS NULL) OR COLOR_CODE_GROUP=@COLOR_CODE_GROUP)
	ORDER BY COLOR_CODE_ID

END
GO
