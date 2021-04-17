if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_FOR_WK0_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_FOR_WK0_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_FOR_WK0_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 400,
 @FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
DECLARE  @Tables INT
-- get the number of store tables
SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS
 
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
    AND ri.HN_MOD = 0
    AND ri.HN_RID % @Tables = 0
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- clear existing values
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
         SET SALES = null,
             SALES_REG = null,
             SALES_PROMO = null,
             SALES_MKDN = null,
             STOCK = null,
             STOCK_REG = null,
             STOCK_MKDN = null,
             IN_STOCK_SALES = null,
             IN_STOCK_SALES_REG = null,
             IN_STOCK_SALES_PROMO = null,
             IN_STOCK_SALES_MKDN = null
  from STORE_FORECAST_WEEK0 vw INNER JOIN #TEMP_ROLLUP_ITEM0 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
        AND vw.FV_RID = @FV_RID
        AND vw.FV_RID = tri.FV_RID
 
-- clear existing values
UPDATE STORE_FORECAST_WEEK_LOCK with (rowlock)
         SET SALES_LOCK = null,
             SALES_REG_LOCK = null,
             SALES_PROMO_LOCK = null,
             SALES_MKDN_LOCK = null,
             STOCK_LOCK = null,
             STOCK_REG_LOCK = null,
             STOCK_MKDN_LOCK = null,
             IN_STOCK_SALES_LOCK = null,
             IN_STOCK_SALES_REG_LOCK = null,
             IN_STOCK_SALES_PROMO_LOCK = null,
             IN_STOCK_SALES_MKDN_LOCK = null
  from STORE_FORECAST_WEEK_LOCK vw INNER JOIN #TEMP_ROLLUP_ITEM0 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
        AND vw.FV_RID = @FV_RID
        AND vw.FV_RID = tri.FV_RID
 
