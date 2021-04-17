if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_HIS_WK_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_HIS_WK_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_HIS_WK_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 700,
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
  where ri.FV_RID = 1
    AND ri.PROCESS = @PROCESS
    AND ri.ITEM_TYPE = @ITEM_TYPE
    AND ri.PH_RID = @PH_RID
    AND ri.HOME_LEVEL = @HOME_LEVEL
    AND ri.BATCH_NUMBER = @BATCH_NUMBER
    AND ri.ITEM_PROCESSED is null
 
if @debug <> 0 select * from #TEMP_ROLLUP_ITEM0
	
-- clear existing values
UPDATE CHAIN_HISTORY_WEEK with (rowlock)
         SET SALES = null,
             SALES_REG = null,
             SALES_PROMO = null,
             SALES_MKDN = null,
             STOCK = null,
             STOCK_REG = null,
             STOCK_MKDN = null
  from CHAIN_HISTORY_WEEK vw INNER JOIN #TEMP_ROLLUP_ITEM0 tri
          ON vw.HN_RID = tri.HN_RID
         AND vw.TIME_ID = tri.TIME_ID
         AND tri.FV_RID = 1
 
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
	[TIME_ID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT ri.HN_RID, ri.TIME_ID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), sum(SALES_MKDN), 
          sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join CHAIN_HISTORY_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.TIME_ID
	OPTION (MAXDOP 1)
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_CHN_HIS_WK_TYPE	
	
insert into @TEMP_SUM0 (HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, 
          STOCK_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_CHN_HIS_WK_WRITE @TEMP_SUM0	
	
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


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_HIS_WK_NOZERO_ROLLUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_HIS_WK_NOZERO_ROLLUP]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_HIS_WK_NOZERO_ROLLUP]
(
 @PROCESS INT,
 @PH_RID INT,
 @HOME_LEVEL INT,
 @ITEM_TYPE INT = 700,
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
  where ri.FV_RID = 1
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
	[TIME_ID]
)WITH (IGNORE_DUP_KEY = OFF)
)
insert into #TEMP_SUM0 (HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT ri.HN_RID, ri.TIME_ID, sum(SALES), sum(SALES_REG), sum(SALES_PROMO), sum(SALES_MKDN), 
          sum(STOCK), sum(STOCK_REG), sum(STOCK_MKDN)
	FROM #TEMP_ROLLUP_ITEM0 ri
	inner join #TEMP_JOIN0 hnj on hnj.PARENT_HN_RID = ri.HN_RID
	inner join CHAIN_HISTORY_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID
	GROUP BY ri.HN_RID, ri.TIME_ID
	OPTION (MAXDOP 1)
	
if @debug <> 0 select * from #TEMP_SUM0
	
	
declare @TEMP_SUM0 as MID_CHN_HIS_WK_TYPE	
	
insert into @TEMP_SUM0 (HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, 
          STOCK, STOCK_REG, STOCK_MKDN)
SELECT HN_RID, TIME_ID, SALES, SALES_REG, SALES_PROMO, SALES_MKDN, STOCK, STOCK_REG, 
          STOCK_MKDN
	FROM #TEMP_SUM0
	
if @debug <> 0 select * from @TEMP_SUM0
	
	
exec dbo.SP_MID_CHN_HIS_WK_WRITE @TEMP_SUM0	
	
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


