--dv =============================================
--dv Modified date: 2/19/2014
--dv Description:	Returns the size name for a size code
--dv =============================================
CREATE FUNCTION [dbo].[UDF_MID_GET_SIZE_NAME]
(@RID int)
RETURNS varchar(200)
 AS
   BEGIN
      declare @sizeName varchar(200), @sizeID varchar(100), @primary varchar(100), @secondary varchar(100)
      
      select @sizeID = SIZE_CODE_ID, @primary = SIZE_CODE_PRIMARY, @secondary = SIZE_CODE_SECONDARY from dbo.SIZE_CODE where SIZE_CODE_RID = @RID
      
      if @secondary = 'No Secondary Size' or @secondary = 'None'
      begin 
         set @secondary = null
      end
      
      if @primary is not null and @primary != ''
      begin
         if @secondary is not null and @secondary != ''
         begin
            set @sizeName = @primary + @secondary
         end
         else
         begin
            set @sizeName = @primary
         end
      end
      else if @secondary is not null and @secondary != ''
      begin
         set @sizeName = @secondary
      end
      else if @secondary is not null and @secondary != ''
      begin
         set @sizeName = @sizeID
      end
      
      return @sizeName
   END

GO
