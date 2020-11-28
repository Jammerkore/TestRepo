--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads the text level for the text code
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_LEVEL]
	@TEXT_CODE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TEXT_LEVEL 
	FROM APPLICATION_TEXT
    WHERE TEXT_CODE = @TEXT_CODE

END
GO