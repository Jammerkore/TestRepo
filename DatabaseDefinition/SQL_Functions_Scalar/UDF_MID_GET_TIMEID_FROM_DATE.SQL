CREATE FUNCTION [dbo].[UDF_MID_GET_TIMEID_FROM_DATE]
(
  @input_date DATETIME,
  @offsetDays int
)
RETURNS INT
AS
BEGIN

	declare @retVal int

	select @input_date = DATEADD(day, @offsetDays, @input_date)

	set @retVal = datepart(year, @input_date) * 1000 + datepart(dy, @input_date)

	return @retVal

END
GO
