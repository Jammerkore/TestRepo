--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Returns Stores and Units for Intransit
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_INTRANSIT]
	@HN_List GET_INTRANSIT_HN_TYPE READONLY,
	@ST_Day_List GET_INTRANSIT_ST_TYPE READONLY,
	@flashBackIn datetime,
	@FlashBack datetime output,
	@Return_Code int = -100 output,
	@debug bit = 0
AS
if (@debug = 0) 
	set nocount on
set @FlashBack = coalesce(@flashBackIn,getdate())
if @debug <> 0 select @FlashBack
declare @hn_tbl table (hn int primary key (hn))
INSERT @hn_tbl 
SELECT * FROM @HN_List
if @debug <> 0	select * from @hn_tbl
declare @rtncd int, @postday int, @nextday int
exec @rtncd=SP_MID_GET_POSTING_DATE @postday output, @nextday output
if @debug <> 0	select @rtncd, @postday, @nextday
declare @st_day_tbl table(st int, startday int, endday int primary key(st))
INSERT @st_day_tbl 
SELECT * FROM @ST_Day_List 
if @debug <> 0 select * from @st_day_tbl
declare @st_inv table (ST_RID int, UNITS int)
insert into @st_inv
	select ST_RID, sum(UNITS) as UNITS
	from dbo.VW_STORE_INTRANSIT I
	join @hn_tbl H on I.HN_RID = H.hn 
	join @st_day_tbl S on I.ST_RID = S.st 
		and I.TIME_ID between case when @nextday >= S.startday then 0 else S.startday end and S.endday
	group by ST_RID
    OPTION (HASH GROUP) 
insert into @st_inv
	select ST_RID, sum(UNITS) as UNITS
	from dbo.VW_STORE_EXTERNAL_INTRANSIT I
	join @hn_tbl H on I.HN_RID = H.hn 
	join @st_day_tbl S on I.ST_RID = S.st 
		and I.TIME_ID between case when @nextday >= S.startday then @nextday else S.startday end and S.endday
	group by ST_RID
    OPTION (HASH GROUP) 
insert into @st_inv
	select ST_RID, sum(UNITS) as UNITS
	from dbo.STORE_INTRANSIT_REV I
	join @hn_tbl H on I.HN_RID = H.hn 
	join @st_day_tbl S on I.ST_RID = S.st and I.TIME_ID between S.startday and S.endday
	where I.FLASHBACK > @FlashBack
	group by ST_RID
    OPTION (HASH GROUP) 
select ST_RID, sum(UNITS) as UNITS
	from @st_inv
	group by ST_RID
    OPTION (HASH GROUP) 
set @Return_Code = @@RowCount
return @Return_Code	

GO