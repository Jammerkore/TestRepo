--dv Create date: 5/20/2013
--dv Description:	Returns a datetime from a julian day date
--dv =============================================
CREATE FUNCTION [dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY]
(
	@julianTimeID INT
)
RETURNS DATETIME
AS
BEGIN

DECLARE @retVal DATETIME

DECLARE @numDays INT
DECLARE @theYear INT
SET @numDays = @julianTimeID % 1000
SET @theYear = (@julianTimeID - (@numDays)) / 1000

SET @retVal = CAST(@theYear AS VARCHAR(4)) + '01' + '01'

set @retVal = DATEADD(d, @numDays -1,  @retVal)

RETURN @retVal

END
GO
