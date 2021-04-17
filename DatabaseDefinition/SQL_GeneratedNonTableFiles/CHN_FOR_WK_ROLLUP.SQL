if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_FOR_WK_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_FOR_WK_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_FOR_WK_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 800,
 @FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
 
-- select records to process
CREATE table #TEMP_ROLLUP_ITEM0 (
   [PROCESS] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
   [TIME_ID] [int] NOT NULL,
   [FV_RID] [int] NOT NULL,
   [ITEM_TYPE] [int] NOT NULL,
   [PH_RID] [int] NOT NULL,
   [HOME_LEVEL] [int] NOT NULL,
   [FIRST_DAY_OF_WEEK] [int] NOT NULL,
   [LAST_DAY_OF_WEEK] [int] NOT NULL,
   [BATCH_NUMBER] [int] NULL,
   [FIRST_DAY_OF_NEXT_WEEK] [int] NULL,
   [HN_MOD] [int] NULL,
   [GENERATED_ITEM] [char](1) NULL,
   [ITEM_PROCESSED] [char](1) NULL,
   [ALTERNATES_ONLY] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[PH_RID],
	[HN_RID],
	[FV_RID],
	[TIME_ID],
	[ITEM_TYPE],
	[PROCESS]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_ROLLUP_ITEM0 (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, 
   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)
select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,
  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY
  FROM ROLLUP_ITEM ri with (nolock)
  where ri.FV_RID = @FV_RID
    AND ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- clear existing values
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
         SET SALES = null,
             SALES_REG = null,
             SALES_PROMO = null,
             SALES_MKDN = null,
             STOCK = null,
             STOCK_REG = null,
             STOCK_MKDN = null
  from CHAIN_FORECAST_WEEK vw INNER JOIN #TEMP_ROLLUP_ITEM0 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
        AND vw.FV_RID = @FV_RID
        AND vw.FV_RID = tri.FV_RID
 
-- clear existing values
UPDATE CHAIN_FORECAST_WEEK_LOCK with (rowlock)
         SET SALES_LOCK = null,
             SALES_REG_LOCK = null,
             SALES_PROMO_LOCK = null,
             SALES_MKDN_LOCK = null,
             STOCK_LOCK = null,
             STOCK_REG_LOCK = null,
             STOCK_MKDN_LOCK = null
  from CHAIN_FORECAST_WEEK_LOCK vw INNER JOIN #TEMP_ROLLUP_ITEM0 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
        AND vw.FV_RID = @FV_RID
        AND vw.FV_RID = tri.FV_RID
 
