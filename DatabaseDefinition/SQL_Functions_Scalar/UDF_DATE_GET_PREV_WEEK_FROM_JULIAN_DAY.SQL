--dv =============================================
--dv Create date:	01/02/2014
--dv Description:	Returns first day of prev week in julian format.
--dv                Requires input parm to be first day of a week.
--dv History:	5/23/2014 TT#3735 -jsobek -Size Day To Week Summary - Add NOLOCK 
--dv =============================================
CREATE FUNCTION [dbo].[UDF_DATE_GET_PREV_WEEK_FROM_JULIAN_DAY]
(
	@currJulWeek INT
)
RETURNS INT
AS
BEGIN
/*=======================================================*/
/* Requires that @currJulWeek is the first day of a week */
/*=======================================================*/
declare @prevJulWeek int
declare @currWeekOffset int
declare @prevWeekOffset int
select @currWeekOffset = FIRST_DAY_OF_WEEK_OFFSET from FISCAL_WEEKS WITH (NOLOCK) where FIRST_DAY_OF_WEEK = @currJulWeek  /* TT#3735 -jsobek -Size Day To Week Summary - Add NOLOCK */
set @prevWeekOffset = @currWeekOffset - 7
select @prevJulWeek = FIRST_DAY_OF_WEEK from FISCAL_WEEKS WITH (NOLOCK) where FIRST_DAY_OF_WEEK_OFFSET = @prevWeekOffset  /* TT#3735 -jsobek -Size Day To Week Summary - Add NOLOCK */
RETURN @prevJulWeek
END
/*End TT#3429-MD - STodd - Size day to week summary sales/stock accum issue */

GO


