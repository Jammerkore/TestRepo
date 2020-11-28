--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads all text sorted by value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM APPLICATION_TEXT
	ORDER BY TEXT_VALUE

END
GO