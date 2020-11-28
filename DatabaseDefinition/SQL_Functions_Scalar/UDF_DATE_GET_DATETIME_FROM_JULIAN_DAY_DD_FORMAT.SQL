--dv Create date:	9/19/2013
--dv Description:	Returns a datetime from a julian day date in YYYYDD format
--dv =============================================
CREATE FUNCTION [dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY_DD_FORMAT]
(
	@julianTimeID INT
)
RETURNS DATETIME
AS
BEGIN
DECLARE @retVal DATETIME
DECLARE @numDays INT
DECLARE @theYear INT
SET @numDays = @julianTimeID % 100
SET @theYear = (@julianTimeID - (@numDays)) / 100
SET @retVal = CAST(@theYear AS VARCHAR(4)) + '01' + '01'
set @retVal = DATEADD(d, @numDays -1,  @retVal)
RETURN @retVal
END


GO
