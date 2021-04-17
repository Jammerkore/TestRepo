if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_HIS_DAY6_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_HIS_DAY6_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_HIS_DAY6_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 200,
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
CREATE table #TEMP_ROLLUP_ITEM6 (
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
 
insert INTO #TEMP_ROLLUP_ITEM6 (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, 
   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)
select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,
  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY
  FROM ROLLUP_ITEM ri with (nolock)
  where ri.FV_RID = 1
    AND ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
    AND ri.HN_MOD = 6
    AND ri.HN_RID % @Tables = 6
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM6
	
-- clear existing values
UPDATE STORE_HISTORY_DAY6 with (rowlock)
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
  from STORE_HISTORY_DAY6 vw INNER JOIN #TEMP_ROLLUP_ITEM6 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
         AND tri.FV_RID = 1
 
-- select nodes to roll
CREATE table #TEMP_JOIN6 (
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
 
insert INTO #TEMP_JOIN6(PH_RID, PARENT_HN_RID, HN_RID, HN_MOD)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID, hnj.HN_RID % @Tables   
  from #TEMP_ROLLUP_ITEM6 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM6(
[HN_MOD] [int] NOT NULL,
[HN_RID] [int] NOT NULL,
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
	[TIME_ID],
	[ST_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM6 (HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, 
          SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT 6, ri.HN_RID, ri.TIME_ID, ST_RID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN), sum(IN_STOCK_SALES), 
          sum(IN_STOCK_SALES_REG), sum(IN_STOCK_SALES_PROMO), sum(IN_STOCK_SALES_MKDN)
	FROM #TEMP_ROLLUP_ITEM6 ri
	inner join #TEMP_JOIN6 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join VW_STORE_HISTORY_DAY vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ST_RID, ri.TIME_ID
	
if @debug <> 0 select * from #TEMP_SUM6
	
	
declare @TEMP_SUM6 as MID_ST_HIS_DAY_TYPE	
	
insert into @TEMP_SUM6 (HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, 
          SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, 
          IN_STOCK_SALES_MKDN
	FROM #TEMP_SUM6
	
if @debug <> 0 select * from @TEMP_SUM6
	
	
exec dbo.SP_MID_ST_HIS_DAY6_WRITE @TEMP_SUM6	
	
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM6 tri      
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
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM6') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM6
   IF OBJECT_ID('tempdb..#TEMP_JOIN6') IS NOT NULL DROP TABLE #TEMP_JOIN6
   IF OBJECT_ID('tempdb..#TEMP_SUM6') IS NOT NULL DROP TABLE #TEMP_SUM6
SET ANSI_WARNINGS ON
	


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_HIS_DAY6_NOZERO_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_HIS_DAY6_NOZERO_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_HIS_DAY6_NOZERO_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 200,
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
CREATE table #TEMP_ROLLUP_ITEM6 (
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
 
insert INTO #TEMP_ROLLUP_ITEM6 (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, 
   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)
select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,
  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY
  FROM ROLLUP_ITEM ri with (nolock)
  where ri.FV_RID = 1
    AND ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
    AND ri.HN_MOD = 6
    AND ri.HN_RID % @Tables = 6
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM6
	
-- select nodes to roll
CREATE table #TEMP_JOIN6 (
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
 
insert INTO #TEMP_JOIN6(PH_RID, PARENT_HN_RID, HN_RID, HN_MOD)
select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID, hnj.HN_RID % @Tables   
  from #TEMP_ROLLUP_ITEM6 ri
  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID
	
CREATE table #TEMP_SUM6(
[HN_MOD] [int] NOT NULL,
[HN_RID] [int] NOT NULL,
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
	[TIME_ID],
	[ST_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM6 (HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, 
          SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT 6, ri.HN_RID, ri.TIME_ID, ST_RID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), 
          sum(SALES_MKDN), sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN), sum(IN_STOCK_SALES), 
          sum(IN_STOCK_SALES_REG), sum(IN_STOCK_SALES_PROMO), sum(IN_STOCK_SALES_MKDN)
	FROM #TEMP_ROLLUP_ITEM6 ri
	inner join #TEMP_JOIN6 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join VW_STORE_HISTORY_DAY vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ST_RID, ri.TIME_ID
	
if @debug <> 0 select * from #TEMP_SUM6
	
	
declare @TEMP_SUM6 as MID_ST_HIS_DAY_TYPE	
	
insert into @TEMP_SUM6 (HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, 
          SALES_MKDN, STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN)
SELECT HN_MOD, HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, IN_STOCK_SALES_PROMO, 
          IN_STOCK_SALES_MKDN
	FROM #TEMP_SUM6
	
if @debug <> 0 select * from @TEMP_SUM6
	
	
exec dbo.SP_MID_ST_HIS_DAY6_WRITE @TEMP_SUM6	
	
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM6 tri      
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
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM6') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM6
   IF OBJECT_ID('tempdb..#TEMP_JOIN6') IS NOT NULL DROP TABLE #TEMP_JOIN6
   IF OBJECT_ID('tempdb..#TEMP_SUM6') IS NOT NULL DROP TABLE #TEMP_SUM6
SET ANSI_WARNINGS ON
	


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_HIS_DAY_WK6_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_HIS_DAY_WK6_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_ST_HIS_DAY_WK6_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 100,
 @BATCH_NUMBER INT,
 @debug bit = 0
)
AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
DECLARE  @Tables INT
-- get the number of store tables
SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS
-- set up fields to get first and last values
DECLARE  @factorm bigint, @factora bigint
SET @factora = 0x100000000 -- needs to be a power of 2 > maximum user column absolute value
SET @factorm = @factora * 2
 
 
-- get the posting date
DECLARE  @PostDate INT
SELECT @PostDate = CURR_DATE_YYYYDDD FROM POSTING_DATE
 
 
-- select records to process
CREATE table #TEMP_ROLLUP_ITEM6 (
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
 
