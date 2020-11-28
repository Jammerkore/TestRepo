--dv =============================================
--dv Modified date: 3/18/2014
--dv Description:	Updates boundaries for store grades
--dv History:		TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_UPDATE_BOUNDARY]
	@HN_RID INT,
	@BOUNDARY INT,
	@ORIGINAL_BOUNDARY int 
AS
BEGIN
	SET NOCOUNT ON
    DECLARE  @Loop INT,
             @LoopCount INT,
             @NextLoopCount INT

    SET @LoopCount = 0
	create table #DESCENDANTS(
	       [LOOPCOUNT] [int] NOT NULL,
	       [PARENT_HN_RID] [int] NOT NULL,
	       [HOME_PH_RID] [int],
	       [HN_RID] [int] NOT NULL,
		   PRIMARY KEY CLUSTERED 
           (
            	[LOOPCOUNT],
            	[PARENT_HN_RID],
            	[HN_RID]
           )WITH (IGNORE_DUP_KEY = ON)
           )
           
    -- insert the children of the node into the temp table
    INSERT #DESCENDANTS (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, HN_RID)
    select distinct @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, hnj.HN_RID
    from [dbo].[HIER_NODE_JOIN] hnj with (nolock)
    JOIN [dbo].[HIERARCHY_NODE] hn with (nolock) ON hn.HN_RID = hnj.HN_RID
    left outer join [dbo].STORE_GRADES sg with (nolock) on sg.HN_RID = hnj.HN_RID
    where @HN_RID = hnj.PARENT_HN_RID
      and sg.GRADE_CODE is null
    OPTION (MAXDOP 1)
    
    SET @Loop = @@ROWCOUNT
    
    -- get nodes to update.  Make sure they inherit by checking GRADE_CODE for null 
    SET @NextLoopCount = @LoopCount + 1
    WHILE @Loop > 0
    BEGIN

       INSERT #DESCENDANTS (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, HN_RID)
       select distinct @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, hnj.HN_RID
       from [dbo].[HIER_NODE_JOIN] hnj with (nolock)
       JOIN #DESCENDANTS t ON hnj.PARENT_HN_RID = t.HN_RID and hnj.PH_RID = t.HOME_PH_RID
       JOIN [dbo].[HIERARCHY_NODE] hn with (nolock) ON hn.HN_RID = hnj.HN_RID
       left outer join [dbo].STORE_GRADES sg with (nolock) on sg.HN_RID = hnj.HN_RID
       WHERE t.LOOPCOUNT =  @LoopCount 
         and sg.GRADE_CODE is null
       OPTION (MAXDOP 1)
    
       SET @Loop = @@ROWCOUNT
    
       -- update counters
       SET @LoopCount = @LoopCount + 1
       SET @NextLoopCount = @LoopCount + 1
    
    END
	
	update STORE_GRADES with (rowlock)
	   set BOUNDARY = @BOUNDARY
	 where HN_RID in (select HN_RID from #DESCENDANTS)
	   and BOUNDARY = @ORIGINAL_BOUNDARY

	drop table #DESCENDANTS

	SELECT @@ROWCOUNT;
END

GO