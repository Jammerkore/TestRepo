CREATE FUNCTION [dbo].[UDF_MID_GET_NODE_DISPLAY]
(@RID int)
RETURNS varchar(200)
 AS
   BEGIN
      declare @nodeDisplay varchar(200), @nodeID varchar(100), @nodeName varchar(100), @nodeDesc varchar(100)
      declare @HN_TYPE int, @HOME_PH_RID int, @parentRID int, @grandParentRID int
      declare @PRODUCT_LEVEL_DELIMITER char(1)

      select @HN_TYPE = HN_TYPE, @HOME_PH_RID = HOME_PH_RID from dbo.HIERARCHY_NODE where HN_RID = @RID

      select @PRODUCT_LEVEL_DELIMITER = PRODUCT_LEVEL_DELIMITER from dbo.SYSTEM_OPTIONS

      if @HN_TYPE = 800203
      begin
         select @parentRID = PARENT_HN_RID from HIER_NODE_JOIN hnj with (nolock)
           where hnj.PH_RID = @HOME_PH_RID 
             and hnj.HN_RID = @RID
         set @nodeDisplay = dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@parentRID) + @PRODUCT_LEVEL_DELIMITER + dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@RID)
      end
      else if @HN_TYPE = 800204
      begin
         select @parentRID = PARENT_HN_RID from HIER_NODE_JOIN hnj with (nolock)
           where hnj.PH_RID = @HOME_PH_RID 
             and hnj.HN_RID = @RID
         select @grandParentRID = PARENT_HN_RID from HIER_NODE_JOIN hnj with (nolock)
           where hnj.PH_RID = @HOME_PH_RID 
             and hnj.HN_RID = @parentRID
         set @nodeDisplay = dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@grandParentRID) + @PRODUCT_LEVEL_DELIMITER + dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@parentRID)
             + @PRODUCT_LEVEL_DELIMITER + dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@RID)
      end
      else
      begin
        set @nodeDisplay = dbo.UDF_MID_GET_NODE_DISPLAY_LEVEL (@RID)
      end
      
      return @nodeDisplay
   END

GO
