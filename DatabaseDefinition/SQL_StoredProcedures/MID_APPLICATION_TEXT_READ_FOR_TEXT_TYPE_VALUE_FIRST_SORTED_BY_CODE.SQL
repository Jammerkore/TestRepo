--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads text (with text value as the first column) for the type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE]
	@TEXT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TEXT_VALUE, TEXT_CODE 
	FROM APPLICATION_TEXT
    WHERE TEXT_TYPE = @TEXT_TYPE
	ORDER BY TEXT_CODE

END
GO