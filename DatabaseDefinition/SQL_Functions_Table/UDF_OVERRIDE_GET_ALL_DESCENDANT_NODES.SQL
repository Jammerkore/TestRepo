CREATE FUNCTION [dbo].[UDF_OVERRIDE_GET_ALL_DESCENDANT_NODES] 
(	
	@MODEL_RID INT
)
RETURNS @t TABLE (OFFSET int, HN_RID int, VERSION_RID int, EXCLUDE_IND char COLLATE database_default) 
AS
BEGIN
 declare  @HN_RID AS INT
 DECLARE @Loop INT,
 @LoopCount INT,
 @HIGH_LEVEL_NODE_LEVEL INT,
 @MODEL_HN_RID INT,
 @MODEL_PH_RID INT

 SELECT @MODEL_HN_RID = om.HN_RID
   from OVERRIDE_LL_MODEL_HEADER om with (NOLOCK)
   where om.OLL_RID = @MODEL_RID

 -- get home hierarchy of model node
 SELECT @MODEL_PH_RID = hn.HOME_PH_RID
 from HIERARCHY_NODE hn with (NOLOCK)
 where hn.HN_RID = @MODEL_HN_RID

 -- insert the root node into the temp table (do not use purge values, they will be applied last)
 INSERT @t (OFFSET, HN_RID, VERSION_RID, EXCLUDE_IND)
  select 0, hn.HN_RID, omd.VERSION_RID, omd.EXCLUDE_IND
    from HIERARCHY_NODE hn LEFT OUTER JOIN OVERRIDE_LL_MODEL_DETAIL omd ON hn.HN_RID = omd.HN_RID  and omd.OLL_RID = @MODEL_RID
    where hn.HN_RID = @MODEL_HN_RID

 SET @Loop = @@ROWCOUNT
 SET @LoopCount = 0;
 -- chase all paths until you get to the lowest leaf that belongs to the hierarchy
 WHILE @Loop > 0
 BEGIN
    INSERT @t (OFFSET, HN_RID, VERSION_RID, EXCLUDE_IND)
    select @LoopCount+1, hnj.HN_RID, 
           COALESCE(omd.VERSION_RID, ovr.VERSION_RID),
           COALESCE(omd.EXCLUDE_IND, ovr.EXCLUDE_IND)
      from HIER_NODE_JOIN hnj with (NOLOCK)
      JOIN HIERARCHY_NODE hn with (NOLOCK) ON hn.HN_RID = hnj.HN_RID
      left outer JOIN OVERRIDE_LL_MODEL_HEADER ollmh with (NOLOCK) ON ollmh.OLL_RID = @MODEL_RID
      left outer JOIN OVERRIDE_LL_MODEL_DETAIL omd with (NOLOCK) ON omd.HN_RID = hnj.HN_RID and omd.OLL_RID = @MODEL_RID
      JOIN @t ovr ON hnj.PARENT_HN_RID = ovr.HN_RID and hnj.PH_RID = @MODEL_PH_RID 
       where ovr.OFFSET = @LoopCount
	     AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

    SET @Loop = @@ROWCOUNT
    SET @LoopCount = @LoopCount + 1
       
 END

 return
END

GO