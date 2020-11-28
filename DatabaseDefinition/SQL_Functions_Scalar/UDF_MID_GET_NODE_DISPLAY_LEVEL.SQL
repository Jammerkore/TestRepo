CREATE FUNCTION [dbo].[UDF_MID_GET_NODE_DISPLAY_LEVEL]
(@RID int)
RETURNS varchar(200)
 AS
   BEGIN
      declare @nodeDisplay varchar(200), @nodeID varchar(100), @nodeName varchar(100), @nodeDesc varchar(100)
      declare @HN_TYPE int, @HOME_PH_RID int, @HOME_LEVEL int, @PRODUCT_LEVEL_DISPLAY_ID int, @parentRID int
      declare @PRODUCT_LEVEL_DELIMITER char(1)

      select @HN_TYPE = HN_TYPE, @HOME_PH_RID = HOME_PH_RID, @HOME_LEVEL = HOME_LEVEL from dbo.HIERARCHY_NODE where HN_RID = @RID

      select @PRODUCT_LEVEL_DISPLAY_ID = PRODUCT_LEVEL_DISPLAY_ID, @PRODUCT_LEVEL_DELIMITER = PRODUCT_LEVEL_DELIMITER from dbo.SYSTEM_OPTIONS
      select @PRODUCT_LEVEL_DISPLAY_ID = PHL_DISPLAY_OPTION_ID from dbo.PRODUCT_HIERARCHY_LEVELS where PH_RID = @HOME_PH_RID and PHL_SEQUENCE = @HOME_LEVEL and PHL_DISPLAY_OPTION_ID is not null

      if @HN_TYPE = 800203
      begin
         select @nodeID = cc.COLOR_CODE_ID,
                @nodeName = cc.COLOR_CODE_NAME, @nodeDesc = coalesce(cn.COLOR_DESCRIPTION, ' ')
           from COLOR_NODE cn with (nolock)
           join COLOR_CODE cc (nolock) on cc.COLOR_CODE_RID = cn.COLOR_CODE_RID
           where HN_RID = @RID
      end
      else if @HN_TYPE = 800204
      begin
         select @nodeID = sc.SIZE_CODE_ID, 
               @nodeName = dbo.UDF_MID_GET_SIZE_NAME(sn.SIZE_CODE_RID) 
           from SIZE_NODE sn with (nolock)
           join SIZE_CODE sc (nolock) on sc.SIZE_CODE_RID = sn.SIZE_CODE_RID
           where HN_RID = @RID
           set @nodeDesc = @nodeName
      end
      else
      begin
        select @nodeID = BN_ID, @nodeName = BN_NAME, @nodeDesc = BN_DESCRIPTION from BASE_NODE with (nolock) where HN_RID = @RID
      end
     
      -- display based on following enum values
      --public enum eHierarchyDisplayOptions

      if @PRODUCT_LEVEL_DISPLAY_ID = 800700        --		NameOnly
         set @nodeDisplay = @nodeName
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800701   --		IdOnly 
         set @nodeDisplay = @nodeID
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800702   --		DescriptionOnly 
         set @nodeDisplay = @nodeDesc
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800703   --		IdAndName
         set @nodeDisplay = @nodeID + ' [' + @nodeName + ']'
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800704   --		IdAndDescription 
         set @nodeDisplay = @nodeID + ' [' + @nodeDesc + ']'
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800705   --		NameAndDescription
         set @nodeDisplay = @nodeName + ' [' + @nodeDesc + ']' 
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800706   --		IdAndNameAndDesc
         set @nodeDisplay = @nodeID + ' [' + @nodeName + ']' + ' [' + @nodeDesc + ']'
      else if @PRODUCT_LEVEL_DISPLAY_ID = 800707   --		DoNotDisplay
         set @nodeDisplay = ''
      
      return @nodeDisplay
   END
   
GO