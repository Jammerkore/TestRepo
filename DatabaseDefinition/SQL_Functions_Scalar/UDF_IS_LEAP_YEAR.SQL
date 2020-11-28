--dv Alogrithm: 
--dv				if year modulo 400 is 0 then leap
--dv				else if year modulo 100 is 0 then no_leap
--dv				else if year modulo 4 is 0 then leap
--dv				else no_leap
--dv =============================================
CREATE FUNCTION [dbo].[UDF_IS_LEAP_YEAR]
(
	@theyear INT
)
RETURNS INT
AS
BEGIN

	declare @retVal tinyint

 select @retVal =
 case 
  when @theyear % 4 <> 0 then 0 
  else
   case 
    when @theyear % 100 = 0 
    then 
     case 
      when @theyear % 400 = 0 
      then 1 
      else 0 end 
    else 1 end 
 end

 return @retVal

END
GO
