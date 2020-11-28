--dv =============================================
--dv Create date:	5/20/2014
--dv Description:	Check to see if a store has any Allocated Units
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_STORE_ALLOCATION_COUNT]
	@ST_RID int,
	@COUNT int output
AS
BEGIN
		set @COUNT = 0

		IF ((select top 1 ba.UNITS_ALLOCATED as BA_STORE_COUNT from BULK_ALLOCATION ba where ba.ST_RID = @ST_RID) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 bca.UNITS_ALLOCATED as BCA_STORE_COUNT from BULK_COLOR_ALLOCATION bca where bca.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 bcsa.UNITS_ALLOCATED as BCSA_STORE_COUNT from BULK_COLOR_SIZE_ALLOCATION bcsa where bcsa.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 da.UNITS_ALLOCATED as DA_STORE_COUNT from DETAIL_ALLOCATION da where da.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 pa.PACKS_ALLOCATED as PA_STORE_COUNT from PACK_ALLOCATION pa where pa.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE IF ((select top 1 ta.UNITS_ALLOCATED as TA_STORE_COUNT from TOTAL_ALLOCATION ta where ta.ST_RID = @ST_RID ) > 0)
		BEGIN
			Set @COUNT = 1
			Return 
		END
		ELSE
			Return 
END
GO