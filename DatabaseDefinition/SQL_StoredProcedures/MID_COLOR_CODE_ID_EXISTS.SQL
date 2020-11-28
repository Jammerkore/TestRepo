--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns the row count based on the COLOR_CODE_ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_ID_EXISTS]
	@COLOR_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM COLOR_CODE 
	WHERE COLOR_CODE_ID = @COLOR_CODE_ID

END
GO