insert INTO #TEMP_ROLLUP_ITEM6 (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, 
   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)
select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,
  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY
  FROM ROLLUP_ITEM ri with (nolock)
  where ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
    AND ri.HN_MOD = 6
    AND ri.HN_RID % @Tables = 6
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM6
	
CREATE table #TEMP_SUM6(
[HN_MOD] [int] NOT NULL,
[HN_RID] [int] NOT NULL,
[TIME_ID] [int] NOT NULL,
[ST_RID] [int] NOT NULL,
[PRESENTFLAG] [int] NULL,
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
	[TIME_ID],
	[ST_RID]
)WITH (IGNORE_DUP_KEY = OFF)
)
-- build a temp table of all non stock values
INSERT INTO #TEMP_SUM6 (HN_RID, TIME_ID, ST_RID, PRESENTFLAG, 
          SALES, SALES_REG, SALES_PROMO, SALES_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN,HN_MOD)
SELECT tmp.HN_RID, tmp.FIRST_DAY_OF_WEEK, vw.ST_RID, 0, 
          sum(SALES) as SALES, sum(SALES_REG) as SALES_REG, sum(SALES_PROMO) as SALES_PROMO, 
          sum(SALES_MKDN) as SALES_MKDN, sum(IN_STOCK_SALES) as IN_STOCK_SALES, sum(IN_STOCK_SALES_REG) as IN_STOCK_SALES_REG, 
          sum(IN_STOCK_SALES_PROMO) as IN_STOCK_SALES_PROMO, sum(IN_STOCK_SALES_MKDN) as IN_STOCK_SALES_MKDN, 
	 vw.HN_MOD
         FROM STORE_HISTORY_DAY6 vw with (nolock), #TEMP_ROLLUP_ITEM6 tmp
         WHERE tmp.PROCESS = @PROCESS 
	AND tmp.ITEM_TYPE = @ITEM_TYPE
	AND tmp.PH_RID = @PH_RID
	AND tmp.HOME_LEVEL = @HOME_LEVEL
	AND vw.HN_RID = tmp.HN_RID
	AND vw.TIME_ID between tmp.FIRST_DAY_OF_WEEK and tmp.LAST_DAY_OF_WEEK
       GROUP BY vw.HN_MOD, tmp.HN_RID, ST_RID, tmp.FIRST_DAY_OF_WEEK
	
if @debug <> 0 select * from #TEMP_SUM6
	
-- update existing values
UPDATE STORE_HISTORY_WEEK6 with (rowlock)
         SET SALES = COALESCE(ts.SALES, vw.SALES),
             SALES_REG = COALESCE(ts.SALES_REG, vw.SALES_REG),
             SALES_PROMO = COALESCE(ts.SALES_PROMO, vw.SALES_PROMO),
             SALES_MKDN = COALESCE(ts.SALES_MKDN, vw.SALES_MKDN),
             IN_STOCK_SALES = COALESCE(ts.IN_STOCK_SALES, vw.IN_STOCK_SALES),
             IN_STOCK_SALES_REG = COALESCE(ts.IN_STOCK_SALES_REG, vw.IN_STOCK_SALES_REG),
             IN_STOCK_SALES_PROMO = COALESCE(ts.IN_STOCK_SALES_PROMO, vw.IN_STOCK_SALES_PROMO),
             IN_STOCK_SALES_MKDN = COALESCE(ts.IN_STOCK_SALES_MKDN, vw.IN_STOCK_SALES_MKDN)
       	FROM STORE_HISTORY_WEEK6 vw
            JOIN #TEMP_SUM6 ts ON vw.HN_RID = ts.HN_RID 
               AND vw.TIME_ID = ts.TIME_ID 
               AND vw.ST_RID = ts.ST_RID
 
