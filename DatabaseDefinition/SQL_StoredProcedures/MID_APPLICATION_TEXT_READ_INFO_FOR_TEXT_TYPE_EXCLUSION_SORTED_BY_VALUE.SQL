--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads text for the type, excluding codes in the list
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE]
	@TEXT_TYPE int,
	@TEXT_CODE_EXCLUDE_LIST TEXT_CODE_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM APPLICATION_TEXT
    WHERE TEXT_TYPE = @TEXT_TYPE
	    AND TEXT_CODE NOT IN (SELECT TEXT_CODE FROM @TEXT_CODE_EXCLUDE_LIST)
	ORDER BY TEXT_VALUE

END
GO