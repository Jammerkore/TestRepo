--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads text (with text value as the first column) for the type and level, excluding text codes in the list
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE]
	@TEXT_TYPE int,
	@TEXT_CODE_EXCLUDE_LIST TEXT_CODE_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TEXT_VALUE, TEXT_CODE 
	FROM APPLICATION_TEXT
    WHERE TEXT_TYPE = @TEXT_TYPE
          AND TEXT_CODE NOT IN (SELECT TEXT_CODE FROM @TEXT_CODE_EXCLUDE_LIST)
	ORDER BY TEXT_VALUE

END
GO