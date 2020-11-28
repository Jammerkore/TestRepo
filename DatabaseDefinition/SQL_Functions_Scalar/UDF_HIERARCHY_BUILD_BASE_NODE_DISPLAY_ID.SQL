--dv Create date: 5/3/2013
--dv Description:	Builds the node description as shown in the merchandise hierarchy
--dv Future Design Considerations: Build the BN_DISPLAY_ID into the BASE_NODE table (slower writes, but faster reads)
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_BUILD_BASE_NODE_DISPLAY_ID]
(
	@PHL_DISPLAY_OPTION_ID int,  --defined in PRODUCT_HIERARCHY_LEVELS (joined on PHL_SEQUENCE=HIERARCHY_NODE.HOME_LEVEL)
	@BN_ID varchar(50),  --defined in BASE_NODE
	@BN_NAME varchar(50), --defined in BASE_NODE
	@BN_DESCRIPTION varchar(250) --defined in BASE_NODE

)
RETURNS varchar(356)  --Max Case: BN_ID(50) + BN_NAME(50) + BN_DESCRIPTION(250) + 6 separator characters
AS
BEGIN
	DECLARE @BN_DISPLAY_ID varchar(356)
	IF (@PHL_DISPLAY_OPTION_ID = 800700) --NameOnly
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_NAME)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800701) --IdOnly
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_ID)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800702) --DescriptionOnly
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_DESCRIPTION)
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800703) --IdAndName
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_ID) + ' [' + RTRIM(@BN_NAME) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800704) --IdAndDescription
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_ID) +  ' [' + RTRIM(@BN_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800705) --NameAndDescription
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_NAME) + ' [' + RTRIM(@BN_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800706) --IdAndNameAndDesc
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_ID) +  ' [' +  RTRIM(@BN_NAME) + ']' + ' [' + RTRIM(@BN_DESCRIPTION) + ']'
		END
	ELSE IF (@PHL_DISPLAY_OPTION_ID = 800707) --DoNotDisplay
		BEGIN
			SET @BN_DISPLAY_ID = ''
		END
	ELSE
		BEGIN
			SET @BN_DISPLAY_ID = RTRIM(@BN_ID) + ' [' + RTRIM(@BN_NAME) + ']'
		END

	RETURN @BN_DISPLAY_ID

END
GO