-- select nodes to roll
CREATE table #TEMP_JOIN0 (
   [PH_RID] [int] NOT NULL,
   [PARENT_HN_RID] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
   [HN_MOD] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PARENT_HN_RID],
	[HN_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_JOIN0(PH_RID, PARENT_HN_RID, HN_RID, HN_MOD)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID, hnj.HN_RID % @Tables   
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM0(
[HN_MOD] [int] NOT NULL,
[HN_RID] [int] NOT NULL,
[FV_RID] [int] NOT NULL,
[TIME_ID] [int] NOT NULL,
[ST_RID] [int] NOT NULL,
[SALES] [int] NULL,
[SALES_REG] [int] NULL,
[SALES_PROMO] [int] NULL,
[SALES_MKDN] [int] NULL,
[STOCK] [int] NULL,
[STOCK_REG] [int] NULL,
[STOCK_MKDN] [int] NULL,
[IN_STOCK_SALES] [int] NULL,
[IN_STOCK_SALES_REG] [int] NULL,
[IN_STOCK_SALES_PROMO] [int] NULL,
[IN_STOCK_SALES_MKDN] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HN_MOD],
	[HN_RID],
	[FV_RID],
	[TIME_ID],
	[ST_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, 
          SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, 
          IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT 0, ri.HN_RID, ri.FV_RID, ri.TIME_ID, ST_RID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN), sum(IN_STOCK_SALES), 
          sum(IN_STOCK_SALES_REG), sum(IN_STOCK_SALES_PROMO), sum(IN_STOCK_SALES_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join VW_STORE_FORECAST_WEEK vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.FV_RID, ST_RID, ri.TIME_ID
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_ST_FOR_WK_TYPE	
	
insert into @TEMP_SUM0 (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, 
          SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, 
          IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, 
          IN_STOCK_SALES_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_ST_FOR_WK0_WRITE @TEMP_SUM0	
	
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


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_FOR_WK0_NOZERO_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_FOR_WK0_NOZERO_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_FOR_WK0_NOZERO_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 400,
 @FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
DECLARE  @Tables INT
-- get the number of store tables
SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS
 
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
    AND ri.HN_MOD = 0
    AND ri.HN_RID % @Tables = 0
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- select nodes to roll
CREATE table #TEMP_JOIN0 (
   [PH_RID] [int] NOT NULL,
   [PARENT_HN_RID] [int] NOT NULL,
   [HN_RID] [int] NOT NULL,
   [HN_MOD] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PARENT_HN_RID],
	[HN_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
 
insert INTO #TEMP_JOIN0(PH_RID, PARENT_HN_RID, HN_RID, HN_MOD)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID, hnj.HN_RID % @Tables   
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM0(
[HN_MOD] [int] NOT NULL,
[HN_RID] [int] NOT NULL,
[FV_RID] [int] NOT NULL,
[TIME_ID] [int] NOT NULL,
[ST_RID] [int] NOT NULL,
[SALES] [int] NULL,
[SALES_REG] [int] NULL,
[SALES_PROMO] [int] NULL,
[SALES_MKDN] [int] NULL,
[STOCK] [int] NULL,
[STOCK_REG] [int] NULL,
[STOCK_MKDN] [int] NULL,
[IN_STOCK_SALES] [int] NULL,
[IN_STOCK_SALES_REG] [int] NULL,
[IN_STOCK_SALES_PROMO] [int] NULL,
[IN_STOCK_SALES_MKDN] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HN_MOD],
	[HN_RID],
	[FV_RID],
	[TIME_ID],
	[ST_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, 
          SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, 
          IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT 0, ri.HN_RID, ri.FV_RID, ri.TIME_ID, ST_RID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN), sum(IN_STOCK_SALES), 
          sum(IN_STOCK_SALES_REG), sum(IN_STOCK_SALES_PROMO), sum(IN_STOCK_SALES_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join VW_STORE_FORECAST_WEEK vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.FV_RID, ST_RID, ri.TIME_ID
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_ST_FOR_WK_TYPE	
	
insert into @TEMP_SUM0 (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, 
          SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, 
          IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, 
          IN_STOCK_SALES_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_ST_FOR_WK0_WRITE @TEMP_SUM0	
	
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


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_FOR_WK0_LOCKS_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_FOR_WK0_LOCKS_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_FOR_WK0_LOCKS_ROLLUP]
(
	@PROCESS INT,
	@PH_RID INT,
	@HOME_LEVEL INT,
 @ITEM_TYPE INT = 400,
	@FV_RID INT,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
DECLARE  @Tables INT
-- get the number of store tables
SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS
-- select records to process
select * INTO #TEMP_ROLLUP_ITEM0 FROM ROLLUP_ITEM ri with (nolock)
   where ri.FV_RID = @FV_RID     
     AND ri.PROCESS = @PROCESS
     AND ri.ITEM_TYPE = @ITEM_TYPE
     AND ri.PH_RID = @PH_RID
     AND ri.HOME_LEVEL = @HOME_LEVEL
     AND ri.BATCH_NUMBER = @BATCH_NUMBER
     AND ri.ITEM_PROCESSED is null
     AND ri.HN_MOD = 0
     AND ri.HN_RID % @Tables = 0
 create clustered index #TEMP_ROLLUP_ITEM_IDX0
	    on #TEMP_ROLLUP_ITEM0
	    (PH_RID, HN_RID)
	
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- create temp table to hold values	
	
create table #TEMP0 (tmpHN_RID   int not null,
                tmpFV_RID   int not null,
                tmpST_RID   int not null,
                tmpTIME_ID  int not null,
                tmpPRESENTFLAG  int null,
                tmpSALES  int    null,
                tmpSALES_REG  int    null,
                tmpSALES_PROMO  int    null,
                tmpSALES_MKDN  int    null,
                tmpSTOCK  int    null,
                tmpSTOCK_REG  int    null,
                tmpSTOCK_MKDN  int    null,
                tmpIN_STOCK_SALES  int    null,
                tmpIN_STOCK_SALES_REG  int    null,
                tmpIN_STOCK_SALES_PROMO  int    null,
                tmpIN_STOCK_SALES_MKDN  int    null,
                tmpHN_MOD  smallint null,
                tmpSALES_LOCK  char(1) null,
                tmpSALES_REG_LOCK  char(1) null,
                tmpSALES_PROMO_LOCK  char(1) null,
                tmpSALES_MKDN_LOCK  char(1) null,
                tmpSTOCK_LOCK  char(1) null,
                tmpSTOCK_REG_LOCK  char(1) null,
                tmpSTOCK_MKDN_LOCK  char(1) null,
                tmpIN_STOCK_SALES_LOCK  char(1) null,
                tmpIN_STOCK_SALES_REG_LOCK  char(1) null,
                tmpIN_STOCK_SALES_PROMO_LOCK  char(1) null,
                tmpIN_STOCK_SALES_MKDN_LOCK  char(1) null)
	
 alter table #TEMP0
	    add primary key clustered (tmpHN_RID, tmpFV_RID, tmpTIME_ID, tmpST_RID)
	
-- build a temp table of keys to be updated along with their lock flags
INSERT #TEMP0 (tmpHN_RID, tmpFV_RID, tmpST_RID, tmpTIME_ID, tmpPRESENTFLAG, 
               tmpSALES, tmpSALES_REG, tmpSALES_PROMO, tmpSALES_MKDN, tmpSTOCK, tmpSTOCK_REG, 
               tmpSTOCK_MKDN, tmpIN_STOCK_SALES, tmpIN_STOCK_SALES_REG, tmpIN_STOCK_SALES_PROMO, 
               tmpIN_STOCK_SALES_MKDN,tmpHN_MOD,
               tmpSALES_LOCK, tmpSALES_REG_LOCK, tmpSALES_PROMO_LOCK, tmpSALES_MKDN_LOCK, tmpSTOCK_LOCK, 
               tmpSTOCK_REG_LOCK, tmpSTOCK_MKDN_LOCK, tmpIN_STOCK_SALES_LOCK, tmpIN_STOCK_SALES_REG_LOCK, 
               tmpIN_STOCK_SALES_PROMO_LOCK, tmpIN_STOCK_SALES_MKDN_LOCK)
SELECT vw.HN_RID, vw.FV_RID, vw.ST_RID, ri.TIME_ID, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          vw.HN_RID % @Tables,
          COALESCE(sfwl.SALES_LOCK, '0'), 
          COALESCE(sfwl.SALES_REG_LOCK, '0'), 
          COALESCE(sfwl.SALES_PROMO_LOCK, '0'), 
          COALESCE(sfwl.SALES_MKDN_LOCK, '0'), 
          COALESCE(sfwl.STOCK_LOCK, '0'), 
          COALESCE(sfwl.STOCK_REG_LOCK, '0'), 
          COALESCE(sfwl.STOCK_MKDN_LOCK, '0'), 
          COALESCE(sfwl.IN_STOCK_SALES_LOCK, '0'), 
          COALESCE(sfwl.IN_STOCK_SALES_REG_LOCK, '0'), 
          COALESCE(sfwl.IN_STOCK_SALES_PROMO_LOCK, '0'), 
          COALESCE(sfwl.IN_STOCK_SALES_MKDN_LOCK, '0')
         from #TEMP_ROLLUP_ITEM0 ri, VW_STORE_FORECAST_WEEK vw with (nolock) LEFT OUTER JOIN STORE_FORECAST_WEEK_LOCK sfwl 
           ON vw.HN_RID = sfwl.HN_RID AND vw.TIME_ID = sfwl.TIME_ID AND vw.FV_RID = sfwl.FV_RID AND vw.ST_RID = sfwl.ST_RID 
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
select distinct hnj.PH_RID PH_RID, hnj.PARENT_HN_RID PARENT_HN_RID, hnj.HN_RID HN_RID, hnj.HN_RID % @Tables HN_MOD  
  INTO #TEMP_JOIN0
  from #TEMP_ROLLUP_ITEM0 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
 alter table #TEMP_JOIN0
	    add primary key clustered (PARENT_HN_RID, HN_RID)
	
-- build a temp table of all summed values
SELECT ri.HN_RID as sumHN_RID, ri.FV_RID as sumFV_RID, vw.ST_RID as sumST_RID, ri.TIME_ID as sumTIME_ID,  
               sum(SALES) as sumSALES, sum(SALES_REG) as sumSALES_REG, sum(SALES_PROMO) as sumSALES_PROMO, 
          sum(SALES_MKDN) as sumSALES_MKDN, sum(STOCK) as sumSTOCK, sum(STOCK_REG) as sumSTOCK_REG, 
          sum(STOCK_MKDN) as sumSTOCK_MKDN, sum(IN_STOCK_SALES) as sumIN_STOCK_SALES, 
          sum(IN_STOCK_SALES_REG) as sumIN_STOCK_SALES_REG, sum(IN_STOCK_SALES_PROMO) as sumIN_STOCK_SALES_PROMO, 
          sum(IN_STOCK_SALES_MKDN) as sumIN_STOCK_SALES_MKDN
  INTO #TEMP_SUM0
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join VW_STORE_HISTORY_WEEK vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID
       GROUP BY ri.HN_RID, ri.FV_RID, ST_RID, ri.TIME_ID
	
 alter table #TEMP_SUM0
	    add primary key clustered (sumHN_RID, sumFV_RID, sumTIME_ID, sumST_RID)
	
if @debug <> 0 select * from #TEMP_SUM0
	
-- update #TEMP with the summed values
UPDATE #TEMP0
         SET tmpSALES = sumSALES,
             tmpSALES_REG = sumSALES_REG,
             tmpSALES_PROMO = sumSALES_PROMO,
             tmpSALES_MKDN = sumSALES_MKDN,
             tmpSTOCK = sumSTOCK,
             tmpSTOCK_REG = sumSTOCK_REG,
             tmpSTOCK_MKDN = sumSTOCK_MKDN,
             tmpIN_STOCK_SALES = sumIN_STOCK_SALES,
             tmpIN_STOCK_SALES_REG = sumIN_STOCK_SALES_REG,
             tmpIN_STOCK_SALES_PROMO = sumIN_STOCK_SALES_PROMO,
             tmpIN_STOCK_SALES_MKDN = sumIN_STOCK_SALES_MKDN
       	FROM #TEMP0 tmp
            JOIN #TEMP_SUM0 ts ON tmpHN_RID = sumHN_RID 
               AND tmpTIME_ID = sumTIME_ID AND tmpFV_RID = sumFV_RID
               AND tmpST_RID = sumST_RID
	
if @debug <> 0 select * from #TEMP0
	
-- deleted updated records from #TEMP_SUM
delete #TEMP_SUM0
   FROM #TEMP_SUM0 ts INNER JOIN #TEMP0 tmp
               ON ts.sumHN_RID = tmp.tmpHN_RID
                 AND ts.sumTIME_ID = tmp.tmpTIME_ID
                 AND ts.sumFV_RID = tmp.tmpFV_RID
                 AND ts.sumST_RID = tmp.tmpST_RID
-- insert other records from #TEMP_SUM
INSERT #TEMP0 (tmpHN_RID, tmpFV_RID, tmpST_RID, tmpTIME_ID, tmpPRESENTFLAG, 
               tmpSALES, tmpSALES_REG, tmpSALES_PROMO, tmpSALES_MKDN, tmpSTOCK, tmpSTOCK_REG, 
               tmpSTOCK_MKDN, tmpIN_STOCK_SALES, tmpIN_STOCK_SALES_REG, tmpIN_STOCK_SALES_PROMO, 
               tmpIN_STOCK_SALES_MKDN, tmpHN_MOD,
               tmpSALES_LOCK, tmpSALES_REG_LOCK, tmpSALES_PROMO_LOCK, tmpSALES_MKDN_LOCK, tmpSTOCK_LOCK, 
               tmpSTOCK_REG_LOCK, tmpSTOCK_MKDN_LOCK, tmpIN_STOCK_SALES_LOCK, tmpIN_STOCK_SALES_REG_LOCK, 
               tmpIN_STOCK_SALES_PROMO_LOCK, tmpIN_STOCK_SALES_MKDN_LOCK)
          SELECT ts.sumHN_RID, ts.sumFV_RID, ts.sumST_RID, ts.sumTIME_ID, 0,  COALESCE(ts.sumSALES, 0), 
               COALESCE(ts.sumSALES_REG, 0), COALESCE(ts.sumSALES_PROMO, 0), COALESCE(ts.sumSALES_MKDN, 0), 
               COALESCE(ts.sumSTOCK, 0), COALESCE(ts.sumSTOCK_REG, 0), COALESCE(ts.sumSTOCK_MKDN, 0), 
               COALESCE(ts.sumIN_STOCK_SALES, 0), COALESCE(ts.sumIN_STOCK_SALES_REG, 0), COALESCE(ts.sumIN_STOCK_SALES_PROMO, 0), 
               COALESCE(ts.sumIN_STOCK_SALES_MKDN, 0),  ts.sumHN_RID % @Tables,
               0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	FROM #TEMP_SUM0 ts
               WHERE COALESCE (ts.sumSALES, ts.sumSALES_REG, ts.sumSALES_PROMO, ts.sumSALES_MKDN, 
               ts.sumSTOCK, ts.sumSTOCK_REG, ts.sumSTOCK_MKDN, ts.sumIN_STOCK_SALES, ts.sumIN_STOCK_SALES_REG, 
               ts.sumIN_STOCK_SALES_PROMO, ts.sumIN_STOCK_SALES_MKDN) IS NOT NULL
-- update STORE_FORECAST_WEEK0 with the summed values where the values are not locked
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET SALES = tmp.tmpSALES
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSALES_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET SALES_REG = tmp.tmpSALES_REG
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSALES_REG_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET SALES_PROMO = tmp.tmpSALES_PROMO
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSALES_PROMO_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET SALES_MKDN = tmp.tmpSALES_MKDN
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSALES_MKDN_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET STOCK = tmp.tmpSTOCK
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSTOCK_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET STOCK_REG = tmp.tmpSTOCK_REG
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSTOCK_REG_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET STOCK_MKDN = tmp.tmpSTOCK_MKDN
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpSTOCK_MKDN_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET IN_STOCK_SALES = tmp.tmpIN_STOCK_SALES
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpIN_STOCK_SALES_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET IN_STOCK_SALES_REG = tmp.tmpIN_STOCK_SALES_REG
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpIN_STOCK_SALES_REG_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET IN_STOCK_SALES_PROMO = tmp.tmpIN_STOCK_SALES_PROMO
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpIN_STOCK_SALES_PROMO_LOCK = 0     
UPDATE STORE_FORECAST_WEEK0 with (rowlock)
       	SET IN_STOCK_SALES_MKDN = tmp.tmpIN_STOCK_SALES_MKDN
           FROM STORE_FORECAST_WEEK0 vw
            JOIN #TEMP0 tmp ON HN_RID = tmp.tmpHN_RID 
               AND vw.TIME_ID = tmp.tmpTIME_ID 
               AND vw.FV_RID = tmp.tmpFV_RID
               AND vw.ST_RID = tmp.tmpST_RID
          where tmp.tmpIN_STOCK_SALES_MKDN_LOCK = 0     
       
-- flag updated records
update #TEMP0 SET tmpPRESENTFLAG = 1
   from #TEMP0 tmp INNER JOIN STORE_FORECAST_WEEK0 vw
               ON  tmp.tmpHN_RID = vw.HN_RID
	AND tmp.tmpTIME_ID = vw.TIME_ID
	AND tmp.tmpFV_RID = vw.FV_RID
	AND tmp.tmpST_RID = vw.ST_RID
-- insert new values 
INSERT STORE_FORECAST_WEEK0 (HN_RID, FV_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, 
               SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
               IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN, HN_MOD)
       	SELECT tmp.tmpHN_RID, tmp.tmpFV_RID, tmp.tmpTIME_ID, tmp.tmpST_RID, 
               tmp.tmpSALES, tmp.tmpSALES_REG, tmp.tmpSALES_PROMO, tmp.tmpSALES_MKDN, tmp.tmpSTOCK, 
               tmp.tmpSTOCK_REG, tmp.tmpSTOCK_MKDN, tmp.tmpIN_STOCK_SALES, tmp.tmpIN_STOCK_SALES_REG, 
               tmp.tmpIN_STOCK_SALES_PROMO, tmp.tmpIN_STOCK_SALES_MKDN, tmp.tmpHN_MOD
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


