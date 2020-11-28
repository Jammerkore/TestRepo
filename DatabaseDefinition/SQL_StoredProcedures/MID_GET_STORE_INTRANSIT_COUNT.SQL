--dv =============================================
--dv Create date:	5/20/2014
--dv Description:	Check to see if a store has any Intransit Units
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_STORE_INTRANSIT_COUNT]
	@ST_RID int,
	@COUNT int output
AS
BEGIN
		set @COUNT = 0

		IF ((select top 1 si.UNITS as STORE_COUNT from STORE_INTRANSIT si where si.ST_RID = @ST_RID) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 sei.UNITS as STORE_EXTERNAL_COUNT from STORE_EXTERNAL_INTRANSIT sei where sei.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE
			Return 
END
GO