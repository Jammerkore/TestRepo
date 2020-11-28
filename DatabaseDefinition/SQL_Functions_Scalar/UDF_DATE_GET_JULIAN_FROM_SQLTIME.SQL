--dv Create date: 5/7/2013
--dv Description:	Converts a SQL Time ID to a Julian Time ID (format YYYYDDD)
--dv =============================================
CREATE FUNCTION [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME]
(
	@SQL_TIME_ID INT
)
RETURNS INT
AS
BEGIN
	DECLARE @JULIAN_TIME_ID INT


	DECLARE @TYEAR INT
	DECLARE @TDAY INT
	SET @TYEAR = 1970;
	SET @TDAY = 1;
	
	DECLARE @DAYCOUNT INT
	SET @DAYCOUNT = @SQL_TIME_ID;

	IF (@SQL_TIME_ID < 1)
	BEGIN
		SET @TYEAR = 1969;
		SET @TDAY = 365;
		WHILE @DAYCOUNT < 1
		BEGIN
			IF ([dbo].[UDF_IS_LEAP_YEAR](@TYEAR)=1)  --Leap Year
			BEGIN
				IF @DAYCOUNT < -366
				BEGIN
					SET @DAYCOUNT = @DAYCOUNT + 366;
					SET @TYEAR = @TYEAR - 1;
				END
				ELSE
				BEGIN
					SET @TDAY = 366 + @DAYCOUNT;
					SET @DAYCOUNT = 1;
				END
			END
			ELSE --Not Leap Year
			BEGIN
				IF @DAYCOUNT < -365
				BEGIN
					SET @DAYCOUNT = @DAYCOUNT + 365;
					SET @TYEAR = @TYEAR - 1;
				END
				ELSE
				BEGIN
					SET @TDAY = 365 + @DAYCOUNT;
					SET @DAYCOUNT = 1;
				END
			END
		END  --end while loop
	END
	ELSE --@SQL_TIME_ID greater than or equal to one
	BEGIN
		WHILE @DAYCOUNT > 0
		BEGIN
			IF ([dbo].[UDF_IS_LEAP_YEAR](@TYEAR)=1)  --Leap Year
			BEGIN
				IF @DAYCOUNT > 366
				BEGIN
					SET @DAYCOUNT = @DAYCOUNT - 366;
					SET @TYEAR = @TYEAR + 1;
				END
				ELSE
				BEGIN
					SET @TDAY = @DAYCOUNT;
					SET @DAYCOUNT = 0;
				END
			END
			ELSE --Not Leap Year
			BEGIN
				IF @DAYCOUNT > 365
				BEGIN
					SET @DAYCOUNT = @DAYCOUNT - 365;
					SET @TYEAR = @TYEAR + 1;
				END
				ELSE
				BEGIN
					SET @TDAY = @DAYCOUNT;
					SET @DAYCOUNT = 0;
				END
			END
		END  --end while loop
	END


	SET @JULIAN_TIME_ID = @TYEAR * 1000 + @TDAY;
	
	RETURN @JULIAN_TIME_ID

END
GO
