--dv =============================================
--dv Created date:	2/5/2015
--dv Description:	Converts a start and end date time to a formatted string (format dd:HH:mm:ss)
--dv Notes: Max difference is 68 years
--dv =============================================
CREATE FUNCTION [dbo].[UDF_DATE_GET_DURATION_AS_STRING]
(
	@startDate DATETIME,
	@endDate DATETIME
)
RETURNS VARCHAR(11)
AS
BEGIN
	DECLARE @formattedString VARCHAR(11);
	DECLARE @diffSeconds INT = 0;
	DECLARE @diffMinutes INT = 0;
	DECLARE @diffHours INT = 0;
	DECLARE @diffDays INT = 0;

	SET @diffSeconds = DATEDIFF(second, @startDate, @endDate);
	SET @diffMinutes = @diffSeconds / 60;
	SET @diffHours = @diffMinutes / 60;
	SET @diffDays = @diffHours / 24;


	SET @formattedString = RIGHT('00'+ CONVERT(VARCHAR,@diffDays) ,2) + ':' + RIGHT('00'+ CONVERT(VARCHAR,@diffHours % 24) ,2) + ':' + RIGHT('00'+ CONVERT(VARCHAR,@diffMinutes % 60) ,2) + ':' + RIGHT('00'+ CONVERT(VARCHAR, @diffSeconds % 60) ,2) 

	RETURN @formattedString

END
GO
