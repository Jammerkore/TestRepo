--dv =============================================
--dv Create date:	1/16/2014
--dv Description:	Reads application text for a text code
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_FROM_CODE]
	@TEXT_CODE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TEXT_VALUE 
	FROM APPLICATION_TEXT 
    WHERE TEXT_CODE = @TEXT_CODE

END
GO

