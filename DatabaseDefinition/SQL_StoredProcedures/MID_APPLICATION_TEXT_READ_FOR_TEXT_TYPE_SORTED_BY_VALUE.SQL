--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads text for the type and level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE]
	@TEXT_TYPE int,
	@TEXT_LEVEL int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM APPLICATION_TEXT 
    WHERE TEXT_TYPE = @TEXT_TYPE
         AND TEXT_LEVEL = @TEXT_LEVEL
	ORDER BY TEXT_VALUE

END
GO