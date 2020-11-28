CREATE FUNCTION [dbo].[UDF_MID_GET_PARENT]
(@RID int)
RETURNS int
 AS
   BEGIN
      declare @HOME_PH_RID int, @parentRID int

      select @HOME_PH_RID = HOME_PH_RID from dbo.HIERARCHY_NODE where HN_RID = @RID

      select @parentRID = PARENT_HN_RID from HIER_NODE_JOIN hnj with (nolock)
           where hnj.PH_RID = @HOME_PH_RID 
             and hnj.HN_RID = @RID
	     and hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy
      
      return @parentRID
   END

GO
