--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns the COLOR_CODE_RID based on the COLOR_CODE_ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_READ_RID_FROM_ID]
	@COLOR_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COLOR_CODE_RID 
	FROM COLOR_CODE 
	WHERE COLOR_CODE_ID = @COLOR_CODE_ID

END
GO