-- flag rows that were updated
UPDATE #TEMP_SUM6 SET PRESENTFLAG = 1
       FROM #TEMP_SUM6 ts
       JOIN STORE_HISTORY_WEEK6 vw ON ts.HN_RID = vw.HN_RID AND ts.TIME_ID = vw.TIME_ID AND ts.ST_RID = vw.ST_RID
 
-- write new values
INSERT STORE_HISTORY_WEEK6 (HN_RID, TIME_ID, ST_RID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, IN_STOCK_SALES, IN_STOCK_SALES_REG, 
          IN_STOCK_SALES_PROMO, IN_STOCK_SALES_MKDN,  HN_MOD)
          SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID,  ts.SALES,  ts.SALES_REG,  ts.SALES_PROMO, 
           ts.SALES_MKDN,  ts.IN_STOCK_SALES,  ts.IN_STOCK_SALES_REG,  ts.IN_STOCK_SALES_PROMO, 
           ts.IN_STOCK_SALES_MKDN,
		6
	FROM #TEMP_SUM6 ts
	    WHERE PRESENTFLAG = 0
	
truncate table #TEMP_SUM6
	
-- build a temp table of all stock values
INSERT INTO #TEMP_SUM6 (HN_RID, TIME_ID, ST_RID, PRESENTFLAG, 
          STOCK, STOCK_REG, STOCK_MKDN,HN_MOD)
SELECT tmp.HN_RID as HN_RID, tmp.FIRST_DAY_OF_WEEK as TIME_ID, vw.ST_RID as ST_RID, 0 AS PRESENTFLAG, 
          COALESCE(vw.STOCK, 0) as STOCK, COALESCE(vw.STOCK_REG, 0) as STOCK_REG, 
          COALESCE(vw.STOCK_MKDN, 0) as STOCK_MKDN,
	 vw.HN_MOD
         FROM STORE_HISTORY_DAY6 vw with (nolock), #TEMP_ROLLUP_ITEM6 tmp
         WHERE tmp.PROCESS = @PROCESS 
	AND tmp.ITEM_TYPE = @ITEM_TYPE
	AND tmp.PH_RID = @PH_RID
	AND tmp.HOME_LEVEL = @HOME_LEVEL
	AND vw.HN_RID = tmp.HN_RID
	AND vw.TIME_ID = tmp.FIRST_DAY_OF_WEEK
	
	
if @debug <> 0 select * from #TEMP_SUM6
	
-- set weekly values to null incase stock value not posted
UPDATE STORE_HISTORY_WEEK6 with (rowlock)
         SET STOCK = null,
             STOCK_REG = null,
             STOCK_MKDN = null
       	FROM STORE_HISTORY_WEEK6 vw
            JOIN #TEMP_ROLLUP_ITEM6 ri ON vw.HN_RID = ri.HN_RID 
               AND vw.TIME_ID = ri.FIRST_DAY_OF_WEEK 
	
-- update existing values
UPDATE STORE_HISTORY_WEEK6 with (rowlock)
         SET STOCK = ts.STOCK,
             STOCK_REG = ts.STOCK_REG,
             STOCK_MKDN = ts.STOCK_MKDN
       	FROM STORE_HISTORY_WEEK6 vw
            JOIN #TEMP_SUM6 ts ON vw.HN_RID = ts.HN_RID 
               AND vw.TIME_ID = ts.TIME_ID 
               AND vw.ST_RID = ts.ST_RID
 
-- flag rows that were updated
UPDATE #TEMP_SUM6 SET PRESENTFLAG = 1
       FROM #TEMP_SUM6 ts
       JOIN STORE_HISTORY_WEEK6 vw ON ts.HN_RID = vw.HN_RID AND ts.TIME_ID = vw.TIME_ID AND ts.ST_RID = vw.ST_RID
 
-- write new values
INSERT STORE_HISTORY_WEEK6 (HN_RID, TIME_ID, ST_RID, STOCK, STOCK_REG, STOCK_MKDN,  HN_MOD)
          SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID,  ts.STOCK,  ts.STOCK_REG,  ts.STOCK_MKDN, 
		6
	FROM #TEMP_SUM6 ts
	    WHERE PRESENTFLAG = 0
	
	
-- update rollup items that were processed 
	
insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) 
 values(25, 'RILock') 
	
update ROLLUP_ITEM with (rowlock) 
   set ITEM_PROCESSED = '1' 
   from ROLLUP_ITEM ri 
      JOIN #TEMP_ROLLUP_ITEM6 tri      
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
   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM6') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM6
   IF OBJECT_ID('tempdb..#TEMP_JOIN6') IS NOT NULL DROP TABLE #TEMP_JOIN6
   IF OBJECT_ID('tempdb..#TEMP_SUM6') IS NOT NULL DROP TABLE #TEMP_SUM6
SET ANSI_WARNINGS ON
	


GO


