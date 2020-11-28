--dv Create date: 5/3/2013
--dv Description:	Builds the node description as shown in the merchandise hierarchy
--dv Future Design Considerations: Build the SIZE_DISPLAY_ID into the SIZE_NODE table (slower writes, but faster reads)
--dv Future Design Considerations: Do not support size descriptions and force clients to choose a correct PHL display option for size
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_BUILD_SIZE_NODE_DISPLAY_ID]
(
	@PHL_DISPLAY_OPTION_ID int,  --defined in PRODUCT_HIERARCHY_LEVELS (joined on PHL_SEQUENCE=HIERARCHY_NODE.HOME_LEVEL)
	@SIZE_CODE_ID varchar(50),  --defined in SIZE_CODE
	@SIZE_CODE_PRIMARY varchar(100), --defined in SIZE_CODE
	@SIZE_CODE_SECONDARY varchar(100) --defined in SIZE_CODE
)
RETURNS varchar(456)  --Max Case: @@SIZE_CODE_ID(50) + @SIZE_NAME(200) (defined below) + @SIZE_DESCRIPTION(200) (defined below) + 6 separator characters
AS
BEGIN
	DECLARE @SIZE_DISPLAY_ID varchar(253)
	
	DECLARE @SIZE_NAME varchar(200) --Max Case: @SIZE_CODE_PRIMARY(100) + @SIZE_CODE_SECONDARY(100)
	DECLARE @SIZE_DESCRIPTION varchar(200)

    SET @SIZE_CODE_ID = RTRIM(@SIZE_CODE_ID)    

    IF @SIZE_CODE_SECONDARY = 'No Secondary Size' OR @SIZE_CODE_SECONDARY = 'None' OR @SIZE_CODE_SECONDARY = ''
    BEGIN
        SET @SIZE_CODE_SECONDARY = NULL
    END

	IF @SIZE_CODE_PRIMARY = ''
    BEGIN
        SET @SIZE_CODE_PRIMARY = NULL
    END
      
    IF @SIZE_CODE_PRIMARY IS NOT NULL 
    BEGIN
        IF @SIZE_CODE_SECONDARY IS NOT NULL 
        BEGIN
        SET @SIZE_NAME = @SIZE_CODE_PRIMARY + @SIZE_CODE_SECONDARY 
        END
        ELSE
        BEGIN
        SET @SIZE_NAME = @SIZE_CODE_PRIMARY 
        END
    END
    ELSE IF @SIZE_CODE_SECONDARY IS NOT NULL 
    BEGIN
        SET @SIZE_NAME = @SIZE_CODE_SECONDARY
    END
    ELSE IF @SIZE_CODE_PRIMARY IS NULL AND @SIZE_CODE_SECONDARY IS NULL 
    BEGIN
        SET @SIZE_NAME = @SIZE_CODE_ID
    END

	SET @SIZE_DESCRIPTION = @SIZE_NAME  --Setting the size description to the size name in order to support errant (by the client) display option ids at the size level
      
	IF (@PHL_DISPLAY_OPTION_ID = 800700) --NameOnly
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_NAME)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800701) --IdOnly
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_CODE_ID)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800702) --DescriptionOnly
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_DESCRIPTION)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800703) --IdAndName
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_CODE_ID) + ' [' + RTRIM(@SIZE_NAME) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800704) --IdAndDescription
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_CODE_ID) +  ' [' + RTRIM(@SIZE_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800705) --NameAndDescription
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_NAME) + ' [' + RTRIM(@SIZE_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800706) --IdAndNameAndDesc
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_CODE_ID) +  ' [' +  RTRIM(@SIZE_NAME) + ']' + ' [' + RTRIM(@SIZE_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800707) --DoNotDisplay
		BEGIN
			SET @SIZE_DISPLAY_ID = ''
		END
	ELSE
		BEGIN
			SET @SIZE_DISPLAY_ID = RTRIM(@SIZE_CODE_ID) + ' [' + RTRIM(@SIZE_NAME) + ']'
		END

	RETURN @SIZE_DISPLAY_ID

END
GO
