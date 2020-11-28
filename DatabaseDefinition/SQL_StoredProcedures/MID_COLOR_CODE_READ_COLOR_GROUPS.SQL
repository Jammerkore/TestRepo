--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns distinct color groups from color codes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_READ_COLOR_GROUPS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		COLOR_CODE_GROUP 
	FROM COLOR_CODE 
	WHERE COLOR_CODE_RID > 0 AND PURPOSE = 0 
	ORDER BY COLOR_CODE_GROUP

END
GO