-- select nodes to roll
CREATE table #TEMP_JOIN0 (
   [PH_RID] [int] NOT NULL,
   [PARENT_HN_RID] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PARENT_HN_RID],
	[HN_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_JOIN0(PH_RID, PARENT_HN_RID, HN_RID)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID   
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM0(
[HN_RID] [int] NOT NULL,
[FV_RID] [int] NOT NULL,
[TIME_ID] [int] NOT NULL,
[SALES] [int] NULL,
[SALES_REG] [int] NULL,
[SALES_PROMO] [int] NULL,
[SALES_MKDN] [int] NULL,
[STOCK] [int] NULL,
[STOCK_REG] [int] NULL,
[STOCK_MKDN] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HN_RID],
	[FV_RID],
	[TIME_ID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT ri.HN_RID, ri.FV_RID, ri.TIME_ID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join CHAIN_FORECAST_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID
	OPTION (MAXDOP 1)
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_CHN_FOR_WK_TYPE	
declare @Locks as MID_CHN_FOR_WK_LOCK_TYPE	
declare @SaveLocks char	
set @SaveLocks = '0'
	
insert into @TEMP_SUM0 (HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, STOCK, 
          STOCK_REG, STOCK_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_CHN_FOR_WK_WRITE @TEMP_SUM0,@Locks,@SaveLocks	
	
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM0 tri      
         ON ri.HN_RID = tri.HN_RID
        AND ri.TIME_ID = tri.TIME_ID
        AND ri.FV_RID = tri.FV_RID     
        AND ri.ITEM_TYPE = tri.ITEM_TYPE
        AND ri.PROCESS = tri.PROCESS
        AND ri.PH_RID = tri.PH_RID
        AND ri.HOME_LEVEL = tri.HOME_LEVEL
        AND ri.BATCH_NUMBER = @BATCH_NUMBER
	
delete from VIRTUAL_LOCK 
 where LOCK_TYPE = 25
   and LOCK_ID = 'RILock'
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM0') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM0
   IF OBJECT_ID('tempdb..#TEMP_JOIN0') IS NOT NULL DROP TABLE #TEMP_JOIN0
   IF OBJECT_ID('tempdb..#TEMP_SUM0') IS NOT NULL DROP TABLE #TEMP_SUM0
SET ANSI_WARNINGS ON
	


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_FOR_WK_NOZERO_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_FOR_WK_NOZERO_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_FOR_WK_NOZERO_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 800,
 @FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
 
-- select records to process
CREATE table #TEMP_ROLLUP_ITEM0 (
   [PROCESS] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
   [TIME_ID] [int] NOT NULL,
   [FV_RID] [int] NOT NULL,
   [ITEM_TYPE] [int] NOT NULL,
   [PH_RID] [int] NOT NULL,
   [HOME_LEVEL] [int] NOT NULL,
   [FIRST_DAY_OF_WEEK] [int] NOT NULL,
   [LAST_DAY_OF_WEEK] [int] NOT NULL,
   [BATCH_NUMBER] [int] NULL,
   [FIRST_DAY_OF_NEXT_WEEK] [int] NULL,
   [HN_MOD] [int] NULL,
   [GENERATED_ITEM] [char](1) NULL,
   [ITEM_PROCESSED] [char](1) NULL,
   [ALTERNATES_ONLY] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[PH_RID],
	[HN_RID],
	[FV_RID],
	[TIME_ID],
	[ITEM_TYPE],
	[PROCESS]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_ROLLUP_ITEM0 (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, 
   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)
select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,
  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY
  FROM ROLLUP_ITEM ri with (nolock)
  where ri.FV_RID = @FV_RID
    AND ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- select nodes to roll
CREATE table #TEMP_JOIN0 (
   [PH_RID] [int] NOT NULL,
   [PARENT_HN_RID] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PARENT_HN_RID],
	[HN_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_JOIN0(PH_RID, PARENT_HN_RID, HN_RID)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID   
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM0(
[HN_RID] [int] NOT NULL,
[FV_RID] [int] NOT NULL,
[TIME_ID] [int] NOT NULL,
[SALES] [int] NULL,
[SALES_REG] [int] NULL,
[SALES_PROMO] [int] NULL,
[SALES_MKDN] [int] NULL,
[STOCK] [int] NULL,
[STOCK_REG] [int] NULL,
[STOCK_MKDN] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HN_RID],
	[FV_RID],
	[TIME_ID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT ri.HN_RID, ri.FV_RID, ri.TIME_ID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join CHAIN_FORECAST_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID
	OPTION (MAXDOP 1)
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_CHN_FOR_WK_TYPE	
declare @Locks as MID_CHN_FOR_WK_LOCK_TYPE	
declare @SaveLocks char	
set @SaveLocks = '0'
	
insert into @TEMP_SUM0 (HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, STOCK, 
          STOCK_REG, STOCK_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_CHN_FOR_WK_WRITE @TEMP_SUM0,@Locks,@SaveLocks	
	
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM0 tri      
         ON ri.HN_RID = tri.HN_RID
        AND ri.TIME_ID = tri.TIME_ID
        AND ri.FV_RID = tri.FV_RID     
        AND ri.ITEM_TYPE = tri.ITEM_TYPE
        AND ri.PROCESS = tri.PROCESS
        AND ri.PH_RID = tri.PH_RID
        AND ri.HOME_LEVEL = tri.HOME_LEVEL
        AND ri.BATCH_NUMBER = @BATCH_NUMBER
	
delete from VIRTUAL_LOCK 
 where LOCK_TYPE = 25
   and LOCK_ID = 'RILock'
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM0') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM0
   IF OBJECT_ID('tempdb..#TEMP_JOIN0') IS NOT NULL DROP TABLE #TEMP_JOIN0
   IF OBJECT_ID('tempdb..#TEMP_SUM0') IS NOT NULL DROP TABLE #TEMP_SUM0
SET ANSI_WARNINGS ON
	


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_FOR_WK_LOCKS_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_FOR_WK_LOCKS_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_FOR_WK_LOCKS_ROLLUP]
(
	@PROCESS INT,
	@PH_RID INT,
	@HOME_LEVEL INT,
 @ITEM_TYPE INT = 800,
	@FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
-- select records to process
select * INTO #TEMP_ROLLUP_ITEM0 FROM ROLLUP_ITEM ri with (nolock)
   where ri.FV_RID = @FV_RID     
     AND ri.PROCESS = @PROCESS
     AND ri.ITEM_TYPE = @ITEM_TYPE
     AND ri.PH_RID = @PH_RID
     AND ri.HOME_LEVEL = @HOME_LEVEL
     AND ri.BATCH_NUMBER = @BATCH_NUMBER
     AND ri.ITEM_PROCESSED is null
 create clustered index #TEMP_ROLLUP_ITEM_IDX0
	    on #TEMP_ROLLUP_ITEM0
	    (PH_RID, HN_RID)
	
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- create temp table to hold values	
	
create table #TEMP0 (tmpHN_RID   int not null,
                tmpFV_RID   int not null,
                tmpTIME_ID  int not null,
                tmpPRESENTFLAG  int null,
                tmpSALES  int    null,
                tmpSALES_REG  int    null,
                tmpSALES_PROMO  int    null,
                tmpSALES_MKDN  int    null,
                tmpSTOCK  int    null,
                tmpSTOCK_REG  int    null,
                tmpSTOCK_MKDN  int    null,
                tmpSALES_LOCK  char(1) null,
                tmpSALES_REG_LOCK  char(1) null,
                tmpSALES_PROMO_LOCK  char(1) null,
                tmpSALES_MKDN_LOCK  char(1) null,
                tmpSTOCK_LOCK  char(1) null,
                tmpSTOCK_REG_LOCK  char(1) null,
                tmpSTOCK_MKDN_LOCK  char(1) null)
	
 alter table #TEMP0
	    add primary key clustered (tmpHN_RID, tmpFV_RID, tmpTIME_ID)
	
-- build a temp table of keys to be updated along with their lock flags
INSERT #TEMP0 (tmpHN_RID, tmpFV_RID, tmpTIME_ID, tmpPRESENTFLAG, 
               tmpSALES, tmpSALES_REG, tmpSALES_PROMO, tmpSALES_MKDN, tmpSTOCK, tmpSTOCK_REG, 
               tmpSTOCK_MKDN,
               tmpSALES_LOCK, tmpSALES_REG_LOCK, tmpSALES_PROMO_LOCK, tmpSALES_MKDN_LOCK, tmpSTOCK_LOCK, 
               tmpSTOCK_REG_LOCK, tmpSTOCK_MKDN_LOCK)
SELECT vw.HN_RID, vw.FV_RID, ri.TIME_ID, 0, 
          0, 0, 0, 0, 0, 0, 0,
          COALESCE(cfwl.SALES_LOCK, '0'), 
          COALESCE(cfwl.SALES_REG_LOCK, '0'), 
          COALESCE(cfwl.SALES_PROMO_LOCK, '0'), 
          COALESCE(cfwl.SALES_MKDN_LOCK, '0'), 
          COALESCE(cfwl.STOCK_LOCK, '0'), 
          COALESCE(cfwl.STOCK_REG_LOCK, '0'), 
          COALESCE(cfwl.STOCK_MKDN_LOCK, '0')
         from #TEMP_ROLLUP_ITEM0 ri, CHAIN_FORECAST_WEEK vw LEFT OUTER JOIN CHAIN_FORECAST_WEEK_LOCK cfwl 
           ON vw.HN_RID = cfwl.HN_RID AND vw.TIME_ID = cfwl.TIME_ID AND vw.FV_RID = cfwl.FV_RID 
         WHERE ri.PROCESS = @PROCESS 
           AND ri.ITEM_TYPE = @ITEM_TYPE
           AND ri.PH_RID = @PH_RID
           AND ri.HOME_LEVEL = @HOME_LEVEL
           AND ri.FV_RID = @FV_RID
           AND vw.HN_RID = ri.HN_RID
           AND vw.FV_RID = ri.FV_RID
           AND vw.TIME_ID = ri.TIME_ID
	
if @debug <> 0 select * from #TEMP0
	
-- select nodes to roll
select distinct hnj.PH_RID PH_RID, hnj.PARENT_HN_RID PARENT_HN_RID, hnj.HN_RID HN_RID  
  INTO #TEMP_JOIN0
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
 alter table #TEMP_JOIN0
	    add primary key clustered (PARENT_HN_RID, HN_RID)
	
if @debug <> 0 select * from #TEMP_JOIN0
	
-- build a temp table of all summed values
SELECT ri.HN_RID as sumHN_RID, ri.FV_RID as sumFV_RID, ri.TIME_ID as sumTIME_ID, 
               sum(SALES) as sumSALES, sum(SALES_REG) as sumSALES_REG, sum(SALES_PROMO) as sumSALES_PROMO, 
               sum(SALES_MKDN) as sumSALES_MKDN, sum(STOCK) as sumSTOCK, sum(STOCK_REG) as sumSTOCK_REG, 
               sum(STOCK_MKDN) as sumSTOCK_MKDN
         INTO #TEMP_SUM0
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
 inner join CHAIN_FORECAST_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.FV_RID = hnj.FV_RID and vw.TIME_ID = hnj.TIME_ID
       GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID
	      OPTION (MAXDOP 1)
	
 alter table #TEMP_SUM0
	    add primary key clustered (sumHN_RID, sumFV_RID, sumTIME_ID)
	
if @debug <> 0 select * from #TEMP_SUM0
	
-- update #TEMP with the summed values
UPDATE #TEMP0
         SET tmpSALES = sumSALES,
             tmpSALES_REG = sumSALES_REG,
             tmpSALES_PROMO = sumSALES_PROMO,
             tmpSALES_MKDN = sumSALES_MKDN,
             tmpSTOCK = sumSTOCK,
             tmpSTOCK_REG = sumSTOCK_REG,
             tmpSTOCK_MKDN = sumSTOCK_MKDN
       	FROM #TEMP0 tmp
            JOIN #TEMP_SUM0 ON tmpHN_RID = sumHN_RID 
               AND tmpTIME_ID = sumTIME_ID AND tmpFV_RID = sumFV_RID
	
if @debug <> 0 select * from #TEMP0
	
-- deleted updated records from #TEMP_SUM
delete #TEMP_SUM0
   FROM #TEMP_SUM0 ts INNER JOIN #TEMP0 tmp
               ON ts.sumHN_RID = tmp.tmpHN_RID
                 AND ts.sumTIME_ID = tmp.tmpTIME_ID
                 AND ts.sumFV_RID = tmp.tmpFV_RID
-- insert other records from #TEMP_SUM
INSERT #TEMP0 (tmpHN_RID, tmpFV_RID, tmpTIME_ID,  tmpPRESENTFLAG, 
               tmpSALES, tmpSALES_REG, tmpSALES_PROMO, tmpSALES_MKDN, tmpSTOCK, tmpSTOCK_REG, 
               tmpSTOCK_MKDN,
               tmpSALES_LOCK, tmpSALES_REG_LOCK, tmpSALES_PROMO_LOCK, tmpSALES_MKDN_LOCK, tmpSTOCK_LOCK, 
               tmpSTOCK_REG_LOCK, tmpSTOCK_MKDN_LOCK)
          SELECT sumHN_RID, sumFV_RID, sumTIME_ID,  0, COALESCE(sumSALES, 0), COALESCE(sumSALES_REG, 0), 
               COALESCE(sumSALES_PROMO, 0), COALESCE(sumSALES_MKDN, 0), COALESCE(sumSTOCK, 0), 
               COALESCE(sumSTOCK_REG, 0), COALESCE(sumSTOCK_MKDN, 0),
               0, 0, 0, 0, 0, 0, 0
	FROM #TEMP_SUM0
               WHERE COALESCE (sumSALES, sumSALES_REG, sumSALES_PROMO, sumSALES_MKDN, sumSTOCK, 
               sumSTOCK_REG, sumSTOCK_MKDN) IS NOT NULL
-- update CHAIN_FORECAST_WEEK with the summed values where the values are not locked
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET SALES = tmpSALES
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSALES_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET SALES_REG = tmpSALES_REG
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSALES_REG_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET SALES_PROMO = tmpSALES_PROMO
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSALES_PROMO_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET SALES_MKDN = tmpSALES_MKDN
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSALES_MKDN_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET STOCK = tmpSTOCK
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSTOCK_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET STOCK_REG = tmpSTOCK_REG
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSTOCK_REG_LOCK = 0     
UPDATE CHAIN_FORECAST_WEEK with (rowlock)
       	SET STOCK_MKDN = tmpSTOCK_MKDN
           FROM CHAIN_FORECAST_WEEK vw 
            JOIN #TEMP0 ON HN_RID = tmpHN_RID 
               AND TIME_ID = tmpTIME_ID 
               AND FV_RID = tmpFV_RID
          where tmpSTOCK_MKDN_LOCK = 0     
 
-- flag updated records
update #TEMP0 SET tmpPRESENTFLAG = 1
   from #TEMP0 tmp INNER JOIN  CHAIN_FORECAST_WEEK vw
               ON  tmp.tmpHN_RID = vw.HN_RID
	AND tmp.tmpTIME_ID = vw.TIME_ID
	AND tmp.tmpFV_RID = vw.FV_RID
 
-- insert new values 
INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
               STOCK, STOCK_REG, STOCK_MKDN)
       	SELECT tmp.tmpHN_RID, tmp.tmpFV_RID, tmp.tmpTIME_ID, 
               tmp.tmpSALES, tmp.tmpSALES_REG, tmp.tmpSALES_PROMO, tmp.tmpSALES_MKDN, tmp.tmpSTOCK, 
               tmp.tmpSTOCK_REG, tmp.tmpSTOCK_MKDN
         FROM #TEMP0 tmp
	   WHERE tmpPRESENTFLAG = 0
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM0 tri      
         on ri.PROCESS = tri.PROCESS     
        AND ri.HN_RID = tri.HN_RID
        AND ri.TIME_ID = tri.TIME_ID
        AND ri.FV_RID = tri.FV_RID     
        AND ri.ITEM_TYPE = tri.ITEM_TYPE
        AND ri.PH_RID = tri.PH_RID
        AND ri.HOME_LEVEL = tri.HOME_LEVEL
        AND ri.BATCH_NUMBER = @BATCH_NUMBER
	
delete from VIRTUAL_LOCK 
 where LOCK_TYPE = 25
   and LOCK_ID = 'RILock'
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM0') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM0
   IF OBJECT_ID('tempdb..#TEMP_JOIN0') IS NOT NULL DROP TABLE #TEMP_JOIN0
   IF OBJECT_ID('tempdb..#TEMP_SUM0') IS NOT NULL DROP TABLE #TEMP_SUM0
SET ANSI_WARNINGS ON
	


GO


