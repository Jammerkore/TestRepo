if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_MOD_WK_READ]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_MOD_WK_READ]
GO
 
CREATE PROCEDURE [dbo].[SP_MID_CHN_MOD_WK_READ]
@dt MID_CHN_MOD_WK_READ_TYPE READONLY,
@Rollup CHAR(1) = NULL
AS
SET NOCOUNT ON
DECLARE @Tables INT,
        @Loop INT,
        @HN_TYPE INT,
        @HN_RID INT,
        @FV_RID INT,
        @HN_MOD INT,
        @ROLL_OPTION INT,
        @LoopCount INT,
        @NextLoopCount INT
SET @Tables = 10
SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,
                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID
        FROM HIERARCHY_NODE hn (NOLOCK)
        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
        JOIN @dt t on t.HN_RID = hn.HN_RID
 
 -- build temp table of values and locks for modified version
 create table #TEMPMOD 
 (HN_RID  int not null,
  FV_RID  int not null,
  TIME_ID int not null,
    SALES int   null,
    SALES_REG int   null,
    SALES_PROMO int   null,
    SALES_MKDN int   null,
    STOCK int   null,
    STOCK_REG int   null,
    STOCK_MKDN int   null,
    WOS real   null,
    WOS_REGPROMO real   null,
    SALES_LOCK   char(1) null, 
    SALES_REG_LOCK   char(1) null, 
    SALES_PROMO_LOCK   char(1) null, 
    SALES_MKDN_LOCK   char(1) null, 
    STOCK_LOCK   char(1) null, 
    STOCK_REG_LOCK   char(1) null, 
    STOCK_MKDN_LOCK   char(1) null, 
    WOS_LOCK   char(1) null, 
    WOS_REGPROMO_LOCK   char(1) null)
  
 
 insert into #TEMPMOD
  select cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID,
    cfw.SALES,   cfw.SALES_REG,   cfw.SALES_PROMO,   cfw.SALES_MKDN,   cfw.STOCK, 
    cfw.STOCK_REG,   cfw.STOCK_MKDN,   cfw.WOS,   cfw.WOS_REGPROMO, 
    COALESCE(cfwl.SALES_LOCK, 0) SALES_LOCK, 
    COALESCE(cfwl.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
    COALESCE(cfwl.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
    COALESCE(cfwl.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
    COALESCE(cfwl.STOCK_LOCK, 0) STOCK_LOCK, 
    COALESCE(cfwl.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
    COALESCE(cfwl.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
    COALESCE(cfwl.WOS_LOCK, 0) WOS_LOCK, 
    COALESCE(cfwl.WOS_REGPROMO_LOCK, 0) WOS_REGPROMO_LOCK
   FROM @dt t
   JOIN VW_CHAIN_FORECAST_WEEK cfw (NOLOCK)
    on cfw.HN_RID = t.HN_RID
    and cfw.FV_RID = t.FV_RID
    and cfw.TIME_ID = t.TIME_ID
   left outer join CHAIN_FORECAST_WEEK_LOCK cfwl (NOLOCK)
    on cfwl.HN_RID = cfw.HN_RID
    and cfwl.FV_RID =cfw.FV_RID
    and cfwl.TIME_ID = cfw.TIME_ID
  union
  select cfwl.HN_RID,  cfwl.FV_RID,  cfwl.TIME_ID, 
    cfw.SALES,   cfw.SALES_REG,   cfw.SALES_PROMO,   cfw.SALES_MKDN,   cfw.STOCK, 
    cfw.STOCK_REG,   cfw.STOCK_MKDN,   cfw.WOS,   cfw.WOS_REGPROMO, 
    COALESCE(cfwl.SALES_LOCK, 0) SALES_LOCK, 
    COALESCE(cfwl.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
    COALESCE(cfwl.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
    COALESCE(cfwl.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
    COALESCE(cfwl.STOCK_LOCK, 0) STOCK_LOCK, 
    COALESCE(cfwl.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
    COALESCE(cfwl.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
    COALESCE(cfwl.WOS_LOCK, 0) WOS_LOCK, 
    COALESCE(cfwl.WOS_REGPROMO_LOCK, 0) WOS_REGPROMO_LOCK
 FROM @dt t
 JOIN CHAIN_FORECAST_WEEK_LOCK cfwl (NOLOCK)
  on cfwl.HN_RID = t.HN_RID
  and cfwl.FV_RID = t.FV_RID
  and cfwl.TIME_ID = t.TIME_ID
 left outer join VW_CHAIN_FORECAST_WEEK cfw (NOLOCK)
  on cfw.HN_RID = cfwl.HN_RID
  and cfw.FV_RID =cfwl.FV_RID
  and cfw.TIME_ID = cfwl.TIME_ID
 
 -- create temp table for history values
 create table #TEMPHISTORY 
 (HN_RID  int not null,
  FV_RID  int not null,
  TIME_ID int not null,
    SALES int null, 
    SALES_REG int null, 
    SALES_PROMO int null, 
    SALES_MKDN int null, 
    STOCK int null, 
    STOCK_REG int null, 
    STOCK_MKDN int null, 
    WOS real null, 
    WOS_REGPROMO real null)
 
 -- alternate and real time roll
IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251
   BEGIN
   SET @LoopCount = 0
   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)
   -- insert the children of the node into the temp table
   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) 
       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables
         from HIER_NODE_JOIN hnj (NOLOCK)
           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID
           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
       where @HN_RID = hnj.PARENT_HN_RID
   SET @Loop = @@ROWCOUNT
   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf
   SET @NextLoopCount = @LoopCount + 1
   WHILE @Loop > 0
   BEGIN
      INSERT #TREE
        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables
          from HIER_NODE_JOIN hnj (NOLOCK)
            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID
            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID
        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000
      SET @Loop = @@ROWCOUNT
      SET @LoopCount = @LoopCount + 1
      SET @NextLoopCount = @LoopCount + 1
   END
   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates
   SELECT * 
     INTO #TEMP2
     FROM #TREE
     CROSS JOIN @dt
   where LOOPCOUNT = @LoopCount - 1
	     or PH_TYPE = 800000
       -- build temp table of summed history values 
       insert into #TEMPHISTORY 
       select @HN_RID as HN_RID, @FV_RID as FV_RID, chw.TIME_ID, 
          SUM(chw.SALES) SALES, 
          SUM(chw.SALES_REG) SALES_REG, 
          SUM(chw.SALES_PROMO) SALES_PROMO, 
          SUM(chw.SALES_MKDN) SALES_MKDN, 
          SUM(chw.STOCK) STOCK, 
          SUM(chw.STOCK_REG) STOCK_REG, 
          SUM(chw.STOCK_MKDN) STOCK_MKDN, 
          SUM(chw.WOS) WOS, 
          SUM(chw.WOS_REGPROMO) WOS_REGPROMO
		FROM #TEMP2 t
	    JOIN VW_CHAIN_HISTORY_WEEK chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID
		    AND chw.TIME_ID = t.TIME_ID
	    GROUP BY chw.TIME_ID
 END 
 ELSE 
 BEGIN 
       -- build temp table of history values 
       insert into #TEMPHISTORY 
       select chw.HN_RID,  t.FV_RID as FV_RID, chw.TIME_ID, 
          chw.SALES as SALES, 
          chw.SALES_REG as SALES_REG, 
          chw.SALES_PROMO as SALES_PROMO, 
          chw.SALES_MKDN as SALES_MKDN, 
          chw.STOCK as STOCK, 
          chw.STOCK_REG as STOCK_REG, 
          chw.STOCK_MKDN as STOCK_MKDN, 
          chw.WOS as WOS, 
          chw.WOS_REGPROMO as WOS_REGPROMO
  FROM @dt t
  JOIN VW_CHAIN_HISTORY_WEEK chw (NOLOCK)
    on chw.HN_RID = t.HN_RID
   and chw.TIME_ID = t.TIME_ID
 END 
 
  --select * from #TEMPMOD
  --select * from #TEMPHISTORY
   -- combine modified values with history
 select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID,
          COALESCE(COALESCE(tmpmod.SALES, tmphis.SALES), 0) SALES, 
          COALESCE(COALESCE(tmpmod.SALES_REG, tmphis.SALES_REG), 0) SALES_REG, 
          COALESCE(COALESCE(tmpmod.SALES_PROMO, tmphis.SALES_PROMO), 0) SALES_PROMO, 
          COALESCE(COALESCE(tmpmod.SALES_MKDN, tmphis.SALES_MKDN), 0) SALES_MKDN, 
          COALESCE(COALESCE(tmpmod.STOCK, tmphis.STOCK), 0) STOCK, 
          COALESCE(COALESCE(tmpmod.STOCK_REG, tmphis.STOCK_REG), 0) STOCK_REG, 
          COALESCE(COALESCE(tmpmod.STOCK_MKDN, tmphis.STOCK_MKDN), 0) STOCK_MKDN, 
          COALESCE(COALESCE(tmpmod.WOS, tmphis.WOS), 0) WOS, 
          COALESCE(COALESCE(tmpmod.WOS_REGPROMO, tmphis.WOS_REGPROMO), 0) WOS_REGPROMO, 
          COALESCE(tmpmod.SALES_LOCK, 0) SALES_LOCK, 
          COALESCE(tmpmod.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
          COALESCE(tmpmod.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
          COALESCE(tmpmod.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
          COALESCE(tmpmod.STOCK_LOCK, 0) STOCK_LOCK, 
          COALESCE(tmpmod.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
          COALESCE(tmpmod.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
          COALESCE(tmpmod.WOS_LOCK, 0) WOS_LOCK, 
          COALESCE(tmpmod.WOS_REGPROMO_LOCK, 0) WOS_REGPROMO_LOCK
    into  #TEMPMOD2
 FROM #TEMPMOD tmpmod
 left outer JOIN #TEMPHISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID
 AND tmphis.FV_RID = tmpmod.FV_RID
 AND tmphis.TIME_ID = tmpmod.TIME_ID
 
 -- remove duplicate rows from history table
  delete #TEMPHISTORY 
    from #TEMPHISTORY, #TEMPMOD
    where #TEMPHISTORY.HN_RID = #TEMPMOD.HN_RID
         and #TEMPHISTORY.FV_RID = #TEMPMOD.FV_RID
         and #TEMPHISTORY.TIME_ID = #TEMPMOD.TIME_ID
 
 --select * from #TEMPHISTORY
 insert into #TEMPMOD2
 select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID,
          COALESCE(tmphis.SALES, 0) SALES, 
          COALESCE(tmphis.SALES_REG, 0) SALES_REG, 
          COALESCE(tmphis.SALES_PROMO, 0) SALES_PROMO, 
          COALESCE(tmphis.SALES_MKDN, 0) SALES_MKDN, 
          COALESCE(tmphis.STOCK, 0) STOCK, 
          COALESCE(tmphis.STOCK_REG, 0) STOCK_REG, 
          COALESCE(tmphis.STOCK_MKDN, 0) STOCK_MKDN, 
          COALESCE(tmphis.WOS, 0) WOS, 
          COALESCE(tmphis.WOS_REGPROMO, 0) WOS_REGPROMO, 
          0 as SALES_LOCK, 
          0 as SALES_REG_LOCK, 
          0 as SALES_PROMO_LOCK, 
          0 as SALES_MKDN_LOCK, 
          0 as STOCK_LOCK, 
          0 as STOCK_REG_LOCK, 
          0 as STOCK_MKDN_LOCK, 
          0 as WOS_LOCK, 
          0 as WOS_REGPROMO_LOCK
 from #TEMPHISTORY tmphis
 
 select * from #TEMPMOD2 order by TIME_ID
 
 OPTION (MAXDOP 1)


if (select object_id('tempdb.dbo.#TEMP2')) > 0 DROP TABLE #TEMP2
if (select object_id('tempdb.dbo.#TEMPMOD')) > 0 DROP TABLE #TEMPMOD
if (select object_id('tempdb.dbo.#TEMPMOD2')) > 0 DROP TABLE #TEMPMOD2
if (select object_id('tempdb.dbo.#TEMPHISTORY')) > 0 DROP TABLE #TEMPHISTORY




GO


