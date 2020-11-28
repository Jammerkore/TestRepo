CREATE FUNCTION [dbo].[UDF_MID_GET_STORE_TEXT]
 (@ST_RID int)
 RETURNS varchar(200)
 AS
 BEGIN
 declare @storeText varchar(200), @storeID varchar(50), @storeName varchar(50), @storeDesc varchar(50)
 declare @STORE_DISPLAY_OPTION_ID int
 declare @optionNameOnly int
 declare @optionIdOnly int
 declare @optionDescriptionOnly int
 declare @optionIdAndName int
 declare @optionIdAndDescription int
 declare @optionNameAndDescription int
 declare @optionIdAndNameAndDesc int
 
 set @optionNameOnly = 800650;
 set @optionIdOnly = 800651;
 set @optionDescriptionOnly = 800652;
 set @optionIdAndName = 800653;
 set @optionIdAndDescription = 800654;
 set @optionNameAndDescription = 800655;
 set @optionIdAndNameAndDesc = 800656;
 
 select @storeID = ST_ID, @storeName = STORE_NAME, @storeDesc = STORE_DESC from dbo.STORES where ST_RID = @ST_RID
 select @STORE_DISPLAY_OPTION_ID = STORE_DISPLAY_OPTION_ID from dbo.SYSTEM_OPTIONS
 
 if @STORE_DISPLAY_OPTION_ID = @optionNameOnly 
begin
set @storeText = @storeName
end
 else if @STORE_DISPLAY_OPTION_ID = @optionIdOnly
begin
set @storeText = @storeID
end
else if @STORE_DISPLAY_OPTION_ID = @optionDescriptionOnly
begin
set @storeText = @storeDesc
end
else if @STORE_DISPLAY_OPTION_ID = @optionIdAndName
begin
set @storeText = @storeID + ' [' + ISNULL(@storeName,'') + ']'
end
else if @STORE_DISPLAY_OPTION_ID = @optionIdAndDescription
begin
set @storeText = @storeID + ' [' + ISNULL(@storeDesc,'') + ']'
end
else if @STORE_DISPLAY_OPTION_ID = @optionNameAndDescription
begin
set @storeText = ISNULL(@storeName,'') + ' [' + ISNULL(@storeDesc,'') + ']'
end
else if @STORE_DISPLAY_OPTION_ID = @optionIdAndNameAndDesc
begin
set @storeText = @storeID + ' [' + ISNULL(@storeName,'') + ']' + ' [' + ISNULL(@storeDesc,'') + ']'
end
 else
begin
set @storeText = @storeName
end
 
 return @storeText 
 END
GO
