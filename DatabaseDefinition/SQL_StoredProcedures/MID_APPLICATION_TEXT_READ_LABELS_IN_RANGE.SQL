--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads text within the range
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE]
	@TEXT_CODE_BEGIN_VALUE int,
	@TEXT_CODE_END_VALUE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TEXT_VALUE, 
		TEXT_CODE,
		TEXT_ORDER 
	FROM APPLICATION_TEXT
    WHERE TEXT_CODE BETWEEN @TEXT_CODE_BEGIN_VALUE AND @TEXT_CODE_END_VALUE
	ORDER BY TEXT_VALUE

END
GO