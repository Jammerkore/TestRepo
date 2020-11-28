--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure that checks hierarchies for orphans
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_CHECK_HIERARCHIES]
(
	@FIX_HOME_LEVEL CHAR = 'N'
)
 AS
 SET NOCOUNT ON
 DECLARE  @Loop INT,
 @LoopCount INT,
 @NextLoopCount INT,
 @StyleLevel INT,
 @UndefinedLevelType INT,
 @ParentOfStyleLevelType INT,
 @StyleLevelType INT,
 @ColorLevelType INT,
 @SizeLevelType INT,
 @HN_RID INT,
 @LEVEL_TYPE INT,
 @PH_RID INT,
 @ROOT INT,
 @PH_ID VARCHAR(100),
 @PH_TYPE INT,
 @PARENT_HN_RID INT,
 @ID VARCHAR(100), 
 @PARENT_ID VARCHAR(100),
 @LEVEL_ID VARCHAR(50),
 @COMPONENT_RID INT,
 @STYLE_ID VARCHAR(50),
 @COLOR_ID VARCHAR(50),
 @LAST_PARENT_HN_RID INT,
 @LAST_HN_RID INT,
 @LAST_COMPONENT_RID INT,
 @LAST_STYLE_ID VARCHAR(50),
 @LAST_COLOR_ID VARCHAR(50),
 @PARENT_HOME_LEVEL INT,
 @HOME_LEVEL INT,
 @HOME_PH_RID INT,
 @PARENT_HOME_PH_RID INT,
 @rowcount INT,
 @errorsfound INT
 -- level type values
 SET @UndefinedLevelType = 800200
 SET @ParentOfStyleLevelType = 800205
 SET @StyleLevelType = 800202
 SET @ColorLevelType = 800203
 SET @SizeLevelType = 800204
 SELECT @StyleLevel = PHL_SEQUENCE FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] (NOLOCK) where PHL_TYPE = @StyleLevelType
 CREATE TABLE #TREE (LOOPCOUNT INT NULL, PARENT_HN_RID INT NULL, HOME_PH_RID INT, PH_TYPE INT, HN_RID INT NOT NULL,
 HOME_LEVEL INT NOT NULL, LEVEL_TYPE INT, ID VARCHAR(100), PARENT_ID VARCHAR(100), LEVEL_ID VARCHAR(50), COMPONENT_RID INT, STYLE_ID VARCHAR(50), COLOR_ID VARCHAR(50))
 
 --check for orphans
 INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL, ID, LEVEL_ID, LEVEL_TYPE)
 select 0 as LOOPCOUNT, null, hn.HOME_PH_RID, ph.PH_TYPE, hn.HN_RID, hn.HOME_LEVEL, null, null, hn.HN_TYPE
 from [dbo].[HIERARCHY_NODE] hn (NOLOCK)
 JOIN [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
 where hn.HN_RID not in (select HN_RID from [dbo].[HIER_NODE_JOIN] parent (NOLOCK))
 -- add identifying IDs
 --update #TREE set LEVEL_TYPE = (select phl.PHL_TYPE from [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) where #TREE.HOME_PH_RID = phl.PH_RID and #TREE.HOME_LEVEL = phl.PHL_SEQUENCE)
 update #TREE set COMPONENT_RID = (select cn.COLOR_CODE_RID from [dbo].[COLOR_NODE] cn (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.HN_RID = cn.HN_RID)
 update #TREE set STYLE_ID = (select cn.STYLE_NODE_ID from [dbo].[COLOR_NODE] cn (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.HN_RID = cn.HN_RID)
 update #TREE set COMPONENT_RID = (select sn.SIZE_CODE_RID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
 update #TREE set STYLE_ID = (select sn.STYLE_NODE_ID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
 update #TREE set COLOR_ID = (select sn.COLOR_NODE_ID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
 update #TREE set ID = (select bn.BN_ID from [dbo].[BASE_NODE] bn (NOLOCK) where #TREE.LEVEL_TYPE < @ColorLevelType and #TREE.HN_RID = bn.HN_RID)
 update #TREE set ID = (select cc.COLOR_CODE_ID from [dbo].[COLOR_CODE] cc (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.COMPONENT_RID = cc.COLOR_CODE_RID) where ID is null
 update #TREE set ID = (select sc.SIZE_CODE_ID from [dbo].[SIZE_CODE] sc (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.COMPONENT_RID = sc.SIZE_CODE_RID) where ID is null
 update #TREE set LEVEL_ID = (select phl.PHL_ID from [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) where #TREE.HOME_PH_RID = phl.PH_RID and #TREE.HOME_LEVEL = phl.PHL_SEQUENCE)
 update #TREE set LEVEL_ID = 'Alternate' where #TREE.LEVEL_ID is null
 print 'Checking hierarchies for orphans'
 set @errorsfound = 0
 declare C1 cursor for select HN_RID, ID, LEVEL_ID from #TREE
 open C1
 fetch next from C1 into @HN_RID, @ID, @LEVEL_ID
 while @@FETCH_STATUS = 0
   begin
      print 'Orphan found. ' + @LEVEL_ID + ' node ID=' + @ID + '(Key=' + CONVERT(nvarchar, @HN_RID) + ')'
      fetch next from C1 into @HN_RID, @ID, @LEVEL_ID
      set @errorsfound = @errorsfound + 1
   end
 CLOSE C1
 DEALLOCATE C1
 if @errorsfound = 0 print 'No orphans were found'
 
 print ''
 print 'Checking hierarchies for duplicates and invalid Parent/Child relationships'
 CREATE TABLE #HIERARCHIES (PH_RID INT, PH_ID VARCHAR(100), PH_TYPE INT, ROOT INT)

 INSERT #HIERARCHIES (PH_RID, PH_ID, PH_TYPE)
 select PH_RID, PH_ID, PH_TYPE
 from [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK)
 update #HIERARCHIES set ROOT = (select hn.HN_RID from [dbo].[HIERARCHY_NODE] hn (NOLOCK) where hn.HOME_PH_RID != 0 and #HIERARCHIES.PH_RID = hn.HOME_PH_RID and hn.HOME_LEVEL = 0)

 select top 1 @PH_RID = PH_RID, @PH_ID = PH_ID, @ROOT = ROOT, @PH_TYPE = PH_TYPE from #HIERARCHIES
 set @rowcount = @@rowcount

 while @rowcount <> 0
 begin
   print ' '
   print 'Processing hierarchy ' + @PH_ID + ' (Key=' + CONVERT(nvarchar, @PH_RID) + ')'
   truncate table #TREE
   -- add root
   set @LoopCount = 0
   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL, LEVEL_TYPE)
   select @LoopCount as LOOPCOUNT, null as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, @ROOT, hn.HOME_LEVEL, hn.HN_TYPE
     from [dbo].[HIERARCHY_NODE] hn (NOLOCK)
     JOIN [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
     where hn.HN_RID = @ROOT

   -- add IDs
   --update #TREE set LEVEL_TYPE = (select phl.PHL_TYPE from [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) where #TREE.HOME_PH_RID = phl.PH_RID and #TREE.HOME_LEVEL = phl.PHL_SEQUENCE)
   update #TREE set ID = (select ph.PH_ID from [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK) where #TREE.HOME_PH_RID = ph.PH_RID)
   update #TREE set LEVEL_ID = 'Hierarchy' 

   -- chase all paths until you get to the end of the hierarchy
   SET @Loop = @@ROWCOUNT
   SET @NextLoopCount = @LoopCount + 1
   WHILE @Loop > 0
   begin
     INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL, LEVEL_TYPE)
       select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hn.HOME_LEVEL, hn.HN_TYPE
         from [dbo].[HIER_NODE_JOIN] hnj (NOLOCK)
         JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID and hnj.PH_RID = t.HOME_PH_RID
         JOIN [dbo].[HIERARCHY_NODE] hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID
         JOIN [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
         JOIN [dbo].[HIER_NODE_JOIN] parent (NOLOCK) ON hn.HN_RID = parent.HN_RID and parent.PH_RID = hn.HOME_PH_RID
       WHERE t.LOOPCOUNT =  @LoopCount 
         and hnj.PH_RID = @PH_RID
     SET @Loop = @@ROWCOUNT
 
     -- update counters
     SET @LoopCount = @LoopCount + 1
     SET @NextLoopCount = @LoopCount + 1
   end
   -- add additional information
     begin transaction
       --update #TREE set LEVEL_TYPE = (select phl.PHL_TYPE from [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) where #TREE.HOME_PH_RID = phl.PH_RID and #TREE.HOME_LEVEL = phl.PHL_SEQUENCE)
       --update #TREE set LEVEL_TYPE = @UndefinedLevelType where LEVEL_TYPE is null
       update #TREE set COMPONENT_RID = (select cn.COLOR_CODE_RID from [dbo].[COLOR_NODE] cn (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.HN_RID = cn.HN_RID)
       update #TREE set STYLE_ID = (select cn.STYLE_NODE_ID from [dbo].[COLOR_NODE] cn (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.HN_RID = cn.HN_RID)
       update #TREE set COMPONENT_RID = (select sn.SIZE_CODE_RID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
       update #TREE set STYLE_ID = (select sn.STYLE_NODE_ID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
       update #TREE set COLOR_ID = (select sn.COLOR_NODE_ID from [dbo].[SIZE_NODE] sn (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.HN_RID = sn.HN_RID) where COMPONENT_RID is null
       update #TREE set ID = (select bn.BN_ID from [dbo].[BASE_NODE] bn (NOLOCK) where #TREE.LEVEL_TYPE < @ColorLevelType and #TREE.HN_RID = bn.HN_RID)
       update #TREE set ID = (select cc.COLOR_CODE_ID from [dbo].[COLOR_CODE] cc (NOLOCK) where #TREE.LEVEL_TYPE = @ColorLevelType and #TREE.COMPONENT_RID = cc.COLOR_CODE_RID) where ID is null
       update #TREE set ID = (select sc.SIZE_CODE_ID from [dbo].[SIZE_CODE] sc (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.COMPONENT_RID = sc.SIZE_CODE_RID) where ID is null
       update #TREE set LEVEL_ID = (select phl.PHL_ID from [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) where #TREE.HOME_PH_RID = phl.PH_RID and #TREE.HOME_LEVEL = phl.PHL_SEQUENCE)
       update #TREE set LEVEL_ID = 'Alternate' where #TREE.LEVEL_ID is null
       update #TREE set PARENT_ID = (select bn.BN_ID from [dbo].[BASE_NODE] bn (NOLOCK) where #TREE.LEVEL_TYPE < @SizeLevelType and #TREE.PARENT_HN_RID = bn.HN_RID)
       update #TREE set PARENT_ID = (select cc.COLOR_CODE_ID from [dbo].[COLOR_CODE] cc (NOLOCK) where #TREE.LEVEL_TYPE = @SizeLevelType and #TREE.COMPONENT_RID = cc.COLOR_CODE_RID) where PARENT_ID is null
     commit
   
   --check for duplicates and wrong home level
   set @LAST_PARENT_HN_RID = -1
   set @LAST_HN_RID = -1
   set @LAST_COMPONENT_RID = -1
   set @errorsfound = 0
   declare C2 cursor for select PARENT_HN_RID, HN_RID, ID, PARENT_ID, LEVEL_ID, LEVEL_TYPE, COMPONENT_RID, LOOPCOUNT, HOME_LEVEL, HOME_PH_RID, STYLE_ID, COLOR_ID from #TREE order by LOOPCOUNT, PARENT_HN_RID, COMPONENT_RID, HN_RID
   open C2
   fetch next from C2 into @PARENT_HN_RID, @HN_RID, @ID, @PARENT_ID, @LEVEL_ID, @LEVEL_TYPE, @COMPONENT_RID, @LoopCount, @HOME_LEVEL, @HOME_PH_RID, @STYLE_ID, @COLOR_ID
   while @@FETCH_STATUS = 0
     begin
        if  @HOME_PH_RID = @PH_RID and @LoopCount <> @HOME_LEVEL
        begin
          set @errorsfound = @errorsfound + 1
          if @FIX_HOME_LEVEL = 'Y'
          begin
             update HIERARCHY_NODE set HOME_LEVEL = @LoopCount where HN_RID = @HN_RID
             print 'Home level incorrect. ID=' + @ID + '(Key=' + CONVERT(nvarchar, @HN_RID) + ') - level fixed'
          end
          else
          begin
             print 'Home level incorrect. ID=' + @ID + '(Key=' + CONVERT(nvarchar, @HN_RID) + ')'
          end
        end

        if @LEVEL_TYPE = @ColorLevelType 
        begin
            if @LAST_PARENT_HN_RID = @PARENT_HN_RID and @LAST_COMPONENT_RID = @COMPONENT_RID and @LAST_STYLE_ID = @STYLE_ID
            begin
              print 'Duplicate found. Parent ID=' + @PARENT_ID + '(Key=' + CONVERT(nvarchar, @PARENT_HN_RID) + ') : ' + @LEVEL_ID + ' level children ID=' + @STYLE_ID + '\' + @ID + '(Keys=' + CONVERT(nvarchar, @LAST_HN_RID) + ' and ' + CONVERT(nvarchar, @HN_RID) + ')'
              set @errorsfound = @errorsfound + 1
            end
        end
        else
        if @LEVEL_TYPE = @SizeLevelType
        begin
            if @LAST_PARENT_HN_RID = @PARENT_HN_RID and @LAST_COMPONENT_RID = @COMPONENT_RID and @LAST_STYLE_ID = @STYLE_ID and @LAST_COLOR_ID = @COLOR_ID
            begin
              print 'Duplicate found. Parent ID=' + @PARENT_ID + '(Key=' + CONVERT(nvarchar, @PARENT_HN_RID) + ') : ' + @LEVEL_ID + ' level children ID=' + @STYLE_ID + '\' + @COLOR_ID + '\' + @ID + '(Keys=' + CONVERT(nvarchar, @LAST_HN_RID) + ' and ' + CONVERT(nvarchar, @HN_RID) + ')'
              set @errorsfound = @errorsfound + 1
            end
        end
        else
        begin
            if @LAST_PARENT_HN_RID = @PARENT_HN_RID and @LAST_HN_RID = @HN_RID
            begin
              print 'Duplicate found. Parent ID=' + @PARENT_ID + '(Key=' + CONVERT(nvarchar, @PARENT_HN_RID) + ') : ' + @LEVEL_ID + ' level children ID=' + @ID + '(Keys=' + CONVERT(nvarchar, @LAST_HN_RID) + ' and ' + CONVERT(nvarchar, @HN_RID) + ')'
              set @errorsfound = @errorsfound + 1
            end
        end

        set @LAST_PARENT_HN_RID = @PARENT_HN_RID
        set @LAST_HN_RID = @HN_RID
        set @LAST_COMPONENT_RID = @COMPONENT_RID
        set @LAST_STYLE_ID = @STYLE_ID
        set @LAST_COLOR_ID = @COLOR_ID
        fetch next from C2 into @PARENT_HN_RID, @HN_RID, @ID, @PARENT_ID, @LEVEL_ID, @LEVEL_TYPE, @COMPONENT_RID, @LoopCount, @HOME_LEVEL, @HOME_PH_RID, @STYLE_ID, @COLOR_ID
     end
   CLOSE C2
   DEALLOCATE C2

   if @errorsfound = 0 print 'No duplicates found for this hierarchy'

   select hnj.*, parent.HOME_PH_RID as [PARENT_HOME_PH_RID], parent.HOME_LEVEL as [ParentLevel], [dbo].[UDF_MID_GET_NODE_DISPLAY_LEVEL](parent.HN_RID) AS [Parent], 
          child.HOME_LEVEL as [ChildLevel], child.HOME_PH_RID as [CHILD_HOME_PH_RID], [dbo].[UDF_MID_GET_NODE_DISPLAY_LEVEL](hnj.HN_RID) AS [Child]
     into #t
     from HIER_NODE_JOIN hnj with (nolock)
     join HIERARCHY_NODE parent with (nolock) on parent.HN_RID = hnj.PARENT_HN_RID
     join HIERARCHY_NODE child with (nolock) on child.HN_RID = hnj.HN_RID
     join PRODUCT_HIERARCHY ph with (nolock) on ph.PH_RID = hnj.PH_RID
     where hnj.PH_RID = @PH_RID
	   and hnj.PARENT_HN_RID != 0
       and (hnj.PH_RID != parent.HOME_PH_RID or parent.HOME_LEVEL != child.HOME_LEVEL + 1)
     order by parent.HOME_LEVEL

    set @errorsfound = 0
    declare C3 cursor for select PARENT_HN_RID, ParentLevel, PARENT_HOME_PH_RID, HN_RID, ChildLevel, Parent, Child, CHILD_HOME_PH_RID from #t
    open C3
    fetch next from C3 into  @PARENT_HN_RID, @PARENT_HOME_LEVEL, @PARENT_HOME_PH_RID, @HN_RID, @HOME_LEVEL, @PARENT_ID, @ID, @HOME_PH_RID
    while @@FETCH_STATUS = 0
     begin
       if @PARENT_HOME_PH_RID != @PH_RID
       begin
          print 'Invalid Parent/Child hierarchy found. Parent ID=' + @PARENT_ID + '(Key=' + CONVERT(nvarchar, @PARENT_HN_RID) + ') parent home hierarchy ' + CONVERT(nvarchar, @PARENT_HOME_PH_RID) + ' at level ' + CONVERT(nvarchar, @PARENT_HOME_LEVEL) + ' :  Child ID=' + @ID + '(Key=' + CONVERT(nvarchar, @HN_RID) + ') at level ' + CONVERT(nvarchar, @HOME_LEVEL)
          set @errorsfound = @errorsfound + 1
       end
       if @PARENT_HOME_PH_RID = @HOME_PH_RID and @PARENT_HOME_LEVEL != @HOME_LEVEL - 1
       begin
          print 'Invalid Parent/Child levels found. Parent ID=' + @PARENT_ID + '(Key=' + CONVERT(nvarchar, @PARENT_HN_RID) + ') at level ' + CONVERT(nvarchar, @PARENT_HOME_LEVEL) + ' : ' + ' Child ID=' + @ID + '(Key=' + CONVERT(nvarchar, @HN_RID) + ') at level ' + CONVERT(nvarchar, @HOME_LEVEL)
          set @errorsfound = @errorsfound + 1
        end
        fetch next from C3 into  @PARENT_HN_RID, @PARENT_HOME_LEVEL, @PARENT_HOME_PH_RID, @HN_RID, @HOME_LEVEL, @PARENT_ID, @ID, @HOME_PH_RID
     end
    CLOSE C3
    DEALLOCATE C3

    if @errorsfound = 0 print 'No invalid Parent/Child relationships found for this hierarchy'

    drop table #t
   
   --remove the hierarchy from the list
   delete #HIERARCHIES where PH_RID = @PH_RID

   --check for another hierarchy
   select top 1 @PH_RID = PH_RID, @PH_ID = PH_ID, @ROOT = ROOT, @PH_TYPE = PH_TYPE from #HIERARCHIES
   set @rowcount = @@rowcount
 end

if (select object_id('tempdb.dbo.#TREE')) > 0 drop table #TREE
if (select object_id('tempdb.dbo.#HIERARCHIES')) > 0 drop table #HIERARCHIES
if (select object_id('tempdb.dbo.#t')) > 0 drop table #t

GO
